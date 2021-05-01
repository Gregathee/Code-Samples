using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMountSlot : TowerPart
{
    [SerializeField] Weapon weapon = null;
    public int slotNumber = 0;


    public void SetSlotNumber(int slot) { slotNumber = slot; }
    public int GetSlotNubmer() { return slotNumber; }

    public void SetWeapon(Weapon weaponIn) { weapon = weaponIn; }

    public Weapon GetWeapon() { return weapon; }
}
