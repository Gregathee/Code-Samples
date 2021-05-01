/*
 * (Greg Brandt)
 * (Assignment 3)
 * Tracks health, game over, and manages the health UI
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
	public int health;
	public int maxHealth;

	public List<Image> hearts;
	public Sprite fullHeart;
	public Sprite emptyHeart;

	public bool gameOver = false;

	public GameObject gameOverText;

	private void Update()
	{
		if (health > maxHealth)
		{
			health = maxHealth;
		}

		for (int i = 0; i < hearts.Count; i++)
		{
			if (i < health)
			{
				hearts[i].sprite = fullHeart;
			}
			else
			{
				hearts[i].sprite = emptyHeart;
			}
		}

		if (health <= 0)
		{
			gameOver = true;
			gameOverText.SetActive(true);

			if (Input.GetKeyDown(KeyCode.R))
			{
				SceneManager.LoadScene((SceneManager.GetActiveScene().name));
			}
		}
	}

	public void TakeDamage()
	{
		health--;
	}
}
