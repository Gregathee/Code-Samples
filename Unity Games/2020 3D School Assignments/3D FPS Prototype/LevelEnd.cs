/* * (Greg Brandt) 
 * * (Assignment 5) 
 * * Displays "You Win!" upon trigger enter.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    public Text winText;

	private void Start()
	{
		winText.text = "You Win!";
		winText.gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			winText.gameObject.SetActive(true);
		}
	}
}
