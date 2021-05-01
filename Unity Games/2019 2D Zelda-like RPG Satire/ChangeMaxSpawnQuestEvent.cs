using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaxSpawnQuestEvent : QuestEvent
{
    [SerializeField]bool halfMaxCount = false;
    protected override void StartEvent()
    {
        EnemySpawner spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        if(halfMaxCount)
        {
            spawner.HalfEnemyCount();
        }
        else
        {
            spawner.RegCount();
        }
    }
}
