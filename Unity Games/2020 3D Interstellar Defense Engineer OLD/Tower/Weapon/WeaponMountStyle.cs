using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMountStyle : TowerPart
{
    [SerializeField] WeaponMountSlot[] slots = null;

    private void Start()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlotNumber(i);
        }
    }

    public WeaponMountSlot[] GetSlots() { return slots; }

    public void ClearSlots()
    {
        foreach (WeaponMountSlot slot in slots)
        {
            if(slot.GetWeapon())Destroy(slot.GetWeapon().gameObject);
            slot.SetWeapon(null);
        }
    }
}
