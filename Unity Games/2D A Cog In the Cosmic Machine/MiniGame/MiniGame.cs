/*
 * MiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Parent class of mini games
 */

using UnityEngine;
using TMPro;

public class MiniGame : MonoBehaviour
{
	[SerializeField] MiniGameType miniGameSceneName;
	[SerializeField] protected float statModification = 1;
	[SerializeField] GameObject gameWinScreen;
	[SerializeField] TMP_Text winText;
	[SerializeField] protected string winMessage;
	[SerializeField] protected string failMessage = "You lose";
    public string[] Successes;
    bool winSound = false;
	protected bool gameOver = false;

    private void Start()
	{
		gameWinScreen.SetActive(false);
        winSound = false;
	}

	public void EndMiniGameEarly()
	{
        OverclockController.instance.EndMiniGame(miniGameSceneName, false);
		AudioManager.instance.PlaySFX("De-Overclock");
		OverclockController.instance.UnloadScene(miniGameSceneName);
	}

    public void EndMiniGameSuccess()
	{
		gameOver = true;
		winText.text = winMessage;

		gameWinScreen.SetActive(true);

		OverclockController.instance.EndMiniGame(miniGameSceneName, true, statModification);

		if (winSound == false)
        {
			if(Successes.Length > 0)
            {
				AudioManager.instance.PlaySFX(Successes[Random.Range(0, Successes.Length - 1)]);
				winSound = true;
			}
        }
    }

	public void EndMiniGameFail(bool modifyStat = false)
	{
		gameOver = true;
		winText.text = failMessage;
		gameWinScreen.SetActive(true);
		if (!modifyStat) { OverclockController.instance.EndMiniGame(miniGameSceneName, false); }
		else { OverclockController.instance.EndMiniGame(miniGameSceneName, false, statModification); }
	}

	public void ConfirmMiniGameSuccess()
	{
		gameWinScreen.SetActive(false);
		OverclockController.instance.UnloadScene(miniGameSceneName);
	}
}