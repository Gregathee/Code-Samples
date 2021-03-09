using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClockSystem : MonoBehaviour
{
    public static ClockSystem Instance = null;
    public static bool StopTime = false;
    [SerializeField] GameObject nextDayMenu = null;
    [SerializeField] GameObject sun;
    [SerializeField] TMP_Text timeText = null;
    [SerializeField] int startHour = 5;
    [SerializeField] int endHour = 2;
    [SerializeField] int sunAngleAdjustment = 180;
    [SerializeField] float timeScale = 1;
    [SerializeField] int fasterDaysMuliplier = 2;
    [SerializeField] bool fasterDays = false;
    List<ItemDispenser> dailyRestockDispensors = new List<ItemDispenser>();
    [SerializeField] Image fadeToBlack = null;
    [SerializeField] Transform spawnPoint = null;
    [SerializeField] Player player = null;
    
    int hour = 5;
    int tensDigit = 0;
    int onesDigit = 0;
    bool AM = true;
    bool advancingTime = false;
    bool nextDayScreen = false;

    void Awake()
    {
        if (!Instance) { Instance = this; }
        else {Destroy(this.gameObject);}
    }

    void Start() { hour = startHour; }
    void Update()
    {
        if(nextDayScreen && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1))){StartNewDay();}
        DisplayTime();
        if (!advancingTime && !GameManager.paused && !StopTime && !nextDayScreen) { StartCoroutine(AdvanceTime()); }
    }
    
    public void RegisterDailyRestock(ItemDispenser dispenser){dailyRestockDispensors.Add(dispenser);}

    void AdjustSunAngle()
    {
        float percentage = (float)(hour + ((float)((tensDigit * 10) + onesDigit) / 60f))/ 24f;
        Vector3 rotation = new Vector3((percentage * 360)-sunAngleAdjustment, -90, 0);
        sun.transform.localEulerAngles = rotation;
    }
    void DisplayTime()
    {
        string text = "";
        AM = (hour < 12);
        if (hour > 12)
        {
            text = (hour - 12).ToString();
        }
        else { text = hour.ToString();}
        text += ":" + tensDigit + "" + onesDigit;
        if (AM) { text += " AM"; }
        else { text += " PM";}
        timeText.text = text;
    }

    public int GetHour() { return hour;}

    public void AdvanceDay()
    {
        CropManager.AdvanceDay();
        foreach(ItemDispenser dispenser in dailyRestockDispensors){dispenser.Restock();}
        nextDayScreen = true;
        StartCoroutine(FadeToBlack());
    }

    public void StartNewDay()
    {
        CustomerManager.Instance.DestroyAllCustomers();
        player.transform.position = spawnPoint.position;
        nextDayMenu.SetActive(false);
        nextDayScreen = false;
        hour = startHour;
        tensDigit = onesDigit = 0;
        StartCoroutine(FadeToBlack(true));
        GameManager.paused = false;
        GameManager.ignorePause = false;
        StopTime = false;

    }

    IEnumerator AdvanceTime()
    {
        advancingTime = true;
        if (timeScale > 0)
        {
            yield return new WaitForSeconds(1f / timeScale);
            if (fasterDays) onesDigit += fasterDaysMuliplier;
            if (++onesDigit > 9)
            {
                onesDigit = 0;
                if (++tensDigit > 5)
                {
                    tensDigit = 0;
                    if (++hour == endHour) { AdvanceDay(); }
                    if (hour > 24) { hour = 1; }
                }
            }
            AdjustSunAngle();
        }
        advancingTime = false;
    }

    IEnumerator FadeToBlack(bool reverse = false)
    {
        Color color = fadeToBlack.color;
        int scale = -1;
        if (!reverse)
        {
            color.a = 0.001f;
            scale *= -1;
        }
        else
        {
            color.a = 0.999f;
        }

        while (color.a < 1 && color.a > 0)
        {
            color.a += 0.005f * scale;
            fadeToBlack.color = color;
            yield return new WaitForEndOfFrame();
        }
        if(!reverse){nextDayMenu.SetActive(true);}
    }
}
