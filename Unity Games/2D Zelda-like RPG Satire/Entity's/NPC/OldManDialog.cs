using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManDialog : Dialog
{
    [SerializeField] string[] beforeSebastianDialog;
    [SerializeField] string[] mainQuestDialog;
    [SerializeField] string[] beforeTrainingDummyDialog;
    [SerializeField] string[] afterTrainingDummyDialog;
    [SerializeField] string[] afterStoryDialog;
    [SerializeField] string[] returnDryCleaningDialog;
    [SerializeField] StringArray[] afterDryCleaningDialog;
    [SerializeField] int afterSebastianQuestNum = 4;
    [SerializeField] int afterTrainingDummyQuestNum = 7;
    [SerializeField] int afterSpiritQuestNum = 11;
    
    int state = 0;

    //Plays dialog based on game state
    protected override void PlayDialog()
    {
        int questNum = QuestManager.questManager.GetPrimaryQuest().GetQuestNumber();
        switch (state)
        {
            case 0:
                if (questNum >= afterSebastianQuestNum)
                {
                    state = 1;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeSebastianDialog));
                break;
            case 1:
                state = 2;
                StartCoroutine(PlayDialogCoroutine(mainQuestDialog));
                break;
            case 2:
                if (questNum >= afterTrainingDummyQuestNum)
                {
                    state = 3;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeTrainingDummyDialog));
                break;
            case 3:
                StartCoroutine(PlayDialogCoroutine(afterTrainingDummyDialog));
                state = 4;
                break;
            case 4:
                if(questNum >= afterSpiritQuestNum)
                {
                    state = 5;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(afterStoryDialog));
                break;
            case 5:
                StartCoroutine(PlayDialogCoroutine(returnDryCleaningDialog));
                state = 6;
                break;

            case 6:
                int random = Random.Range(0, afterDryCleaningDialog.Length - 1);
                StartCoroutine(PlayDialogCoroutine(afterDryCleaningDialog[random].GetArray()));
                break;
        }
    }
}
