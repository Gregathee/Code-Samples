using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;


public class BossSpeech : MovementDirective
{
    [SerializeField] Dialog funnyDialog = null;
    [SerializeField] Dialog notFunnyDialog = null;
    [SerializeField] TextMeshProUGUI textBox = null;
    [SerializeField] GameObject dialogBox = null;
    [SerializeField] Enemy king = null;
    [SerializeField] new GameObject camera = null;
    bool hasPlayedSpeech = false;

    private void Start(){ ConnectEnemyToMovement(king);}

    //Detects game version and prompts speech respectivly
    public override void Move()
    {
        base.Move();
        if (enemy != null)
        {
            if (playerWithInDetectRange && !hasPlayedSpeech)
            {
                if (GameManager.gameManager.FunnyVersion())
                {
                    funnyDialog.PlayDialog(ref textBox, ref dialogBox);
                }
                else
                {
                    notFunnyDialog.PlayDialog(ref textBox, ref dialogBox);
                }
                StartCoroutine( PromptSpeech());
            }
        }
    }

    //Pans camera and prompts dialog
    IEnumerator PromptSpeech()
    {
        camera.SetActive(true);
        hasPlayedSpeech = true;
        while(GameManager.gameManager.NPCIsTalking()) { yield return null; }
        camera.SetActive(false);
        EndPhase();
        GetComponentInParent<BossMovement>().ConcludeSpeech();
    }

    //Intentially blank
    public override void StopCharge() { }
}

