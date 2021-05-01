using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilSpiritDialog : Dialog
{
    [SerializeField]string[] beforeQuest;
    [SerializeField]string[] afterQuest;
    bool isOnCorrectQuest = false;
    
    //Prompts speech based on if the player has the sword
    protected override void PlayDialog()
    {
        if (QuestManager.questManager.GetPrimaryQuest().GetQuestNumber() == 10)
        {
            isOnCorrectQuest = true;
        }
        if (isOnCorrectQuest && willPromptSpeech)
        {
            StartCoroutine(PlayDialogCoroutine(afterQuest, true));
            willPromptSpeech = false;
        }
        else StartCoroutine(PlayDialogCoroutine(beforeQuest, true));
    }
}
