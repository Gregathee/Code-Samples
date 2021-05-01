/*
 * (Greg Brandt)
 * (Assignment 3)
 * Spawns prefabs at a given interval
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;

	float leftBound = -14;
	float rightBound = 14;
	float spawnPosZ = 20;

	public HealthSystem healthSystem;

	private void Start()
	{
		healthSystem = GameObject.FindGameObjectWithTag("HealthSystem").GetComponent<HealthSystem>();
		StartCoroutine(SpawnTimer());
	}

	private void Update()
	{
		
	}

	IEnumerator SpawnTimer()
	{
		yield return new WaitForSeconds(3f);
		while(!healthSystem.gameOver)
		{
			SpawnRandomPrefab();
			float randomDelay = Random.Range(0.8f, 2.0f);
			yield return new WaitForSeconds(randomDelay);
		}
	}

	void SpawnRandomPrefab()
	{
		int prefabIndex = Random.Range(0, prefabsToSpawn.Length);
		Vector3 spawnPos = new Vector3(Random.Range(leftBound, rightBound), 0, spawnPosZ);
		Instantiate(prefabsToSpawn[prefabIndex], spawnPos, prefabsToSpawn[0].transform.rotation);
	}
}
