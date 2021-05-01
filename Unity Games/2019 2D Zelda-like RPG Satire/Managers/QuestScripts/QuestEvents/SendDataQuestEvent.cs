using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendDataQuestEvent : QuestEvent
{
    
    protected override void StartEvent()
    {
        GameManager.gameManager.EndGame(true);
    }
}
