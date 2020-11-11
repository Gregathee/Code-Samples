using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Inst;

    private TextMeshProUGUI scoreUI;

    private ulong currentScore = 0;

    private void Awake()
    {
        if(Inst != null && Inst != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ResetScore()
        => currentScore = 0;

    public void AddScore(int amount)
    {
        currentScore += (ulong)amount;

        if(scoreUI == null)
            scoreUI = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<TextMeshProUGUI>();
        scoreUI.text = currentScore.ToString();

        if(currentScore % 1000 == 0)
            BaseManager.Inst?.RestoreBases();
    }

    public void PostScore()
    {
        if(GetHighScore() < currentScore)
        {
            PlayerPrefs.SetString("HighScore", currentScore.ToString());
            PlayerPrefs.Save();
        }
    }
    public ulong GetHighScore()
    {
        var scoreString = PlayerPrefs.GetString("HighScore");
        ulong.TryParse(scoreString, out ulong curHigh);

        return curHigh;
    }
}
