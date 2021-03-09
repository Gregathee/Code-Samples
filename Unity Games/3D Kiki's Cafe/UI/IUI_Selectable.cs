using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IUI_Selectable
{
    void Select();
    void Deselect();

    void Invoke();

    string GetDisplayName();
}
