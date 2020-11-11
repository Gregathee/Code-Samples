using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
	public int score = 0;
	public int scoreToWin = 3;
	bool won = false;

	PlayerControllerX player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerX>();
	}

	private void Update()
	{
		if(score >= scoreToWin)
		{
			Debug.Log("Win");
			won = true;
			player.gameOver = true;
		}

		if(!player.gameOver)
		{
			scoreText.text = "Score: " + score;
		}
		else if(won)
		{
			scoreText.text = "You won! Press R to restart";
		}
		else
		{
			scoreText.text = "You lose! Press R to restart";
		}
		if(player.gameOver && Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
			
	}
}
