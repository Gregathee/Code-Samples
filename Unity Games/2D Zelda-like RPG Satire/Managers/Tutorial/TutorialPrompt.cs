using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class TutorialPrompt : MonoBehaviour
{
    [SerializeField] string promptText = "";
    [SerializeField] TextMeshProUGUI promptTextBox = null;
    [SerializeField] GameObject promptSpace = null;

    public virtual void ActivatePrompt()
    {
        promptSpace.SetActive(true);
        promptTextBox.text = promptText;
        StartCoroutine(ManageTutorial());
    }
    protected abstract IEnumerator ManageTutorial();
    public void DeactivateTutorial(){ promptSpace.SetActive(false);}
}
