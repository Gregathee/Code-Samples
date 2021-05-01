using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    public void StartGame()
    {
        if (inputField.text != "")
        {
            DataMiner.workerID = inputField.text;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { StartGame(); }
    }
}
