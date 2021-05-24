using System;
using System.Collections;
using HobbitUtilz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Manages confirmation prompts and warnings
    /// </summary>
    public class TC_UI_ConfirmationManager : MonoBehaviour
    {
        public static TC_UI_ConfirmationManager Instance;
        
        [SerializeField] TMP_Text _prompt;
        [SerializeField] GameObject _confirmButton;
        [SerializeField] TMP_InputField _partNameIP;
        [SerializeField] GameObject _partNameIPObject;
        [SerializeField] GameObject _secondaryConfirmation;
        [SerializeField] TMP_Text _secondaryPrompt;

        public delegate void ConfirmButtonDelegate();
        ConfirmButtonDelegate _confirmButtonDelegate;

        void Update() { if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) Confirm(); }

        public void Initialize()
        {
            if (!Instance) { Instance = this; _confirmButtonDelegate = ClosePrompt; }
            else { Destroy(gameObject);}
        }

        public void PromptMessage
        (
            string message, 
            bool showConfirmButton, 
            bool showInput, 
            ConfirmButtonDelegate 
                confirmationAction = null, 
            string inputText = ""
        )
        {
            if (gameObject.activeInHierarchy)
            {
                _secondaryConfirmation.SetActive(true);
                _secondaryPrompt.text = message;
                return;
            }
            _partNameIP.text = inputText;
            gameObject.SetActive(true);
            _confirmButton.SetActive(showConfirmButton);
            _partNameIPObject.gameObject.SetActive(showInput);
            _prompt.text = message;
            _confirmButtonDelegate = confirmationAction ?? ClosePrompt;
        }

        public string PartName() { return _partNameIP.text;}

        public void Confirm()
        {
            if (_partNameIPObject.gameObject.activeInHierarchy)
            {
                if (!HU_Functions.ValidInput(_partNameIP.text))
                {
                    StartCoroutine(FlashInputField());
                    return;
                }
            }
            _confirmButtonDelegate();
            if(!_secondaryConfirmation.activeInHierarchy){ gameObject.SetActive(false);}
        }

        public void ClosePrompt() { gameObject.SetActive(false); }

        IEnumerator FlashInputField()
        {
            Image ipImage = _partNameIP.GetComponent<Image>();
            Color originalColor = ipImage.color;
            Color alteredColor = originalColor;
            alteredColor.a = 0.5f;
            for (int i = 0; i < 3; i++)
            {
                ipImage.color = alteredColor;
                yield return new WaitForSeconds(0.05f);
                ipImage.color = originalColor;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
