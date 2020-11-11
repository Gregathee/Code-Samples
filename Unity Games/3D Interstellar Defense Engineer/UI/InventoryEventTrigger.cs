using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryEventTrigger : EventTrigger
{
    TowerPartInventorySlot inventorySlot;

    public void SetInventorySlot(TowerPartInventorySlot slot) { inventorySlot = slot; }
    public override void OnPointerDown(PointerEventData data)
    {
        inventorySlot.FollowCursor();
    }
}
