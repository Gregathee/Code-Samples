using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlot : UI_Slot
{
    [SerializeField] TMP_Text stackText = null;
    [SerializeField] protected Color selectedColor = new Color();
    [SerializeField] Item item = null;
    Inventory inventory;
    int itemCount = 0;
    bool held = false;
    
    public void Initialize(Inventory owner)
    {
        inventory = owner;
        stackText.text = "";
        if (item)
        {
            elementImage.sprite = item.GetSprite();
            itemCount = 1;
        }
        else {elementImage.gameObject.SetActive(false);}
    }
    /// <summary>
    /// Unmark slot to not be held for potential swapping
    /// </summary>
    public void LetGo()
    {
        held = false;
        selectionIndicator.color = unselectedColor;
    }
    
    /// <summary>
    /// Mark slot to be held for potential swapping
    /// </summary>
    public override void Invoke()
    {
        if (inventory)
        {
            if (inventory.HoldItem(this))
            {
                inventory.ClearSelects();
                selectionIndicator.color = selectedColor;
                held = true;
            }
        }
    }
    public override string GetDisplayName()
    {
        if (item) { return item.name; }
        return "";
    }
    /// <summary>
    /// Move selection cursor to this slot
    /// </summary>
    public override void Select() { if(!held)selectionIndicator.color = hoverColor; }

    /// <summary>
    /// Remove selection cursor from this slot
    /// </summary>
    public override void Deselect() { if(!held)selectionIndicator.color = unselectedColor; }
    
    /// <summary>
    /// Used to increase the stack count of this slot if it already contains an item
    /// Returns true if slot can fit items being added or if slot is empty
    /// To add to empty slot, use SetItem()
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AddToStack(int count = 1)
    {
        if (item)
        {
            if (itemCount + count <= item.GetStackSize())
            {
                itemCount += count;
                stackText.text = itemCount.ToString();
                if (item.GetComponent<ItemTool>()) { stackText.text = "";}
                return true;
            }
        }
        return false;
    }
    
    public int GetItemCount() { return itemCount;}
    
    /// <summary>
    /// Used to add item stack to empty slot
    /// Returns true if item was set succesfully
    /// To add to not empty slot, use AddToStack()
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="count"></param>
    public bool SetItem(Item newItem, int count = 1)
    {
        if (newItem)
        {
            if (item)
            {
                Debug.Log("[InventorySlot] Attempted to set item of a non empty slot");
                return false;
            }
            elementImage.gameObject.SetActive(true);
            item = newItem;
            itemCount = count;
            stackText.text = count.ToString();
            if (item.GetComponent<ItemTool>()) { stackText.text = "";}
            elementImage.sprite = item.GetSprite();
        }
        else
        {
            ClearElement();
            Debug.Log("Set null item");
        }

        return true;
    }
    
    /// <summary>
    /// Returns item and decrements count from item count
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public Item PopItem(int count = 1)
    {
        Item tempItem = null;
        if (count > itemCount) { return null;}
        if (itemCount - count >= 0)
        {
            tempItem = item;
            itemCount -= count;
        }

        if (itemCount > 0)
        {
            stackText.text = itemCount.ToString();
            if (item.GetComponent<ItemTool>()) { stackText.text = "";}
        }
        else { ClearElement(); }
        return tempItem;
    }

    public Item GetItem() { return item; }
    
    public override void ClearElement()
    {
        item = null;
        elementImage.sprite = defaultImage;
        elementImage.gameObject.SetActive(false);
        stackText.text = "";
    }
}
