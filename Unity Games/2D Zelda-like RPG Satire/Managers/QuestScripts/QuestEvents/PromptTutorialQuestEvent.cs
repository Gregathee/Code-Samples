using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptTutorialQuestEvent : QuestEvent
{
    [SerializeField] Tutorial tutorial;

    protected override void StartEvent()
    {
        StartCoroutine(PromptTutorialAfterTalk());
    }

    IEnumerator PromptTutorialAfterTalk()
    {
        yield return new WaitForSeconds(1);
        while(!GameManager.gameManager.EntitiesCanMove())
        {
            yield return null;
        }
        TutorialPromptManager.tutorialPromptManager.ActivateTutorial(tutorial);
    }
}
