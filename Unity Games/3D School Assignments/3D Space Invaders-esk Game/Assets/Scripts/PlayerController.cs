/*
 * (Greg Brandt)
 * (Assignment 3)
 * Moves the player side to side with A and D
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float horizontalInput;
	public float speed = 10.0f;
	private float xRange = 14;

	private void Update()
	{
		horizontalInput = Input.GetAxis("Horizontal");
		transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
		if(transform.position.x < -xRange)
		{
			transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
		}
		if (transform.position.x > xRange)
		{
			transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
		}
	}
}
