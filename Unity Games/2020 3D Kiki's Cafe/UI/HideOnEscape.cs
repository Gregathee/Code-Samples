using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnEscape : MonoBehaviour, IEscapeKeySubscriber
{
    bool escapePressed = false;
    void Start()
    {
        GameManager.RegisterEscapeSubscriber(this, 0);
    }
    void Update()
    {
        if (escapePressed)
        {
            GameManager.SkipNextPause();
            gameObject.SetActive(false);
            escapePressed = false;
            if(GetComponent<SelectableGrid>()){GetComponent<SelectableGrid>().Disable();}
        }
    }
    public void EscapeKeyPressed()
    {
        escapePressed = true;
    }
}
