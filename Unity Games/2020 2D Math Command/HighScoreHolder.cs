using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHolder : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "BEST: " + ScoreManager.Inst.GetHighScore().ToString();
    }
}
