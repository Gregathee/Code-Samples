using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTutorial : TutorialPrompt
{
    [SerializeField] GameObject spacebar;

    protected override IEnumerator ManageTutorial()
    {
        spacebar.SetActive(true);
        GameObject enemy = GameObject.Find("Training Dummy");
        while (enemy != null)
        {
            yield return null;
        }
        spacebar.SetActive(false);
        DeactivateTutorial();
    }
}
