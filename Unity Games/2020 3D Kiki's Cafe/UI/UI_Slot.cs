using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public abstract class UI_Slot : MonoBehaviour, IUI_Selectable, IPointerClickHandler
{
    [SerializeField] protected Image elementImage;
    [SerializeField] protected Image selectionIndicator = null;
    [SerializeField] protected Color unselectedColor = new Color();
    [SerializeField] protected Color hoverColor = new Color();
    protected Sprite defaultImage = null;

    void Start() { defaultImage = elementImage.sprite; }

    /// <summary>
    /// Move selection cursor to this slot
    /// </summary>
    public virtual void Select() { selectionIndicator.color = hoverColor; }

    /// <summary>
    /// Remove selection cursor from this slot
    /// </summary>
    public virtual void Deselect() { selectionIndicator.color = unselectedColor; }

    public abstract void Invoke();
    public abstract string GetDisplayName();
    

    public virtual void ClearElement()
    {
        elementImage.sprite = defaultImage;
    }
    public void OnPointerClick(PointerEventData eventData){Invoke();}
}
