using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingDialog : Dialog
{
    [SerializeField] string[] dialog;

    protected override void PlayDialog(){ StartCoroutine(PlayDialogCoroutine(dialog, true));}
}
