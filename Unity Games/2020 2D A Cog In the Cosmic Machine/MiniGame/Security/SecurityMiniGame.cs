/*
 * SecurityMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/21/2020 (en-US)
 * Description: Manages Security mini game
 */

using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class SecurityMiniGame : MiniGame
{
    [Tooltip("Uninteractable toggles used to show the player how many successes they need/have.")]
    [SerializeField] Toggle[] successTrackers;
    [SerializeField] CodeBlock[] codeSegments;
    [Tooltip("Text that displays the required code characters.")]
    [SerializeField] TMP_Text codePreview;
    [Tooltip("How long each code character can be seen.")]
    [SerializeField] float displayTime = 1;
    [SerializeField] GameObject tryAgainText;
    [SerializeField] int startCodeLength = 3;
    public int successes = 0;
    string validChars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    string requiredCode = "";
    string availableCode = "";
    string inputCode = "";

    [Tooltip("SFX names")]
    public string[] Correct;
    [Tooltip("SFX names")]
    public string[] Incorrect;
    [Tooltip("SFX names")]
    public string[] DisplaySound;

    void Start() 
    { 
        GenerateCode(); 
        foreach(Toggle toggle in successTrackers) { toggle.isOn = false; }
    }

    void Update()
    {
		if (inputCode.Length == requiredCode.Length && inputCode.Length > 0) 
        {
            foreach (CodeBlock block in codeSegments) { block.gameObject.SetActive(false); }
            if (inputCode == requiredCode)
            {
                inputCode = "";
                successes++;
                AudioManager.instance.PlaySFX(Correct[Random.Range(0, Correct.Length - 1)]);
                for (int i = 0; i < successes; i++) { successTrackers[i].isOn = true; }
                for(int i = successes; i < successTrackers.Length; i++) { successTrackers[i].isOn = false; }
                if (successes == successTrackers.Length) { Debug.Log("win"); EndMiniGameSuccess(); }
                else 
                {
                    Debug.Log("");
                    GenerateCode();
                }
            }
            else 
            {
                inputCode = "";
                AudioManager.instance.PlaySFX(Incorrect[Random.Range(0, Incorrect.Length - 1)]);
                StartCoroutine(PromptTryAgain());
            }
        }
    }

    void ScrambleCodeBlocks()
    {
        for (int i = 0; i < codeSegments.Length - 1; i++)
        {
            CodeBlock block = codeSegments[i];
            int randomIndex = Random.Range(i + 1, codeSegments.Length);
            codeSegments[i] = codeSegments[randomIndex];
            codeSegments[randomIndex] = block;
        }
    }

    void GenerateCode()
    {
        foreach(CodeBlock block in codeSegments) { block.RestetInput(); }
        ScrambleCodeBlocks();
        requiredCode = "";
        availableCode = "";
        foreach (CodeBlock codeSegment in codeSegments)
        {
            string code = validChars[Random.Range(0, validChars.Length)].ToString();
            while (availableCode.Contains(code)) { code = validChars[Random.Range(0, validChars.Length)].ToString(); }
            availableCode += code;
            codeSegment.codeText.text = code;
        }
        for(int i = 0; i < startCodeLength+successes; i++)
		{
            string code = availableCode[Random.Range(0, availableCode.Length)].ToString();
            while (requiredCode.Contains(code)) { code = availableCode[Random.Range(0, availableCode.Length)].ToString(); }
            requiredCode += code;
        }
        StartCoroutine(DisplayCode());
    }

    public void InputCode(string newCode) { inputCode += newCode; }

    IEnumerator PromptTryAgain()
	{
        tryAgainText.SetActive(true);
        yield return new WaitForSeconds(3);
        GenerateCode();
        tryAgainText.SetActive(false);
	}

    IEnumerator DisplayCode()
	{
        foreach (CodeBlock block in codeSegments) { block.gameObject.SetActive(false); }
        codePreview.text = "";
        yield return new WaitForSeconds(1);
        foreach(char codeSegment in requiredCode)
		{
            AudioManager.instance.PlaySFX(DisplaySound[Random.Range(0, DisplaySound.Length - 1)]);
            codePreview.text = codeSegment.ToString();
            yield return new WaitForSeconds(displayTime);
            codePreview.text = "";
        }
        foreach (CodeBlock block in codeSegments) { block.gameObject.SetActive(true); }
        inputCode = "";
    }
}