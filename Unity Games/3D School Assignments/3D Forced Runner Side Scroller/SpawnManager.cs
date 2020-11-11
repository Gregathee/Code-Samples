/* * (Student Name) 
 * * (Assignment 4) 
 * *Spawns prefabs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public GameObject obstaclePrefab;
	Vector3 spawnPosition = new Vector3(40, 0,0);

	float startDelay = 2;
	float repeateRate = 2;

	private void Start()
	{
		StartCoroutine(SpawnObstacles());
	}

	IEnumerator SpawnObstacles()
	{
		yield return new WaitForSeconds(startDelay);
		while(!GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().gameOver)
		{
			yield return new WaitForSeconds(repeateRate);
			if(!GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().gameOver)
			{
				Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);
			}
		}
	}
}
