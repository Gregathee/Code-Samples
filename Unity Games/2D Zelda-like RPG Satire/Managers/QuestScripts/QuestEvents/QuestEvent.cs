using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Event that is triggered upon quest completion
public abstract class QuestEvent : MonoBehaviour, IQuestEvent
{
    bool isTriggered = false;
    protected abstract void StartEvent();
    public void TriggerEvent()
    {
        if (!isTriggered)
        {
            isTriggered = true;
            StartEvent();
        }
    }

}
