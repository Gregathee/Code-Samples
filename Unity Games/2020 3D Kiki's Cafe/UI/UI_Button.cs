using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour, IUI_Selectable
{
    [SerializeField] GameObject cursor = null;
    Button button;

    void Start() { button = GetComponent<Button>(); }

    public void Select(){cursor.SetActive(true);}
    public void Deselect(){cursor.SetActive(false);}
    
    public void Invoke(){button.onClick.Invoke();}
    public string GetDisplayName() { return ""; }


    public void OnPointerClick(PointerEventData eventData) { Invoke(); }
}
