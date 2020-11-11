using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObsticleQuestEvent : QuestEvent
{
    [SerializeField]GameObject obsticle;

    protected override void StartEvent(){ obsticle.SetActive(true);}
}
