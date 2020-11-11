using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;
using UnityEngine.Networking;
using TMPro;

//Collects data from game play and sends to database
public class DataMiner : MonoBehaviour
{
    public static DataMiner dataMiner;

    List<KeyValuePair<int, int>> questLog = new List<KeyValuePair<int, int>>();
    List<string> questLogStrings = new List<string>();
    string logString = "";
    StreamWriter file;
    int totalDeaths = 0;
    int questDeaths = 0;
    KeyValuePair<int, int> currentTime;
    string adjustedTimeStamp = "";
    const int numberOfLogableQuests = 10;
    public static string workerID = "";
    bool dataHasBeenSent = false;

    private void Awake()
    {
        dataMiner = GameObject.Find("DataMiner").GetComponent<DataMiner>();
    }

    public void AddDeathToLog()
    {
        totalDeaths++;
        questDeaths++;
    }

    public void LogQuestCompletion()
    {
        AdjustTimeStamp(ref questLog);
        questLogStrings.Add(adjustedTimeStamp + ", " + questDeaths + ", " + GameManager.gameManager.GetDifficultyAverage() + ", ");
        questDeaths = 0;
    }

    //Stores timeStamp, modifies adjustedTime to reflect time stamp in seconds
    void AdjustTimeStamp(ref List<KeyValuePair<int, int>> log)
    {
        currentTime = GameManager.gameManager.TimeStamp();
        if (log.Count == 0) log.Add(GameManager.gameManager.TimeStamp());
        else
        {
            int minute = currentTime.Key - log[log.Count - 1].Key;
            int second = currentTime.Value - log[log.Count - 1].Value;
            log.Add(new KeyValuePair<int, int>(minute, second));
        }
        int numberOfSeconds = currentTime.Value + (currentTime.Key * 60);
        adjustedTimeStamp = numberOfSeconds.ToString();
    }

    //ID, pointerVersion, logVersion, funnyVersion, totalTime, totalPauseTime, totalDeaths, totalAverageDifficulty, quest1Time, quest1Deaths, quest1AverageDifficulty...
    public void LogData()
    {
        if (!dataHasBeenSent)
        {
            dataHasBeenSent = true;
            logString += workerID + ", " + GameManager.gameManager.QuestPointerVersion() + ", "
                + GameManager.gameManager.QuestLogVersion() + ", " + GameManager.gameManager.FunnyVersion() + ", ";
            List<KeyValuePair<int, int>> blank = new List<KeyValuePair<int, int>>();
            AdjustTimeStamp(ref blank);
            //               totalTime 
            logString += adjustedTimeStamp + ", " + GameManager.gameManager.GetPauseTime() + ", " +
                totalDeaths + ", " + GameManager.gameManager.GetFinalDifficultyAverage() + ", ";
            //Adds quest data to final log string
            foreach (string s in questLogStrings)
            {
                logString += s;
            }
            //For all quests incomplete, Adds default values in their place
            if (questLogStrings.Count < numberOfLogableQuests)
            {
                for (int i = questLogStrings.Count; i < numberOfLogableQuests; i++)
                {
                    logString += -1 + ", " + -1 + ", " + -1 + ", ";
                }
            }
            StartCoroutine(WriteTextViaPHP(logString));
        }
    }

    IEnumerator WriteTextViaPHP(string data)
    {
        bool successful = true;

        WWWForm form = new WWWForm();
        form.AddField("data", data);
        UnityWebRequest www = UnityWebRequest.Post("https://greg.topher.games/fromunity.php", form);
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            www.SetRequestHeader("User-Agent", "Unity 2019");
        www.SendWebRequest();
        yield return www.isDone;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("good");
        }
    }
}
