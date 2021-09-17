using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{
    [SerializeField] AudioClip goodJobClip = null;
    [SerializeField] AudioClip gameOverClip = null;
    [SerializeField] LightUpButton[] lightUpButtons = null;
    [SerializeField] float startUpSpeed = 1;
    [SerializeField] float speedScale = 1;
    [SerializeField] float sequenceDelay = 1;
    [SerializeField] float maxLightUpTime = 5f;
    [SerializeField] float defaultSpeedCurver = 1.25f;
    [SerializeField] int maxInARow = 3;
    [SerializeField] GameObject gameOverScreen = null;
    [SerializeField] InputField speedScaleIP = null;
    [SerializeField] InputField startUpSpeedIP = null;
    [SerializeField] InputField speedCurveIP = null;
    [SerializeField] Toggle scaleToggle = null;
    [SerializeField] Text text = null;
    [SerializeField] Text scoreText = null;
    
    AudioSource goodJobSource = null;
    AudioSource gameOverSource = null;
    List<int> buttonSequence = new List<int>();
    public static bool playingSequence = false;
    bool buttonLit = false;
    int sequenceIndex = 0;
    float lightUpTime = 1;
    float speedCurve = 1;

    void Start()
    {
        for (int i = 0; i < lightUpButtons.Length; ++i) { lightUpButtons[i].Initialize(this, i); }
        
        // Set texts
        text.text = "";
        speedScaleIP.text = speedScale.ToString();
        startUpSpeedIP.text = startUpSpeed.ToString();
        speedCurveIP.text = defaultSpeedCurver.ToString();
        
        // Set up good job audio source
        GameObject gj = new GameObject();
        goodJobSource = gj.AddComponent<AudioSource>();
        goodJobSource.clip = goodJobClip;
        goodJobSource.volume = 0.5f;
        
        // Set up game over audio source
        GameObject go = new GameObject();
        gameOverSource = go.AddComponent<AudioSource>();
        gameOverSource.clip = gameOverClip;
    }

    /// <summary>
    /// Interpret player game settings and start game
    /// </summary>
    public void SetUp()
    {
        SanitizeInput();
        
        // Get player settings
        speedScale = float.Parse(speedScaleIP.text);
        startUpSpeed = float.Parse(startUpSpeedIP.text);
        speedCurve = float.Parse(speedCurveIP.text);
        
        // assign player settings
        lightUpTime = startUpSpeed;
        Mathf.Clamp(speedScale, 0.01f, 0.99f);
        Mathf.Clamp(lightUpTime, 0.1f, maxLightUpTime);
        
        gameOverScreen.SetActive(false);
        
        // Initialize button sequence
        sequenceIndex = 0;
        buttonSequence.Clear();
        buttonSequence.Add(Random.Range(0, lightUpButtons.Length));
        
        scoreText.text = (buttonSequence.Count - 1).ToString();
        
        StartCoroutine(PlayIntro());
    }

    /// <summary>
    /// Determines if button pressed was the next in the sequence
    /// </summary>
    /// <param name="input"></param>
    public void Input(int input)
    {
        if(input != buttonSequence[sequenceIndex]){GameOver();}
        else
        {
            // If input was the last button in the sequence
            if (++sequenceIndex == buttonSequence.Count)
            {
                goodJobSource.PlayOneShot(goodJobClip);
                
                sequenceIndex = 0;
                int next = Random.Range(0, lightUpButtons.Length);
                int last = buttonSequence[buttonSequence.Count - 1];
                
                // if button sequence is long enough for there to be too many of the same color in a row
                if (buttonSequence.Count >= maxInARow)
                {
                    // Ensure maxInARow is at least 2 to avoid infinite loop
                    if (maxInARow < 2) { maxInARow = 2; }
                    
                    // loop backwards through sequence to determine if too many of the same color played in a row
                    bool targetInARow = false;
                    for (int i = buttonSequence.Count - 2; i >= buttonSequence.Count - maxInARow; --i)
                    {
                        if (buttonSequence[i] != last) { break; }
                        if (i == buttonSequence.Count - maxInARow) { targetInARow = true; }
                    }
                    
                    // Assign next button randomly until it is not the same color that occured too many times in a row
                    while (targetInARow)
                    {
                        next = Random.Range(0, lightUpButtons.Length);
                        targetInARow = next == last;
                    }
                }
                
                buttonSequence.Add(next);
                StartCoroutine(PlaySequence());
                scoreText.text = (buttonSequence.Count - 1).ToString();
            }
        }
    }

    void GameOver()
    {
        gameOverSource.PlayOneShot(gameOverClip);
        text.text = "Game Over. Your score was " + buttonSequence.Count;
        gameOverScreen.SetActive(true);
        StartCoroutine(ShowAd());
    }

    // Show add after 1 second
    IEnumerator ShowAd()
    {
        yield return new WaitForSeconds(1);
        Monetizer.instance.DisplayInterstitialAd();
    }
    
    // Plays the required button press sequence
    IEnumerator PlaySequence()
    {
        playingSequence = true;
        yield return new WaitForSeconds(sequenceDelay);
        for (int i = 0; i < buttonSequence.Count; ++i)
        {
            StartCoroutine(TurnOnLightUpButton(buttonSequence[i]));
            yield return new WaitUntil(() => !buttonLit);
            if(i < buttonSequence.Count-1)yield return new WaitForSeconds(lightUpTime/2);
        }
        if (scaleToggle.isOn) { lightUpTime = startUpSpeed - ( speedScale * (buttonSequence.Count/1.25f));}
        playingSequence = false;
    }

    // Turn on a specified button for lightUpTime seconds
    IEnumerator TurnOnLightUpButton(int index)
    {
        buttonLit = true;
        lightUpButtons[index].TurnOn();
        yield return new WaitForSeconds(lightUpTime);
        lightUpButtons[index].TurnOff();
        buttonLit = false;
    }

    /// <summary>
    /// Loop through all the buttons to indicate the game is starting
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayIntro()
    {
        for (int i = 0; i < lightUpButtons.Length; ++i)
        {
            StartCoroutine(TurnOnLightUpButton(i));
            yield return new WaitUntil(() => !buttonLit);
        }
        for (int i = 0; i < lightUpButtons.Length; ++i)
        {
            StartCoroutine(TurnOnLightUpButton(i));
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(PlaySequence());
    }

    void SanitizeInput()
    {
        if (speedScaleIP.text.Contains("-") || speedScaleIP.text == "") { speedScaleIP.text = 1.ToString(); }
        if (startUpSpeedIP.text.Contains("-") || startUpSpeedIP.text == "") { startUpSpeedIP.text = 1.ToString(); }
        if (speedCurveIP.text.Contains("-") || speedCurveIP.text == "") { speedCurveIP.text = 1.ToString(); }
    }
}
