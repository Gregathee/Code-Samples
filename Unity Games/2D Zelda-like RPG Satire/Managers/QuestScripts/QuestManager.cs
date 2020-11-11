using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;
    [SerializeField] TextMeshProUGUI questLog = null;
    [SerializeField] GameObject questLogObject;
    public bool isOnTempQuest = false;
    Quest currentQuest = null;
    QuestPointer questPointer = null;
    bool questLogVersion;

    private void Awake()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    private void Start()
    {
        questPointer = GameObject.Find("QuestPointer").GetComponent<QuestPointer>();
        questLogVersion = GameManager.gameManager.QuestLogVersion();
        if (!questLogVersion) Destroy(questLogObject);
    }

    public void SetQuestPointer(QuestPointer pointer) { questPointer = pointer; }

    public Quest GetPrimaryQuest() { return currentQuest; }

    void AddPrimaryQuest( Quest quest)
    {
        currentQuest = quest;
        questPointer.SetTarget(quest.GetTargetWaypoint());
        questPointer.isOnQuest = true;
        UpdateQuestLog();
    }

    public void CompleteQuest(IQuestCompleter questCompleter)
    {
        if (currentQuest != null)
        {
            if (currentQuest.CompleteQuest(ref questCompleter))
            {
                CompleteQuest(currentQuest.GetQuestNumber());
                currentQuest.TriggerEvent();
            }
        }
    }

    private void CompleteQuest(int questNumber) 
    {
        if (currentQuest.GetQuestNumber() == questNumber)
        {
            bool isMainQuest = currentQuest.IsMainQuest();
            if(isMainQuest) DataMiner.dataMiner.LogQuestCompletion();
        }
        else Debug.Log("Quest can't be completed");
    }

    void UpdateQuestLog()
    {
        if (questLogVersion)
        {
            questLog.text = "";
            questLog.text += (currentQuest.GetQuestInfo() + "\n\t");
        }
    }

    public void GetQuestFromQuestGiver(IQuestGiver questGiver)
    {
        if(currentQuest == null) 
        {
            if (questGiver.GetQuestNumber() == -1)
            { Debug.Log("Quest was not given number"); return; }
            if (questGiver.GetQuestPrereqNumber() == 0 )
            {
                AddPrimaryQuest(questGiver.GiveQuest());
                questGiver.IncrementQuestIndex();
            }
            return; 
        }
        if (questGiver.GetQuestPrereqNumber() == currentQuest.GetQuestNumber() 
            && currentQuest.QuestCompleted())
        {
            AddPrimaryQuest(questGiver.GiveQuest());
            questGiver.IncrementQuestIndex();
        }
    }

    public void AddTempQuestTarget(int lowestQuestNumber, int highestQuestNumber, Transform target)
    {
        if(currentQuest.GetQuestNumber() <= lowestQuestNumber || 
            currentQuest.GetQuestNumber() >= highestQuestNumber)
        {
            questPointer.SetTarget(target);
            isOnTempQuest = true;
        }
    }

    public void RemoveTempQuestTarget()
    {
        questPointer.SetTarget(currentQuest.GetQuestTarget());
        isOnTempQuest = false;
    }
}
