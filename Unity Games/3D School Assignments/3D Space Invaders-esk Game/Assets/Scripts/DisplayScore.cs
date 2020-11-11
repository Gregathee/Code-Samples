/*
 * (Greg Brandt)
 * (Assignment 3)
 * Tracks and displays score
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
	public Text textBox;
	public int score;

		private void Start()
	{
		textBox = GetComponent<Text>();
		textBox.text = "Score: 0";
	}

	private void Update()
	{
		textBox.text = "Score: " + score;
	}
}
