using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiQuesEvent : QuestEvent
{
    [SerializeField] QuestEvent[] questEvents;

    protected override void StartEvent()
    {
        foreach(QuestEvent questEvent in questEvents)
        {
            questEvent.TriggerEvent();
        }
    }
}
