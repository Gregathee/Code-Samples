using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocation : MonoBehaviour, IQuestGiver, IQuestCompleter
{
    [SerializeField] QuestGiver questGiver = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            QuestManager.questManager.CompleteQuest(this);
            QuestManager.questManager.GetQuestFromQuestGiver(this);
        }
    }

    public int GetQuestNumber(){return questGiver.GetQuestNumber(); }
    public int GetQuestPrereqNumber() {return questGiver.GetQuestPrereqNumber();}
    public Quest GiveQuest() {return questGiver.GiveQuest();}
    public bool HasQuest() {return questGiver.HasQuest();}
    public string GetQuestCompleterName(){ return gameObject.name;}
    public void IncrementQuestIndex() { questGiver.IncrementQuestIndex(); }
    public string GetName(){ return name;}
}
