using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SebastianDialog : Dialog
{
    [SerializeField] string[] beforeGlassesDialog;
    [SerializeField] string[] findOldManDialog;
    [SerializeField] string[] beforeOldManDialog;
    [SerializeField] StringArray[] afterOldManDialog;
    [SerializeField] int beforeGlassesQuestNum = 3;
    [SerializeField] int afterOldManQuestNum = 5;
    int state = 0;

    //Plays dialog based on game state
    protected override void PlayDialog()
    {
        int questNum = QuestManager.questManager.GetPrimaryQuest().GetQuestNumber();
        switch (state)
        {
            case 0:
                if(questNum >= beforeGlassesQuestNum)
                {
                    state = 1;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeGlassesDialog));
                break;
            case 1:
                state = 2;
                StartCoroutine(PlayDialogCoroutine(findOldManDialog));
                break;
            case 2:
                if(questNum >= afterOldManQuestNum)
                {
                    state = 3;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeOldManDialog));
                break;
            case 3:
                int random = Random.Range(0, afterOldManDialog.Length - 1);
                StartCoroutine(PlayDialogCoroutine(afterOldManDialog[random].GetArray()));
                break;
        }
    }
}
