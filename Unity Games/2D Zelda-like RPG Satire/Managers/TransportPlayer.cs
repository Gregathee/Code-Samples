using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//Used by doors
public class TransportPlayer : MonoBehaviour
{
    [SerializeField] Transform teleportLocation = null;
    [SerializeField] int lowestQuestNumber1;
    [SerializeField] int highestQuestNumber1;
    [SerializeField] int lowestQuestNumber2;
    [SerializeField] int highestQuestNumber2;
    [SerializeField] bool isEntrance;
    [SerializeField] float questIndicatorDistance = 1.2f;
    [SerializeField] GameObject questIndicator;
    [SerializeField] int quest1;
    [SerializeField] int quest2;
    bool playerOnTeleporter = false ;
    
    private void Start()
    {
        if (highestQuestNumber1 == 0) highestQuestNumber1 = GameManager.gameManager.highestQuestNumber;
        if (highestQuestNumber2 == 0) highestQuestNumber2 = GameManager.gameManager.highestQuestNumber;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerOnTeleporter = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerOnTeleporter = false;
    }

    private void LateUpdate()
    {
        if (!GameObject.Find("GiveFirstQuest"))
        {
            //Allows arrow to point to exit if player quest is not to go in
            if (QuestManager.questManager.GetPrimaryQuest().GetQuestNumber() == quest1 ||
                QuestManager.questManager.GetPrimaryQuest().GetQuestNumber() == quest2 ||
                QuestManager.questManager.isOnTempQuest)
            {
                ManageQuestIndicator();
            }
            else if(questIndicator) questIndicator.SetActive(false);
            if (playerOnTeleporter)
            {
                GameManager.gameManager.TeleportPlayer(teleportLocation.transform);
                ManageTempPointer();
            }
        }
    }


    void ManageQuestIndicator()
    {
        Vector2 playerPosition = GameManager.player.transform.position;
        float distanceFromPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceFromPlayer < questIndicatorDistance && GameManager.gameManager.QuestPointerVersion())
        {
            questIndicator.SetActive(true);
        }
        else if(questIndicator)questIndicator.SetActive(false);
    }

    void ManageTempPointer()
    {
        if (QuestManager.questManager.GetPrimaryQuest().GetQuestNumber() <= highestQuestNumber1)
        {
            ManageTempPointer(lowestQuestNumber1, highestQuestNumber1, teleportLocation.transform);
        }
        else
        {
            ManageTempPointer(lowestQuestNumber2, highestQuestNumber2, teleportLocation.transform);
        }
    }

    void ManageTempPointer(int lowestQuestNumber, int highestQuestNumber, Transform target)
    {
        if (isEntrance)
        {
            QuestManager.questManager.AddTempQuestTarget(lowestQuestNumber, highestQuestNumber, target);
        }
        else
        {
            QuestManager.questManager.RemoveTempQuestTarget();
        }
    }
}
