/*
 * CropHarvestMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: Manages crop harvest mini game.
 */

using UnityEngine;
using TMPro;

public class CropHarvestMiniGame : MiniGame
{
    //Tool that is currently picked up.
    public static MiniGameTool selectedTool = null;
    [Tooltip("Crops that are needed to be harvest to win the game.")]
    [SerializeField] MiniGameCrop[] crops = null;

    int requiredScore;
    int score = 0;
    bool gameOver = false;

	void Update()
    {
        if(score == requiredScore&&!gameOver) { gameOver = true; EndMiniGameSuccess();  }
    }

    public void IncrementScore() { score++; }
    public void IncrementRequiredScore() { requiredScore++; }
}