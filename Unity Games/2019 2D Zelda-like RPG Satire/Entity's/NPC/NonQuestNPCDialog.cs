using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonQuestNPCDialog : Dialog
{
    [SerializeField] string[] primaryDialog;
    [SerializeField] string[] secondaryDialog;
    bool hasNotSpoken = true;

    //Plays initial dialog then summary dialog
    protected override void PlayDialog()
    {
        if (hasNotSpoken)
        {
            StartCoroutine(PlayDialogCoroutine(primaryDialog));
            hasNotSpoken = false;
        }
        else StartCoroutine(PlayDialogCoroutine(secondaryDialog));
    }
}
