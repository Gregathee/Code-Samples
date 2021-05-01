using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Objects that implement IQuestGiver can be sources of quests
public interface IQuestGiver 
{
    Quest GiveQuest();
    int GetQuestNumber();
    int GetQuestPrereqNumber();
    bool HasQuest();
    void IncrementQuestIndex();
    string GetName();
}
