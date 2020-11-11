/* * (Student Name) 
 * * (Assignment 4) 
 * * Moves object left. 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed = 30f;
    float leftBound = -15;

	private void Update()
	{
		if (!GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().gameOver)
		{
			transform.Translate(Vector3.left * Time.deltaTime * speed);
		}

		if(transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
		{
			Destroy(gameObject);
		}
	}
}
