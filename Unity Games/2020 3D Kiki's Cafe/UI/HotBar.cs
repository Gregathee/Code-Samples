using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotBar : MonoBehaviour
{
    [SerializeField] SelectableGrid slots= null;
    [SerializeField] TMP_Text displayText = null;
    public static InventorySlot selectedSlot = null;
    Inventory inventory;
    static int slotIndex = 0;
    bool freezeHotkeys = false;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        slots.EnableDisplayOnSelect(displayText);
        slots.SelectSelectable(0,0);
    }
    
    void Update()
    {
        if (freezeHotkeys) {return;}
        SelectSlotFromHotKeys();
        if(Input.GetKeyDown(KeyCode.Joystick1Button7)){slots.Right();}
        if(Input.GetKeyDown(KeyCode.Joystick1Button6)){slots.Left();}
    }

    public bool Frozen() { return freezeHotkeys;}

    /// <summary>
    /// Prevent directional input from selecting hotbar slots
    /// </summary>
    public void FreezeHotkeys()
    {
        freezeHotkeys = true;
    }

    /// <summary>
    /// Allow directional input from selecting hotbar slots
    /// </summary>
    public void UnfreezeHotkeys()
    {
        freezeHotkeys = false;
    }

    /// <summary>
    /// Returns the value of available space the hotbar has for a particular item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetAvailableSpaceForItem(Item item)
    {
        int total = 0;
        foreach (IUI_Selectable slot in slots.GetSelectables())
        {
            if (((InventorySlot)slot).GetItem() == null) { total += item.GetStackSize(); }
            else if (((InventorySlot)slot).GetItem() == item) { total += item.GetStackSize() - ((InventorySlot)slot).GetItemCount();}
        }
        return total;
    }

    /// <summary>
    /// Remove selection cursor from currently selected slot
    /// </summary>
    public void DeselectSlot() { slots.GetSelectedSelectable().Deselect(); }

    /// <summary>
    /// Select a slot with it's index
    /// </summary>
    /// <param name="slotNumber"></param>
    public void SelectSlot(int slotNumber)
    {
        slots.SelectSelectable(0, slotNumber);
    }

    public InventorySlot GetSelectedSlot() { return (InventorySlot)slots.GetSelectedSelectable();}

    public int GetSelectedSlotIndex()
    {
        int i = 0;
        foreach (IUI_Selectable slot in slots.GetSelectables())
        {
            if (slot == slots.GetSelectedSelectable()) { return i;} ++i;
        }
        return 0;
    }

    void SelectSlotFromHotKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || D_Pad.UpPress() && !Input.GetKey(KeyCode.Joystick1Button3)) { SelectSlot(0);}
        else if(Input.GetKeyDown(KeyCode.Alpha2) || D_Pad.RightPress() && !Input.GetKey(KeyCode.Joystick1Button3)){SelectSlot(1);}
        else if(Input.GetKeyDown(KeyCode.Alpha3) || D_Pad.DownPress() && !Input.GetKey(KeyCode.Joystick1Button3)){SelectSlot(2);}
        else if(Input.GetKeyDown(KeyCode.Alpha4) || D_Pad.LeftPress() && !Input.GetKey(KeyCode.Joystick1Button3)){SelectSlot(3);}
        else if(Input.GetKeyDown(KeyCode.Alpha5) || D_Pad.UpPress() && Input.GetKey(KeyCode.Joystick1Button3)) {SelectSlot(4);}
        else if(Input.GetKeyDown(KeyCode.Alpha6) || D_Pad.RightPress() && Input.GetKey(KeyCode.Joystick1Button3)) {SelectSlot(5);}
        else if(Input.GetKeyDown(KeyCode.Alpha7) || D_Pad.DownPress() && Input.GetKey(KeyCode.Joystick1Button3)) {SelectSlot(6);}
        else if(Input.GetKeyDown(KeyCode.Alpha8) || D_Pad.LeftPress() && Input.GetKey(KeyCode.Joystick1Button3)) {SelectSlot(7);}
    }
    
}
