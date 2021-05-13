using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts.Ammo;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// A UI slot that holds and displays a reference to ammo during weapon creation.
    /// </summary>
    public class TC_Fac_Wep_AmmoSlot : MonoBehaviour
    {
        TP_Ammo _ammo;
        [SerializeField] RawImage _rawImage;

        bool _mouseOver;

        void Start() { _rawImage.color = new Color(0, 0, 0, 0); }

        void Update()
        {
            if (!_mouseOver) return;
            if (_ammo && Input.GetMouseButtonDown(1)) { _ammo = null; return; }
            if (!Input.GetMouseButtonUp(0)) return;
            // When ammo is being dragged, the ammo's inventory slot is SlotFollowingCursor.
            _ammo = TC_UI_TP_InventorySlot.SlotFollowingCursor.GetTowerComponent().GetComponent<TP_Ammo>();
            if (_ammo) { _rawImage.texture = _ammo.GetView().targetTexture; }
        }

        public void SetAmmo(TP_Ammo ammo)
        {
            _ammo = ammo;
            _rawImage.color = Color.white;
        }

        public TP_Ammo GetAmmo() { return _ammo;}

        // Mouse Enter is called from UI events attached to this object.
        public void MouseEnter() { _mouseOver = true; }
        public void MouseExit() { _mouseOver = false; }

        public void ClearSlot() { _ammo = null; _rawImage.color = new Color(0, 0, 0, 0); }
    }
}
