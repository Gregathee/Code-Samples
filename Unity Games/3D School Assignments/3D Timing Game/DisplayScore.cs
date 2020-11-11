/*
 * (Greg Brandt)
 * (Assignment 3)
 * Tracks and displays score
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayScore : MonoBehaviour
{
	public Text textBox;
	public GameObject winText;
	public int score;

	private void Start()
	{
		textBox.text = "Score: 0";
	}

	private void Update()
	{
		textBox.text = "Score: " + score;
		if(score >=5)
		{
			GameObject.FindGameObjectWithTag("HealthSystem").GetComponent<HealthSystem>().gameOver = true;
			winText.SetActive(true);
			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene((SceneManager.GetActiveScene().name));
			}
		}
	}
}
