using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTutorial : TutorialPrompt
{
    [SerializeField] GameObject spacebar;
    [SerializeField] int buttonPressesRequired = 3;

    protected override IEnumerator ManageTutorial()
    {
        spacebar.SetActive(true);
        for (int i = 0; i < buttonPressesRequired; i++)
        {
            while (!Input.GetKeyDown(KeyCode.Space) ||GameManager.gameManager.IsPaused()) { yield return null; }
            while (Input.GetKeyUp(KeyCode.Space)) { yield return null; }
        }
        spacebar.SetActive(false);
        DeactivateTutorial();
    }
}
