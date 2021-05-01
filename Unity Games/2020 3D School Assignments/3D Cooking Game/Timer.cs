/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Ends the level after a given time.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text timeText;
    [SerializeField] int requiredSeconds = 120;
    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
	{
        while (requiredSeconds > 0)
        {
            timeText.text = requiredSeconds.ToString();
            yield return new WaitForSeconds(1);
            requiredSeconds--;
        }
        GameManager.Instance.GameOver();
	}
}
