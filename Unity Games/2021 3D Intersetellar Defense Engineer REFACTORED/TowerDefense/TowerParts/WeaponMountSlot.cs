using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Used to represent where a weapon can be placed on a Weapon Mount
    /// </summary>
    public class WeaponMountSlot : MonoBehaviour
    {
        [SerializeField] TP_Weapon _weapon;
        public int SlotNumber;
        public void SetSlotNumber(int slot) { SlotNumber = slot; }
        public int GetSlotNumber() { return SlotNumber; }

        public void SetWeapon(TP_Weapon weaponIn)
        {
            _weapon = weaponIn;
            _weapon.transform.SetParent(transform);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.SetIsPreview(false);
        }

        public TP_Weapon GetWeapon() { return _weapon; }

        public void ClearSlot()
        {
            if(_weapon) {Destroy(_weapon.gameObject);}
            _weapon = null;
        }
    }
}