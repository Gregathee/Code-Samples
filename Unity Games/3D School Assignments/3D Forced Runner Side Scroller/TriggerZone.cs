/* * (Student Name) 
 * * (Assignment 4) 
 * * Detects player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().score++;
		}
	}
}
