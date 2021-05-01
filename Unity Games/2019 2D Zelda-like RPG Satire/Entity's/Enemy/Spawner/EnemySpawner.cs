using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] EnemySpawnPoint[] enemySpawnPoints = null;
    [SerializeField] int maxEnemyCount = 6;
    bool canSpawn = false;
    bool halfMaxCount = false;
    int enemyCount;

    private void Start(){ StartCoroutine(SpawnEnemies()); }

    //Follow player and spawns enemies
    private void Update()
    {
        transform.position = GameManager.player.transform.position;
        int level = GameManager.gameManager.GetDifficultyLevel();

        //Determine how many enemies can be in the scene based off of difficulty
        switch(level)
        {
            case 1:
                maxEnemyCount = 4;
                break;
            case 2:
                maxEnemyCount = 6;
                break;
            case 3:
                maxEnemyCount = 8;
                break;
            case 4:
                maxEnemyCount = 10;
                break;
            default:
                maxEnemyCount = 5;
                break;
        }
        //Each enemy has 2 child opjects with tag enemy
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length /2;
        //Count does not include unique enemies
        if(GameObject.Find("Training Dummy") != null) { enemyCount--; }
        if (GameObject.Find("King") != null) {  enemyCount--; }
        if (GameObject.Find("EvilSpirit") != null) {  enemyCount--; }
        if (halfMaxCount) maxEnemyCount /= 2;
    }

    public void HalfEnemyCount()
    {
        halfMaxCount = true;
    }

    public void RegCount() { halfMaxCount = false; }

    //Determines random spawn points to spawn from
    IEnumerator SpawnEnemies()
    {
        
        while (!GameManager.gameManager.gameOver)
        {
            int level = GameManager.gameManager.GetDifficultyLevel();
            int random1 = Random.Range(0, enemySpawnPoints.Length);
            int random2 = Random.Range(0, enemySpawnPoints.Length);
            int random3 = Random.Range(0, enemySpawnPoints.Length);
            int random4 = Random.Range(0, enemySpawnPoints.Length);
            if (canSpawn && GameManager.gameManager.EntitiesCanMove() && enemyCount < maxEnemyCount)
            {
                enemySpawnPoints[random1].SpawnEnemy();
                if (level == 2) enemySpawnPoints[random2].SpawnEnemy();
                if (level == 3) enemySpawnPoints[random3].SpawnEnemy();
                if (level == 4) enemySpawnPoints[random4].SpawnEnemy();
            }
            yield return new WaitForSeconds(1);
        }
    }

    //Triggered by quest event
    public void AllowSpawning() { canSpawn = true; }


}
