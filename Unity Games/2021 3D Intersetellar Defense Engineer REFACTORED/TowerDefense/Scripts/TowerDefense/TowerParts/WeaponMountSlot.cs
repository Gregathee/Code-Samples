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
        public Quaternion SavedLocalRotation;
        public void SetSlotNumber(int slot) { SlotNumber = slot; }
        public int GetSlotNumber() { return SlotNumber; }

        public void SetWeapon(TP_Weapon weaponIn, bool lookAtMouse = false)
        {
            if(_weapon) {Destroy(_weapon.gameObject);}
            _weapon = weaponIn;
            _weapon.transform.SetParent(transform);
            _weapon.transform.localPosition = Vector3.zero;
            _weapon.CorrectRotation();
            _weapon.transform.localRotation = SavedLocalRotation;
            _weapon.SetIsPreview(false);
            _weapon.CompensateScale();
            if(_weapon.GetView()) {_weapon.GetView().gameObject.SetActive(false);}
            if(lookAtMouse){_weapon.LookAtMouse();}
        }

        public TP_Weapon GetWeapon() { return _weapon; }

        public void ClearSlot()
        {
            if(_weapon) {Destroy(_weapon.gameObject);}
            _weapon = null;
        }
    }
}