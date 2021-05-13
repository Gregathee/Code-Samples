using TowerDefense.TowerParts;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// An invisible sphere collider that follows the placement of weapons in the weapon creator. When the SlotDetector collides with a weapon mount slot, it snaps the weapon in place.
    /// </summary>
    public class TC_UI_SlotDetector : MonoBehaviour
    {
        public bool IsTouchingSlot;
        WeaponMountSlot _slot;
        void OnTriggerEnter(Collider other)
        {
            WeaponMountSlot slot = other.GetComponent<WeaponMountSlot>();
            if (!slot) return;
            IsTouchingSlot = true;
            _slot = slot;
        }

        void OnTriggerExit(Collider other)
        {
            WeaponMountSlot slot = other.GetComponent<WeaponMountSlot>();
            if (!slot) return;
            IsTouchingSlot = false;
            this._slot = null;
        }

        public WeaponMountSlot GetSlot() { return _slot; }
    }
}