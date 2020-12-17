/*
 * AstroidEmitter.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: Spawns asteroids at specified positions
 */

using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AstroidEmitter : MonoBehaviour
{
	[Tooltip("Transforms that positions will used to spawn asteroids.")]
    [SerializeField] Transform[] spawnPoints;
	[Tooltip("Object that will be instatiated at spawn points.")]
    [SerializeField] GameObject astroidPrefab;
	[Tooltip("Lower bound of random range of seconds that an asteroid will spawn after the most recent asteroid was spawwned.")]
	[SerializeField] float minimumSpawnDelay = 2;
	[Tooltip("Upper bound of random range of seconds that an asteroid will spawn after the most recent asteroid was spawwned.")]
	[SerializeField] float maximumSpawnDelay = 5;

	AstroidMiniGame miniGameManager;
	List<int> indexesInUse = new List<int>();

	private void Start()
	{
		miniGameManager = FindObjectOfType<AstroidMiniGame>();
		StartCoroutine(SpawnAstroids());
	}

	IEnumerator SpawnAstroids()
	{
		int index;
		while (miniGameManager.requiredAstroids > 0 && miniGameManager.damageTillFailure > 0)
		{
			yield return new WaitForSeconds(Random.Range(minimumSpawnDelay, maximumSpawnDelay));
			index = Random.Range(0, spawnPoints.Length);

			//Ensure good distribution of spawend asteroids
			if (!indexesInUse.Contains(index))
			{
				indexesInUse.Add(index);
				if(indexesInUse.Count == spawnPoints.Length/2) { indexesInUse.Clear(); }
				Instantiate(astroidPrefab, spawnPoints[index].position, new Quaternion(), transform);
			}
		}
	}
}