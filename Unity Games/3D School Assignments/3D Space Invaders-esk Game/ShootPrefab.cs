/*
 * (Greg Brandt)
 * (Assignment 3)
 * Instantiates a prefab on Space key down.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPrefab : MonoBehaviour
{
    public GameObject prefabToShoot;

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Instantiate(prefabToShoot, transform.position, prefabToShoot.transform.rotation);
		}
	}
}
