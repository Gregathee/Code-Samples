using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveGallagherGlassesQuestEvent : QuestEvent
{
    [SerializeField] GameObject glasses;
    protected override void StartEvent()
    {
        glasses.SetActive(true);
    }
}
