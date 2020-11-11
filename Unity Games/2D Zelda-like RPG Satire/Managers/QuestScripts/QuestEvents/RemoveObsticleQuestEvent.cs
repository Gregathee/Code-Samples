using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveObsticleQuestEvent : QuestEvent
{
    [SerializeField]GameObject obsticle;
    protected override void StartEvent(){ obsticle.SetActive(false);}
}
