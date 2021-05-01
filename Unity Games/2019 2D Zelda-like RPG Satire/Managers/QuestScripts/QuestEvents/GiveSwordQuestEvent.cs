using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSwordQuestEvent : QuestEvent
{
    [SerializeField] float difficultyIncrease;
    protected override void StartEvent()
    {
        GameManager.gameManager.AllowSwordSwinging();
        GameManager.gameManager.IncreaseDifficulty(difficultyIncrease);
    }
}
