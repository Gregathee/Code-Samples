using System.Collections.Generic;
using TMPro;
using UnityEngine;


//crated so IUI_Selectables could be assigned via the editor
[System.Serializable]
struct SelectableRow
{
    public GameObject[] selectableObjects;
    public List<IUI_Selectable> selectables;
}

//Enables a grid of UI selectables, mainly for controller support

public class SelectableGrid : MonoBehaviour
{
    [SerializeField] SelectableRow[] selectableRows;
    [SerializeField] bool invertVertical = false;
    int row = 0;
    int column = 0;
    bool enabled = false;
    public Component[] selectable;
    bool displaysNameOnSelect = false;
    TMP_Text displayText = null;

    void Awake()
    {
        //convert assigned gameobjects into IUI_Selectables
        for(int i = 0; i < selectableRows.Length; ++i)
        {
            selectableRows[i].selectables = new List<IUI_Selectable>();
            foreach (GameObject gm in selectableRows[i].selectableObjects)
            {
                selectable = gm.GetComponents(typeof(IUI_Selectable));
                selectableRows[i].selectables.Add(selectable[0] as IUI_Selectable);
                if (selectable[0] == null) { Debug.Log(name);}
                ((IUI_Selectable) selectable[0]).Deselect();
            }
        }
        
    }
    void Update()
    {
        if (!enabled) return;
        NavigateButtons();
        if ((Input.GetKeyDown(KeyCode.E)) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            selectableRows[row].selectables[column].Invoke();
        }
    }

    public void Enable()
    {
        ClockSystem.StopTime = true;
        GameManager.ignorePause = false;
        GameManager.ignorePause = true;
        enabled = true;
        row = 0;
        column = 0;
        if (selectableRows[row].selectables == null) { Debug.Log(selectableRows[row].selectableObjects[0].name);}
        selectableRows[row].selectables[column].Select();
    }

    public void Disable()
    {
        GameManager.SkipNextPause();
        ClockSystem.StopTime = false;
        ClearSelects();
        enabled = false;
    }

    public void EnableDisplayOnSelect(TMP_Text text)
    {
        displaysNameOnSelect = true;
        displayText = text;
    }

    void ClearSelects()
    {
        if (displaysNameOnSelect) { displayText.text = "";}
        foreach (SelectableRow selectableRow in selectableRows)
        {
            foreach (IUI_Selectable selectable in selectableRow.selectables)
            {
                selectable.Deselect();
            }
        }
    }


    public IUI_Selectable GetSelectedSelectable() { return selectableRows[row].selectables[column];}

    //converts grid into a list
    public List<IUI_Selectable> GetSelectables()
    {
        List<IUI_Selectable> selectableList = new List<IUI_Selectable>();
        foreach (SelectableRow selectableRow in selectableRows)
        {
            foreach (IUI_Selectable selectable in selectableRow.selectables)
            {
                selectableList.Add(selectable);
            }
        }
        return selectableList;
    }

    public void SelectSelectable(IUI_Selectable selectable)
    {
        bool quit = false;
        for (int i = 0; i < selectableRows.Length && !quit; ++i)
        {
            for (int j = 0; j < selectableRows[i].selectables.Count && !quit; ++j)
            {
                if (selectable == selectableRows[i].selectables[j])
                {
                    row = i;
                    column = j;
                    quit = true;
                    selectableRows[i].selectables[j].Select();
                }
            }
        }
    }

    public void SelectSelectable( int newRow, int newColumn)
    {
        selectableRows[row].selectables[column].Deselect();
        row = newRow;
        column = newColumn;
        selectableRows[row].selectables[column].Select();
        if (displaysNameOnSelect) { displayText.text = selectableRows[row].selectables[column].GetDisplayName(); }
    }

    void NavigateButtons()
    {
        if (invertVertical)
        {
            if (DirectionalInput.UpPress()) { _down(); }
            if (DirectionalInput.DownPress()) { _up(); }
        }
        else
        {
            if (DirectionalInput.UpPress()) { _up(); }
            if (DirectionalInput.DownPress()) { _down(); }
        }

        if(DirectionalInput.LeftPress()){Left();}
        if(DirectionalInput.RightPress()){Right();}
    }

    void _up()
    {
        if(row + 1 < selectableRows.Length){SelectSelectable(row+1, column);}
    }

    void _down()
    {
        if(row - 1 > -1){SelectSelectable(row-1, column);}
    }

    public void Right()
    {
        if(column + 1 < selectableRows[row].selectables.Count){SelectSelectable(row, column+1);}
    }

    public void Left()
    {
        if(column - 1 > -1){SelectSelectable(row, column-1);}
    }
}
