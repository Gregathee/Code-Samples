using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates sprayer weapons.
    /// </summary>
    public class TC_Fac_Wep_Sprayer : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Wep_Sprayer>
    {
        [SerializeField] TC_Fac_Wep_AmmoSlot _ammoSlot;
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canSHootAirTog;
        [SerializeField] TMP_Dropdown _sprayRangeInput;
        
        public TP_Wep_Sprayer CreateTowerPart()
        {
            SanitizeInput();
            TP_Wep_Sprayer sprayerWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Sprayer>());
            sprayerWeapon.SetAmmoFilePath(_ammoSlot.GetAmmo().CustomPartFilePath);
            sprayerWeapon.SetSize((PartSize)_sizeDD.value);
            sprayerWeapon.FireRate = float.Parse(_fireRateIP.text);
            sprayerWeapon.CanShootDown = _canShootGroundTog.isOn;
            sprayerWeapon.CanShootUp = _canSHootAirTog.isOn;
            sprayerWeapon.SprayRange = (TurretAngle)_sprayRangeInput.value;
            return sprayerWeapon;
        }

        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Sprayer.FIRE_RATE_BOUNDS);
        }
    }
}
