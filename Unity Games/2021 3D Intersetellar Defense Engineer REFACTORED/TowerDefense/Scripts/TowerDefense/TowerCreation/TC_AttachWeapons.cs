using System;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerCreation
{
    /// <summary>
    /// Prevents weapons from rotating while being attached
    /// </summary>
    public class TC_AttachWeapons : MonoBehaviour
    {
        void OnEnable()
        {
            TP_Weapon.AttachingWeapon = true;
            Camera.main.orthographic = true;
        }

        void OnDisable()
        {
            TP_Weapon.AttachingWeapon = false;
            if(Camera.main)Camera.main.orthographic = false;
        }
    }
}
