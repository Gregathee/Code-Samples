/*
 * (Greg Brandt)
 * (Assignment 3)
 * Destroys the object and it self and adds to score on collision
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
	DisplayScore displayScoreScript;

	private void Start()
	{
		displayScoreScript = GameObject.FindGameObjectWithTag("DisplayScoreText").GetComponent<DisplayScore>();
	}
	private void OnTriggerEnter(Collider other)
	{
		displayScoreScript.score++;
		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}
