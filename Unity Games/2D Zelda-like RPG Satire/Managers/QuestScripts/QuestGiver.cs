using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGiver : IQuestGiver
{
    [SerializeField] bool hasQuest = false;
    [SerializeField] Quest[] quests = null;
    int questIndex = 0;
    const string name = "unnamed";

    public void SetPrimaryQuest(ref Quest[] quests) { this.quests = quests; }
    public void SetHasQuest(ref bool boolIn) { hasQuest = boolIn; }
    public Quest GiveQuest() {return quests[questIndex];  }
    public void IncrementQuestIndex() {if(questIndex < quests.Length)questIndex++; }
    public int GetQuestNumber() { return quests[questIndex].GetQuestNumber(); }
    public bool HasQuest() { return hasQuest; }
    public string GetName() { return name; }

    public int GetQuestPrereqNumber() 
    {
        if (questIndex < quests.Length)
        {
            return quests[questIndex].GetQuestPrereqNumber();
        }
        else return 0;
    }
    

    

    
}
