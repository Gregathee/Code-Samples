using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.UI.Inventory
{
    /// <summary>
    /// Enables an inventory slot to be clicked and dragged.
    /// </summary>
    public class InventoryEventTrigger : EventTrigger
    {
        TC_UI_TP_InventorySlot _inventorySlot;

        public void SetInventorySlot(TC_UI_TP_InventorySlot slot) { _inventorySlot = slot; }
        public override void OnPointerDown(PointerEventData data) { _inventorySlot.FollowCursor(); }
    }
}
