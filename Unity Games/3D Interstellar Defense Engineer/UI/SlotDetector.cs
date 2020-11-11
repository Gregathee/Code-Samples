using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDetector : MonoBehaviour
{

    public bool isTouchingSlot;
    WeaponMountSlot slot;
    private void OnTriggerEnter(Collider other)
    {
        WeaponMountSlot slot = other.GetComponent<WeaponMountSlot>();
        if (slot)
        {
            isTouchingSlot = true;
            this.slot = slot;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WeaponMountSlot slot = other.GetComponent<WeaponMountSlot>();
        if (slot)
        {
            isTouchingSlot = false;
            this.slot = null;
        }
    }

    public bool IsTouchingSlot() { return isTouchingSlot; }

    public WeaponMountSlot GetSlot() { return slot; }
}
