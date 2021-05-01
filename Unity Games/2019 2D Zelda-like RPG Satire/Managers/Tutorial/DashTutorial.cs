using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTutorial : TutorialPrompt
{
    [SerializeField] GameObject shiftButton;


    [SerializeField] int buttonPressesRequired = 3;

    protected override IEnumerator ManageTutorial()
    {
        shiftButton.SetActive(true);
        for (int i = 1; i < buttonPressesRequired; i++)
        {
            while (!Input.GetKeyDown(KeyCode.LeftShift) || GameManager.gameManager.IsPaused()) { yield return null; }
            while (!GameManager.player.dashCooledDown) { yield return null; }
        }
        shiftButton.SetActive(false);
        DeactivateTutorial();
    }
}
