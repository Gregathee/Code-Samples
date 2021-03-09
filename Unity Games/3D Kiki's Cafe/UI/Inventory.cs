using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*TODO
    Advanced inventory manage:
        Split stack
        Take one
        Combine stack    
*/

[System.Serializable]
struct InventoryArray
{
    public InventorySlot[] inventory;
}

public class Inventory : MonoBehaviour, IEscapeKeySubscriber
{
    [SerializeField] SelectableGrid inventory = null;
    //[SerializeField] InventoryArray[] inventory = null;
    [SerializeField] GameObject inventoryObject = null;
    HotBar hotBar = null;
    bool hidden = true;
    int hotBarIndex = 0;
    public InventorySlot heldSlot;
    RecipeBook recipeBook = null;
    bool escapePressed = false;

    void Start()
    {
        GameManager.RegisterEscapeSubscriber(this, 0);
        hotBar = GetComponent<HotBar>();
        recipeBook = GetComponent<RecipeBook>();
        for (int i = 0; i < inventory.GetSelectables().Count; ++i)
        {
             ((InventorySlot)inventory.GetSelectables()[i]).Initialize(this); 
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Tab)) && !recipeBook.IsOpen()) { OpenClose(); }
        if (!hidden)
        {
            //alternative input to close inventory
            if (escapePressed)
            {
                escapePressed = false;
                OpenClose();
            }
        }
    }

    #region Inventory Organization
    /// <summary>
    ///Marks a slot to be held for item swapping, if a slot is marked this will swap held slot with slotToHold
    ///Returns true if slotToHold if slotToHold becomes marked
    /// </summary>
    public bool HoldItem(InventorySlot slotToHold)
    {
        if (slotToHold.GetItem() && !heldSlot)
        {
            heldSlot = slotToHold;
            return true;
        }
        if (slotToHold.GetItem() && heldSlot)
        {
            if (heldSlot.GetItem() == slotToHold.GetItem()) { _holdItemConsolidate(ref slotToHold); }
            else { _holdItemSwap(ref slotToHold); }
        }
        else if (heldSlot) { _holdItemAddToEmptySlot(ref slotToHold); } 
        ClearSelects();
        slotToHold.Select();
        heldSlot = null;
        return false;
    }

    void _holdItemConsolidate(ref InventorySlot slotToHold)
    {
        int amountToAdd = 0;
        if (heldSlot.GetItemCount() <= slotToHold.GetItem().GetStackSize() - slotToHold.GetItemCount())
        {
            amountToAdd = heldSlot.GetItemCount();
        }
        else { amountToAdd = slotToHold.GetItem().GetStackSize() - slotToHold.GetItemCount(); }
        slotToHold.AddToStack(amountToAdd);
        heldSlot.PopItem(amountToAdd);
        ClearSelects();
    }

    void _holdItemSwap(ref InventorySlot slotToHold)
    {
        int tempCount = slotToHold.GetItemCount();
        Item tempItem = slotToHold.PopItem(slotToHold.GetItemCount());
        int heldCount = heldSlot.GetItemCount();
        slotToHold.SetItem(heldSlot.PopItem(heldSlot.GetItemCount()), heldCount);
        heldSlot.SetItem(tempItem, tempCount);
        ClearSelects();
        slotToHold.Select();
        heldSlot = null;
    }

    void _holdItemAddToEmptySlot(ref InventorySlot slotToHold)
    {
        int amountToAdd = heldSlot.GetItemCount();
        slotToHold.SetItem(heldSlot.PopItem(amountToAdd), amountToAdd);
        ClearSelects();
        slotToHold.Select();
        heldSlot = null;
    }
    
    #endregion

    #region AddItemToInventoryInFull
    
    /// <summary>
    /// Adds item stack to inventory only if inventory has room for entire stack
    /// Returns true if adding item was successful
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AddItemToInventoryInFull(Item item, int count = 1)
    {
        if (GetInventoryItemQuantity(item) < count) { return false;}

        bool done = false;
        foreach (IUI_Selectable slot in inventory.GetSelectables())
        {
            if (_addItemToPreexistingSlot((InventorySlot) slot, ref item, ref count))
            {
                done = true;
                break;
            }
        }
        if (!done)
        {
            foreach (IUI_Selectable slot in inventory.GetSelectables())
            {
                //if (_addItemToPreexistingSlot((InventorySlot)slot, ref item, ref count))
                {
                    if (_addItemToEmptySlot((InventorySlot)slot, ref item, ref count)) return true;
                }
            }
        }
        return true;
    }

    bool _addItemToPreexistingSlot(InventorySlot slot, ref Item item, ref int count)
    {
        if (slot.GetItem() == item) { if(_addToFilledSlot(ref slot, ref item, ref count)){ return true; } }
        return false;
    }

    bool _addItemToEmptySlot(InventorySlot slot, ref Item item, ref int count)
    {
        if (slot.GetItem() == null) { if (_addToEmptyStack(ref slot, ref item, ref count)) { return true;} }
        return false;
    }

    bool _addToEmptyStack(ref InventorySlot slot, ref Item item, ref int count)
    {
        if (count > item.GetStackSize())
        {
            slot.SetItem(item, item.GetStackSize());
            count -= item.GetStackSize();
            return false;
        }
        else
        {
            slot.SetItem(item, count);
            return true; 
        }
    }

    bool _addToFilledSlot(ref InventorySlot slot, ref Item item, ref int count)
    {
        if (item.GetStackSize() - slot.GetItemCount() < count)
        {
            int amountToAdd = item.GetStackSize() - slot.GetItemCount();
            if (slot.AddToStack(amountToAdd)) { count -= amountToAdd; }
            return false;
        }
        slot.AddToStack(count);
        return true;
    }
    #endregion

    public KeyValuePair<bool, int> SearchForItem(Item target, int searchCount = 1)
    {
        int found = 0;
        foreach (IUI_Selectable slot in inventory.GetSelectables())
        {
            if (((InventorySlot)slot).GetItem() == target) { found += ((InventorySlot)slot).GetItemCount(); }
        }
        
        return new KeyValuePair<bool, int>(found >= searchCount, found);
    }

    public bool RemoveItemFromInventory(Item target, int amount = 1)
    {
        if (SearchForItem(target, amount).Key)
        {
            foreach (IUI_Selectable slot in inventory.GetSelectables())
            {
                if (_removeItemFromSlot((InventorySlot)slot, ref target, ref amount)) { return true; }
            }
        }
        return false;
    }

    bool _removeItemFromSlot(InventorySlot slot, ref Item target, ref int amount)
    {
        if (slot.GetItem() == target)
        {
            if (amount > slot.GetItemCount())
            {
                amount -= slot.GetItemCount();
                slot.PopItem(slot.GetItemCount());
            }
            else
            {
                slot.PopItem(amount);
                return true;
            }
        }
        return false;
    }
    
    public bool IsOpen() { return !hidden; }

    public void ClearSelects()
    {
        foreach (IUI_Selectable slot in inventory.GetSelectables())
        {
            ((InventorySlot)slot).LetGo();
            ((InventorySlot)slot).Deselect();
        }
    }
    
    void OpenClose()
    {
        hidden = !hidden;
        if (hidden)
        {
            inventory.Disable();
            inventoryObject.transform.localScale = Vector3.zero;
            ClearSelects();
            hotBar.DeselectSlot();
            hotBar.SelectSlot(hotBarIndex);
        }
        else
        {
            inventory.Enable();
            inventoryObject.transform.localScale = Vector3.one;
            hotBarIndex = hotBar.GetSelectedSlotIndex();
            if (heldSlot)
            {
                heldSlot.LetGo();
                heldSlot = null;
            }

            hotBar.DeselectSlot();
            hotBar.SelectSlot(0);
        }
    }
    
    int GetInventoryItemQuantity(Item item)
    {
        int total = 0;
        foreach (IUI_Selectable slot in inventory.GetSelectables())
        {
            if (((InventorySlot)slot).GetItem() == null) { total += item.GetStackSize(); }
            else if (((InventorySlot)slot).GetItem() == item) { total += item.GetStackSize() - ((InventorySlot)slot).GetItemCount();}
        }
        return total;
    }
    public void EscapeKeyPressed()
    {
        escapePressed = true;
    }
}
