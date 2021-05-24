using System;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates laser weapons.
    /// </summary>
    public class TC_Fac_Wep_Laser : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] GameObject _sizeDD_Object;
        [SerializeField] TMP_InputField _damageIP;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canShootAirTog;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] Slider _rSlider;
        [SerializeField] Slider _gSlider;
        [SerializeField] Slider _bSlider;
        [SerializeField] Image _laserColorDisplay;
        
        void Awake() { TP_Wep_Laser.Factory = this; }

        void Update() {SanitizeInput(); }

        /// <summary>
        /// Constructs and returns a laser weapon.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Wep_Laser laserWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Laser>());
            laserWeapon.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.LASER_WEAPON_DIR + partName + ".json";
            laserWeapon.name = partName;
            laserWeapon.SetSize((PartSize)_sizeDD.value);
            laserWeapon.Damage = int.Parse(_damageIP.text);
            laserWeapon.FireRate = float.Parse(_fireRateIP.text);
            laserWeapon.CanShootDown = _canShootGroundTog.isOn;
            laserWeapon.CanShootUp = _canShootAirTog.isOn;
            laserWeapon.RotationSpeed = int.Parse(_rotationSpeedIP.text);
            laserWeapon.LaserColor = new Color(_rSlider.value, _gSlider.value, _bSlider.value, 1);
            return laserWeapon;
        }
        
        /// <summary>
        /// Deconstructs a laser weapon and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Wep_Laser laserWeapon = part.GetComponent<TP_Wep_Laser>();
            _partSelector.JumpToPart(laserWeapon);
            _sizeDD.value = (int)laserWeapon.GetSize();
            _damageIP.text = laserWeapon.Damage.ToString();
            _fireRateIP.text = laserWeapon.FireRate.ToString();
            _canShootAirTog.isOn = laserWeapon.CanShootUp;
            _canShootGroundTog.isOn = laserWeapon.CanShootDown;
            _rotationSpeedIP.text = laserWeapon.RotationSpeed.ToString();
            _rSlider.value = laserWeapon.LaserColor.r;
            _gSlider.value = laserWeapon.LaserColor.g;
            _bSlider.value = laserWeapon.LaserColor.b;
            
            // Hide size option to prevent size changing from causing weapon overlap.
            _sizeDD_Object.SetActive(!laserWeapon.FileInUse());
        }
        
        public override void SetAsActiveFactory()
        {
            base.SetAsActiveFactory();
            _sizeDD_Object.SetActive(true);
        }
        
        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public override ColoredTowerPart GetColoredTowerPart1()
        {
            return _partSelector.GetCurrentPart().GetComponent<ColoredTowerPart>();
        }
        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public override ColoredTowerPart GetColoredTowerPart2() { return null; }
        
        public void ChangeSize() { _partSelector.SetSize((PartSize)_sizeDD.value, true); }

        // Updates the color sample representing the color of the laser.
        public void UpdateColorSample()
        {
            _laserColorDisplay.color = new Color(_rSlider.value, _gSlider.value, _bSlider.value, 1);
        }

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _damageIP, TP_Wep_Laser.DAMAGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Laser.FIRE_RATE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_Wep_Laser.ROTATION_SPEED_BOUNDS);
        }
    }
}
