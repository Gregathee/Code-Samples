/* * (Student Name) 
 * * (Assignment 4) 
 * * Manages Score
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int score;

    public bool won = false;
	public bool gameOver;

	private void Update()
	{
		if (!gameOver)
		{
			scoreText.text = "Score: " + score;
		}

		if(gameOver && !won)
		{
			scoreText.text  = "You Lose!\nPress R to Try Again!";
		}

		if(score >= 10)
		{
			gameOver = true;
			won = true;
			scoreText.text = "You Win\nPress R to Try Again!";
		}

		if(gameOver && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
