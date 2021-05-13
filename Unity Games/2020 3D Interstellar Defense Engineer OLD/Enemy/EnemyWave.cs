using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] Path path = null;
    [SerializeField] Path groundPath = null;
    [SerializeField] Transform groundSpawn = null;
    [SerializeField] List<SpawnContainer> spawnStack = null;
    
    public List<Enemy> spawnedEnemies = new List<Enemy>();
    bool timeForNextEnemy = true;

    private void Update()
    {
        SpawnEnemy();
        if(spawnedEnemies.Count > 0)DetermineEnemyPosistions();
    }

    void DetermineEnemyPosistions()
    {
        int count = spawnedEnemies.Count;
        bool foundDead = false;
        int i = 0;
        while (i < count)
        {
            RemoveDead(ref foundDead, ref count);
            if (!foundDead) { i++; }
            foundDead = false;
        }
        spawnedEnemies.Sort();
        i = 0;
        while (i < count)
        {
            RemoveDead(ref foundDead, ref count);
            if (!foundDead)
            {
                if (spawnedEnemies[i]) spawnedEnemies[i].SetPosition(i);
                i++;
            }
            foundDead = false;
        }
    }

    void RemoveDead(ref bool foundDead, ref int count)
    {
        for (int j = 0; j < spawnedEnemies.Count; j++)
        {
            if (spawnedEnemies[j].IsDead()) { foundDead = true; }
            if (foundDead)
            {
                Enemy enemy = spawnedEnemies[j];
                spawnedEnemies.RemoveAt(j);
                Destroy(enemy.gameObject);
                count = spawnedEnemies.Count;
                break;
            }
        }
    }

    public void SpawnEnemy()
    {
        if(timeForNextEnemy && spawnStack.Count > 0)
        {
            SpawnContainer spawnContainer = spawnStack[0];
            spawnStack.Remove(spawnContainer);
            Enemy spawnedEnemy;
            if (spawnContainer.GetEnemy().flying)
            {
                spawnedEnemy = Instantiate(spawnContainer.GetEnemy(), groundSpawn.position, transform.rotation).GetComponent<Enemy>();
                spawnedEnemy.AssignPath(path);
            }
            else
            {
                spawnedEnemy = Instantiate(spawnContainer.GetEnemy(), groundSpawn.position, transform.rotation).GetComponent<Enemy>();
                spawnedEnemy.AssignPath(groundPath);
            }
                spawnedEnemies.Add(spawnedEnemy);
            
            StartCoroutine(TimeTillNextEnemy(spawnContainer.GetTimeTillNextSpawn()));
        }
    }

    IEnumerator TimeTillNextEnemy(float time)
    {
        timeForNextEnemy = false;
        yield return new WaitForSeconds(time);
        timeForNextEnemy = true;
    }
}
