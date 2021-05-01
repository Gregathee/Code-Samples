using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Dialog : MonoBehaviour
{
    TextMeshProUGUI textBox = null;
    GameObject dialogBox = null;
    protected bool willPromptSpeech = true;

    protected abstract void PlayDialog();
    public bool WillPromptSpeech() { return willPromptSpeech; }

    public void PlayDialog(ref TextMeshProUGUI textBox, ref GameObject dialogBox)
    {
        this.textBox = textBox;
        this.dialogBox = dialogBox;
        PlayDialog();
    }

    //Prompts dialog box and advances if player hits space
    protected IEnumerator PlayDialogCoroutine
        (string[] dialog, bool triggersTutorial, bool isBoss, Tutorial tutorial)
    {
        bool isFirstQuest = GameManager.gameManager.IsFirstQuest();
        if (!GameManager.gameManager.NPCIsTalking())
        {
            GameManager.gameManager.PreventEntityMovement();
            GameManager.gameManager.StartNPCSpeech();
            if (!GameManager.gameManager.devMode)
            {
                for (int i = 0; i < dialog.Length; i++)
                {
                    if (!isFirstQuest && !isBoss)
                    {
                        
                    }
                    dialogBox.SetActive(true);
                    textBox.gameObject.SetActive(true);
                    string tempText = "";
                    textBox.text = dialog[i];
                    
                    foreach (char s in dialog[i])
                    {
                        tempText += s;
                        textBox.text = tempText;
                        if (!Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(0.02f);
                        else yield return new WaitForSeconds(0.01f);
                    }
                    

                    isFirstQuest = false;
                    isBoss = false;
                    while (!Input.GetKey(KeyCode.Space) || GameManager.gameManager.IsPaused())
                    {
                        while (GameManager.gameManager.IsPaused()) { yield return null; }
                        yield return null;
                    }
                    if (Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(0.3f);
                    
                }
            }
            
            dialogBox.SetActive(false);
            textBox.gameObject.SetActive(false);
            GameManager.gameManager.StopNPCSpeech();
            GameManager.gameManager.AllowEntityMovement();
            if (triggersTutorial)
                TutorialPromptManager.tutorialPromptManager.ActivateTutorial(tutorial);
            yield return new WaitForSeconds(1);
        }
    }
    
    protected IEnumerator PlayDialogCoroutine(string[] dialog)
    {
        StartCoroutine(PlayDialogCoroutine(dialog, false, false, Tutorial.Talk));
        yield return null;
    }

    protected IEnumerator PlayDialogCoroutine(string[] dialog, bool isBoss)
    {
        StartCoroutine(PlayDialogCoroutine(dialog, false, true, Tutorial.Talk));
        yield return null;
    }

}
