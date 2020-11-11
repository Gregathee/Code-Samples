/*
 * (Greg Brandt)
 * (Assignment 3)
 * Moves the object forward at a given speed
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40;

	private void Update()
	{
		transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}
}
