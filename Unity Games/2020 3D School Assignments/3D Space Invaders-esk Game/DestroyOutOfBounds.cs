/*
 * (Greg Brandt)
 * (Assignment 3)
 * Destroys the game object when out of bounds
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float topBound = 20;
    public float bottomBound = -10;

	HealthSystem healthSystem;

	private void Start()
	{
		healthSystem = GameObject.FindGameObjectWithTag("HealthSystem").GetComponent<HealthSystem>();
	}

	private void Update()
	{
		if(transform.position.z > topBound)
		{
			Destroy(gameObject);
		}
		if(transform.position.z < bottomBound)
		{
			healthSystem.TakeDamage();
			Destroy(gameObject);
		}
	}
}
