/*
 * AstroidMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/21/2020 (en-US)
 * Description: Manages asteroid mini game and detects win condition
 */

using UnityEngine;
using TMPro;

public class AstroidMiniGame : MiniGame
{
	[Tooltip("Text that flashes to indicate damage was taken.")]
    public GameObject damageText;
	[Tooltip("Number of asteroids to be shot to win the game.")]
    public int requiredAstroids = 10;
	[Tooltip("Display of the number remaining to be shot to win the game.")]
    [SerializeField]TMP_Text scoreText = null;
	[Tooltip("Number of times asteroids can hit the hull to lose the game.")]
	public int damageTillFailure = 3;

	private void Update()
	{
		if (!gameOver)
		{
			scoreText.text = "Astroids Remaining: " + requiredAstroids;
			if (requiredAstroids == 0) { EndMiniGameSuccess(); }
			if (damageTillFailure == 0) { EndMiniGameFail(true); }
		}
	}

	public void TakeDamage() { damageTillFailure--; }
}