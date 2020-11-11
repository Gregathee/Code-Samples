using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpgradeSwordQuestEvent : QuestEvent
{
    [SerializeField] Player oldPlayer;
    [SerializeField] Player newPlayer;
    [SerializeField] GameObject oldCamera;
    [SerializeField] GameObject newCamera;
    [SerializeField] AttackAOE[] AOEs;

    protected override void StartEvent()
    {
       StartCoroutine( StepByStep());
    }

    IEnumerator StepByStep()
    {
        Vector3 oldPos = oldPlayer.transform.position;
        PlayerFacing facing = oldPlayer.GetPlayerFacing();
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        switch (facing)
        {
            case PlayerFacing.Up:
                up = true;
                break;
            case PlayerFacing.Down:
                down = true;
                break;
            case PlayerFacing.Left:
                left = true;
                break;
            case PlayerFacing.Right:
                right = true;
                break;
        }

        GameManager.player = newPlayer;

        foreach (AttackAOE a in AOEs) { a.entityTransform = newPlayer.transform; }
        oldPlayer.gameObject.SetActive(false);
        newPlayer.gameObject.SetActive(true);
        newPlayer.SetActiveAOE(up, down, left, right);
        yield return new WaitForSeconds(0.1f);
        newCamera.SetActive(true);
        oldCamera.SetActive(false);
        newPlayer.transform.position = oldPos;



        
        
        
        QuestManager.questManager.SetQuestPointer(newPlayer.GetComponentInChildren<QuestPointer>());
        
        newPlayer.UpdateLifeBar();
        yield return new WaitForSeconds(1);

    }
}
