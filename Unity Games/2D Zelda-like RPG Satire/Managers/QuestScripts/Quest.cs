using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField] string funnyQuestInfo = "";
    [SerializeField] string notFunnyQuestInfo = "";
    [SerializeField] int questNumber = -1;
    [SerializeField] int prereqQuestNumber = -1;
    [SerializeField] Transform targetWaypoint = null;
    [SerializeField] bool isMainQuest;
    [SerializeField] QuestEvent questEvent;
    bool questComplete = false;

    public bool QuestCompleted() { return questComplete; }
    public string GetQuestInfo() 
    {
        if (GameManager.gameManager.FunnyVersion()){ return funnyQuestInfo; }
        else { return notFunnyQuestInfo; }
    }
    public int GetQuestNumber() { return questNumber; }
    public int GetQuestPrereqNumber() { return prereqQuestNumber; }
    public Transform GetQuestTarget() { return targetWaypoint; }
    //Indicates quest completion can be logged
    public bool IsMainQuest() { return isMainQuest; }
    public Transform GetTargetWaypoint() { return targetWaypoint; }
    public void TriggerEvent() { questEvent.TriggerEvent(); }

    public bool CompleteQuest( ref IQuestCompleter questCompleter)
    {
        if(questCompleter.GetQuestCompleterName() == targetWaypoint.name)
        {
            questComplete = true;
        }
        return questComplete;
    }
}
