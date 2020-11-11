using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GallagherDialog : Dialog
{
    [SerializeField] string[] glassesDialog;
    [SerializeField] string[] beforeGlassesReturnedDialog;
    [SerializeField] string[] findSebastionDialog;
    [SerializeField] string[] beforeSebastionFoundDialog;
    [SerializeField] string[] afterSebastionFoundDialog;
    [SerializeField] StringArray[] afterOldManDialog;
    [SerializeField] int findSebastianQuestNum = 2;
    [SerializeField] int sebastiansQuestNum = 4;
    [SerializeField] int oldManQuestNum = 6;
    int state = 0;

    //Plays dialog based on the game state
    protected override void PlayDialog()
    {
        int questNum = QuestManager.questManager.GetPrimaryQuest().GetQuestNumber();
        switch (state)
        {
            case 0:
                StartCoroutine(PlayDialogCoroutine(glassesDialog, true, false,  Tutorial.Walk));
                state = 1;
                break;
            case 1:
                
                if (questNum >= findSebastianQuestNum)
                {
                    state = 2;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeGlassesReturnedDialog));
                break;
            case 2:
                state = 3;
                StartCoroutine(PlayDialogCoroutine(findSebastionDialog));
                break;
            case 3:
                if (questNum >= sebastiansQuestNum)
                {
                    state = 4;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(beforeSebastionFoundDialog));
                break;
            case 4:
                if (questNum >= oldManQuestNum)
                {
                    state = 5;
                    PlayDialog();
                    break;
                }
                StartCoroutine(PlayDialogCoroutine(afterSebastionFoundDialog));
                break;
            case 5:
                int random = Random.Range(0, afterOldManDialog.Length);
                StartCoroutine(PlayDialogCoroutine(afterOldManDialog[random].GetArray()) );
                break;
        }
        
    }
}
