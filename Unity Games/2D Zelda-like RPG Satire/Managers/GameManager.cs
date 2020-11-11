using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Source of all game state info and modifications
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public static Player player = null;

    [SerializeField] public int highestQuestNumber;
    [SerializeField] float difficultyModifier = 1;
    [SerializeField] float difficultyModifierDecrease = 0.1f;
    [SerializeField] float difficultyModifierIncrease = 0.03f;
    [SerializeField] float deathDifficultyDecreaseAmount = 1f;
    [SerializeField] float difficultyMinimum = 0.1f;
    [SerializeField] float levelTwoBound = 0.5f;
    [SerializeField] float levelThreeBound = 1;
    [SerializeField] float levelFourBound = 1.5f;
    [SerializeField] float enemyDespawnDistance = 5;
    [SerializeField] int levelOneHeartDropRate = 75;
    [SerializeField] int levelTwoHeartDropRate = 50;
    [SerializeField] int levelThreeHeartDropRate = 25;
    public bool devMode = false;
    [SerializeField] GameObject devPortals;
    [SerializeField] GameObject startImage;
    [SerializeField] TextMeshProUGUI timer = null;
    [SerializeField] GameObject pause = null;
    [SerializeField] GameObject timerBox = null;
    [SerializeField] Image deathScreen = null;
    [SerializeField] King king;
    [SerializeField] King evilSpirit;
    [SerializeField] bool questPointerVersion = true;
    [SerializeField] bool questLogVersion = true;
    [SerializeField] bool funnyVersion = true;
    [SerializeField] GameObject endScreen;
    [SerializeField] GameObject congratsText;
    [SerializeField] GameObject devModeText;
    bool entitiesCanMove = true;
    bool isFirstQuest = true;
    bool canSwingSword = true;
    bool isPaused = false;
    bool playerIsDead = false;
    bool npcIsTalking = false;
    [HideInInspector]public bool gameOver = false;
    [HideInInspector] public Animator animator = null;
    [SerializeField] const int timeToPlay = 30;
    [SerializeField]int minute = 0;
    [SerializeField]int second = 0;
    [SerializeField]int pauseSeconds = 0;
    [SerializeField] bool forceDevMode = false;
    int tempTicks = 0;
    int totalTicks = 0;
    float tempTotalDifficulty = 0;
    float totalDifficulty = 0;
    float TempAverageDifficulty;
    float averageDifficutly;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void Start()
    {
        StartCoroutine(StartCountDown());
        if (Application.platform == RuntimePlatform.WebGLPlayer && !forceDevMode)
        { 
            devMode = false;
        }
        if (devMode) 
        {
            devPortals.SetActive(true);
            startImage.SetActive(false);
            GameObject.Find("GiveFirstQuest").GetComponent<GiveFirstQuest>().skipTutorial = true;
            devModeText.SetActive(true);
        }
        else 
        {
            devModeText.SetActive(false);
        }
    }

    
    private void OnApplicationFocus(bool focus)
    {if (!startImage.activeInHierarchy && !devMode) { Pause(); }}

    public bool QuestPointerVersion() { return questPointerVersion; }
    public bool QuestLogVersion() { return questLogVersion; }
    public bool FunnyVersion() { return funnyVersion; }
    public bool EntitiesCanMove() { return entitiesCanMove; }

    public bool NPCIsTalking() { return npcIsTalking; }
    public void StartNPCSpeech() { npcIsTalking = true; }
    public void StopNPCSpeech() { npcIsTalking = false; }
    
    public float GetEnemyDispawnDistance() { return enemyDespawnDistance; }

    public bool IsFirstQuest() 
    {
        if(isFirstQuest)
        {
            isFirstQuest = false;
            return true;
        }
        return false;
    }

    public void PreventEntityMovement() 
    { 
        entitiesCanMove = false;
        animator.SetBool("EntitiesCanMove", false);
    }

    public void AllowEntityMovement() 
    { 
        entitiesCanMove = true;
        animator.SetBool("EntitiesCanMove", true);
    }

    public bool CanSwingSword() { return canSwingSword; }
    public void PreventSwordSwinging() { canSwingSword = false; }
    public void AllowSwordSwinging() { canSwingSword = true; }

    public float GetDifficultyModifier() { return difficultyModifier; }
    public void IncreaseDifficulty() { difficultyModifier += difficultyModifierIncrease; }
    public void IncreaseDifficulty(float increase) { difficultyModifier += increase; }
   
    public void DecreaseDifficulty()
    {
        difficultyModifier -= difficultyModifierDecrease;
        if (difficultyModifier < difficultyMinimum) difficultyModifier = difficultyMinimum;
    }

    public float GetDifficultyAverage()
    {
        float temp = TempAverageDifficulty;
        TempAverageDifficulty = 0;
        tempTicks = 0;
        tempTotalDifficulty = 0;
        return temp;
    }
    public float GetFinalDifficultyAverage() { return averageDifficutly; }

    public int GetHeartDropRate()
    {
        int dropRate = levelOneHeartDropRate;
        if (difficultyModifier > levelTwoBound) dropRate = levelTwoHeartDropRate;
        if (difficultyModifier > levelThreeBound) dropRate = levelThreeHeartDropRate;
        return dropRate;
    }

    public int GetDifficultyLevel()
    {
        int level = 1;
        if (difficultyModifier >= levelTwoBound) level = 2;
        if (difficultyModifier >= levelThreeBound) level = 3;
        if (difficultyModifier >= levelFourBound) level = 4;
        return level;

    }

    public bool IsPaused() { return isPaused; }
    public void Pause()
    {
        GameObject.Find("Pause Button").GetComponentInChildren<TextMeshProUGUI>().text = "Unpause";
        entitiesCanMove = false;
        timerBox.SetActive(true);
        pause.SetActive(true);
        isPaused = true;
    }

    public void Unpause()
    {
        if (!gameOver)
        {
            GameObject.Find("Pause Button").GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
            if (!npcIsTalking) entitiesCanMove = true;
            timerBox.SetActive(false);
            pause.SetActive(false);
            isPaused = false;
        }
    }

    public void PauseSwitch()
    {
        if (isPaused) Unpause();
        else Pause();
    }

    public void TeleportPlayer(Transform teleportLocation)
    {
        float x = teleportLocation.position.x;
        float y = teleportLocation.position.y;
        float z = player.transform.position.z;
        //prevent player from changing z axis on teleport
        Vector3 teleportPos = new Vector3(x, y, z);
        player.transform.position = teleportPos;
    }


    public bool PlayerIsDead() { return playerIsDead; }
    public IEnumerator ShowDeathScreen()
    {
        difficultyModifier -= deathDifficultyDecreaseAmount;
        if (difficultyModifier <= difficultyMinimum) difficultyModifier = difficultyMinimum;
        if(king)king.ResetHealth();
        if(evilSpirit)evilSpirit.ResetHealth();
        entitiesCanMove = false;
        deathScreen.gameObject.SetActive(true);
        float waitTime = 4;
        playerIsDead = true;
        yield return new WaitForSeconds(1);
        deathScreen.CrossFadeColor(new Vector4(0,0,0, 0), waitTime, false, true, true);
        yield return new WaitForSeconds(waitTime);
        deathScreen.gameObject.SetActive(false);
        entitiesCanMove = true;
        playerIsDead = false;
    }

    //Tracks active play time and pause time seperatly 
    //When active time is up the game ends and data is sent
    IEnumerator StartCountDown()
    {
        while (startImage.activeInHierarchy)
        {
            yield return null;
        }
        minute = timeToPlay;
        second = 59;
        while (minute > 0)
        {
            minute--;

            while (second > 0)
            {
                timer.text = (minute + " : " + second);
                yield return new WaitForSeconds(1);
                if (isPaused)
                {
                    pauseSeconds++;
                }
                else
                {
                    second--;
                    tempTotalDifficulty += difficultyModifier;
                    totalDifficulty += difficultyModifier;
                    tempTicks++;
                    totalTicks++;
                    TempAverageDifficulty = tempTotalDifficulty / tempTicks;
                    averageDifficutly = totalDifficulty / totalTicks;
                }
            }
            second = 59;
        }
        EndGame(false);
        
    }

    public void EndGame(bool win)
    {
        gameOver = true;
        Debug.Log("Gameover");
        DataMiner.dataMiner.LogData();
        endScreen.SetActive(true);
        if (win) congratsText.SetActive(true);
        else congratsText.SetActive(false);
        PreventEntityMovement();
    }

    public int GetPauseTime() { return pauseSeconds; }
    public KeyValuePair<int, int> TimeStamp()
    {
        KeyValuePair<int, int> pair = new KeyValuePair<int, int>(timeToPlay - minute, 60 - second);
        return pair;
    }

    [HideInInspector]public bool MobileUp = false;
    [HideInInspector] public bool MobileDown = false;
    [HideInInspector] public bool MobileLeft = false;
    [HideInInspector] public bool MobileRight = false;
    [HideInInspector] public bool MobileSwing = false;
    [HideInInspector] public bool MobileDash = false;
    public void MobileUpEnter() { MobileUp = true; }
    public void MobileUpExit() { MobileUp = false; }
    public void MobileDownEnter() { MobileDown = true; }
    public void MobileDownExit() { MobileDown = false; }
    public void MobileLeftEnter() { MobileLeft = true; }
    public void MobileLeftExit() { MobileLeft = false; }
    public void MobileRightEnter() { MobileRight = true; }
    public void MobileRightExit() { MobileRight = false; }
    public void MobileSwingEnter() { MobileSwing = true; }
    public void MobileSwingExit() { MobileSwing = false; }
    public void MobileDashEnter() { MobileDash = true; }
    public void MobileDashExit() { MobileDash = false; }
}
