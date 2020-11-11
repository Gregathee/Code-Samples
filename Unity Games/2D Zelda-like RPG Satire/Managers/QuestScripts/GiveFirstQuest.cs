using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sets up the intial game state
public class GiveFirstQuest : MonoBehaviour
{
    [SerializeField] NPC npc = null;
    [SerializeField] RespawnPoint respawnPoint = null;
    [SerializeField] GameObject startObject;
    public bool skipTutorial = false;

    private void Update()
    {
        if (!GameManager.gameManager.IsPaused())
        {
            if (!skipTutorial)
            {
                GameManager.gameManager.PreventSwordSwinging();
                GameManager.gameManager.PreventEntityMovement();
            }
            else
            {
                GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().AllowSpawning();
            }
            if (!startObject.activeInHierarchy)
            {
                QuestManager.questManager.GetQuestFromQuestGiver(npc);
                if (!skipTutorial)
                {
                    npc.PromptDialog();
                    TutorialPromptManager.tutorialPromptManager.ActivateTutorial(Tutorial.Talk);
                }
                GameManager.player.SetRespawnPoint(respawnPoint);
                Destroy(gameObject);
            }
        }
    }  
}
