using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPromptManager : MonoBehaviour
{
    public static TutorialPromptManager tutorialPromptManager;
    [SerializeField] TutorialPrompt talkTutorial;
    [SerializeField] TutorialPrompt walkTutorial;
    [SerializeField] TutorialPrompt swordTutorial;
    [SerializeField] TutorialPrompt dashTutorial;

    private void Awake()
    {
        tutorialPromptManager = GameObject.Find("TutorialPromptManager").GetComponent<TutorialPromptManager>();
    }

    public void ActivateTutorial(Tutorial tutorial)
    {
        switch (tutorial)
        {
            case Tutorial.Talk:
                talkTutorial.ActivatePrompt();
                break;
            case Tutorial.Walk:
                walkTutorial.ActivatePrompt();
                break;
            case Tutorial.Sword:
                swordTutorial.ActivatePrompt();
                break;
            case Tutorial.Dash:
                dashTutorial.ActivatePrompt();
                break;
            default:
                break;
        }
    }
}

public enum Tutorial { Talk, Walk, Sword, Dash };

