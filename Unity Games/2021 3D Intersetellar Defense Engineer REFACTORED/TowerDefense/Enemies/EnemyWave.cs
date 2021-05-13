using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemies
{
    [System.Serializable]
    public struct SpawnContainer
    {
        public Enemy Enemy;
        public float TimeTillNextSpawn;
    }
    public class EnemyWave : MonoBehaviour
    {
        public List<Enemy> SpawnedEnemies = new List<Enemy>();
        
        [SerializeField] EnemyPath enemyPath;
        [SerializeField] EnemyPath groundEnemyPath;
        [SerializeField] Transform _groundSpawn;
        [SerializeField] List<SpawnContainer> _spawnStack;

        bool _timeForNextEnemy = true;

        void Update()
        {
            SpawnEnemy();
            if (SpawnedEnemies.Count > 0) DetermineEnemyPositions();
        }

        public void SpawnEnemy()
        {
            if (!_timeForNextEnemy || _spawnStack.Count <= 0) return;
            SpawnContainer spawnContainer = _spawnStack[0];
            _spawnStack.Remove(spawnContainer);
            Enemy spawnedEnemy;
            if (spawnContainer.Enemy.Flying)
            {
                spawnedEnemy = Instantiate(spawnContainer.Enemy, _groundSpawn.position, transform.rotation).GetComponent<Enemy>();
                spawnedEnemy.AssignPath(enemyPath);
            }
            else
            {
                spawnedEnemy = Instantiate(spawnContainer.Enemy, _groundSpawn.position, transform.rotation).GetComponent<Enemy>();
                spawnedEnemy.AssignPath(groundEnemyPath);
            }
            SpawnedEnemies.Add(spawnedEnemy);

            StartCoroutine(TimeTillNextEnemy(spawnContainer.TimeTillNextSpawn));
        }

        void DetermineEnemyPositions()
        {
            int count = SpawnedEnemies.Count;
            bool foundDead = false;
            int i = 0;
            while (i < count)
            {
                RemoveDead(ref foundDead, ref count);
                if (!foundDead) { i++; }
                foundDead = false;
            }
            SpawnedEnemies.Sort();
            i = 0;
            while (i < count)
            {
                RemoveDead(ref foundDead, ref count);
                if (!foundDead)
                {
                    if (SpawnedEnemies[i]) SpawnedEnemies[i].SetPosition(i);
                    i++;
                }
                foundDead = false;
            }
        }

        void RemoveDead(ref bool foundDead, ref int count)
        {
            for (int j = 0; j < SpawnedEnemies.Count; j++)
            {
                if (SpawnedEnemies[j].IsDead()) { foundDead = true; }
                if (foundDead)
                {
                    Enemy enemy = SpawnedEnemies[j];
                    SpawnedEnemies.RemoveAt(j);
                    Destroy(enemy.gameObject);
                    count = SpawnedEnemies.Count;
                    break;
                }
            }
        }

        IEnumerator TimeTillNextEnemy(float time)
        {
            _timeForNextEnemy = false;
            yield return new WaitForSeconds(time);
            _timeForNextEnemy = true;
        }
    }
}