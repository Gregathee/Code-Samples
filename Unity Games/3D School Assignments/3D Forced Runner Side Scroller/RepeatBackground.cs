/* * (Student Name) 
 * * (Assignment 4) 
 * * Makes background repeat
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    Vector3 startPosition;
	float repeatWidth;

	private void Start()
	{
		startPosition = transform.position;
		repeatWidth = GetComponent<BoxCollider>().size.x / 2;

	}

	private void Update()
	{
		if(transform.position.x < startPosition.x - repeatWidth)
		{
			transform.position = startPosition;
		}
	}
}
