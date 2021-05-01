using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject[] enemies = null;
    [SerializeField] bool canSpawn = true;

    public void SpawnEnemy()
    {
        int enemyIndex = Random.Range(0, enemies.Length);
        if (canSpawn)
        {
            Instantiate(enemies[enemyIndex], transform.position, new Quaternion());
        }
    }

    //Prevents enemies from spawning in walls
    private void Update()
    {
        Collider2D collider = GetComponent<Collider2D>();
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();

        if(collider.OverlapCollider(filter.NoFilter(), colliders) > 0)
        {
            canSpawn = false;
        }
        else { canSpawn = true; }
    }
}
