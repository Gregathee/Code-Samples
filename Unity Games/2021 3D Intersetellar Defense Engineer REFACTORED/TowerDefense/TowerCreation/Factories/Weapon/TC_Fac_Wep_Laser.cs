using System;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates laser weapons.
    /// </summary>
    public class TC_Fac_Wep_Laser : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Wep_Laser>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _damageIP;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canShootAir;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] Slider _rSlider;
        [SerializeField] Slider _gSlider;
        [SerializeField] Slider _bSlider;
        [SerializeField] Image _laserColorDisplay;

        void Update() { _laserColorDisplay.color = new Color(_rSlider.value, _gSlider.value, _bSlider.value, 1); }

        public TP_Wep_Laser CreateTowerPart()
        {
            SanitizeInput();
            TP_Wep_Laser laserWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Laser>());
            laserWeapon.SetSize((PartSize)_sizeDD.value);
            laserWeapon.Damage = int.Parse(_damageIP.text);
            laserWeapon.FireRate = float.Parse(_fireRateIP.text);
            laserWeapon.CanShootDown = _canShootGroundTog.isOn;
            laserWeapon.CanShootUp = _canShootAir.isOn;
            laserWeapon.RotationSpeed = int.Parse(_rotationSpeedIP.text);
            laserWeapon.LaserColor = new Color(_rSlider.value, _gSlider.value, _bSlider.value, 1);
            return laserWeapon;
        }

        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _damageIP, TP_Wep_Laser.DAMAGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Laser.FIRE_RATE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_Wep_Laser.ROTATION_SPEED_BOUNDS);
        }
    }
}
