using UnityEngine;
namespace TowerDefense.TowerCreation.UI.Inventory
{
    /// <summary>
    /// Remotely changes the mode of all inventory slots to spawn an image on instantiate a part on drag. Used to attach weapons to weapon mount.
    /// </summary>
    public class TC_UI_TP_Inventory_ChangeSlotToSpawnImage : MonoBehaviour
    {
        void OnEnable() { TC_UI_TP_InventorySlot.DragImage = false; }

        void OnDisable() { TC_UI_TP_InventorySlot.DragImage = true; }
    }
}
