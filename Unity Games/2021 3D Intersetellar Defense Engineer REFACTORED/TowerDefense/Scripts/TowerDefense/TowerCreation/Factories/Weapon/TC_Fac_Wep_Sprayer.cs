using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.Factories.Ammo;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates sprayer weapons.
    /// </summary>
    public class TC_Fac_Wep_Sprayer : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_TComp_AssignmentSlot _ammoSlot;
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] GameObject _sizeDD_Object;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] TMP_InputField _speedIP;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canSHootAirTog;
        [SerializeField] TMP_Dropdown _sprayRangeInput;
        
        void Awake() { TP_Wep_Sprayer.Factory = this; }
        void Update() { SanitizeInput(); }
        
        /// <summary>
        /// Constructs and returns a Sprayer weapon.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Wep_Sprayer sprayerWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Sprayer>());
            sprayerWeapon.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_WEAPON_DIR + partName + ".json";
            sprayerWeapon.name = partName;
            sprayerWeapon.SetAmmoFilePath(_ammoSlot.GetTowerComponent().CustomPartFilePath);
            sprayerWeapon.SetSize((PartSize)_sizeDD.value);
            sprayerWeapon.FireRate = float.Parse(_fireRateIP.text);
            sprayerWeapon.Speed = int.Parse(_speedIP.text);
            sprayerWeapon.CanShootDown = _canShootGroundTog.isOn;
            sprayerWeapon.CanShootUp = _canSHootAirTog.isOn;
            sprayerWeapon.SprayRange = (TurretAngle)_sprayRangeInput.value;
            
            TP_Ammo_Spray ammo = _ammoSlot.GetTowerComponent().GetComponent<TP_Ammo_Spray>();
            ammo = TowerPart.LoadTowerPartFromFile(ammo.CustomPartFilePath).GetComponent<TP_Ammo_Spray>();
            
            sprayerWeapon.SetAmmo(ammo);
            sprayerWeapon.GetAmmo().transform.SetParent(transform);
            sprayerWeapon.GetAmmo().transform.localPosition = new Vector3(0, -500, 0);
            sprayerWeapon.GetAmmo().gameObject.SetActive(false);
            return sprayerWeapon;
        }
        
        /// <summary>
        /// Deconstructs a sprayer weapon and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Wep_Sprayer sprayer = part.GetComponent<TP_Wep_Sprayer>();
            _partSelector.JumpToPart(sprayer);
            _sizeDD.value = (int)sprayer.GetSize();
            _fireRateIP.text = sprayer.FireRate.ToString();
            _speedIP.text = sprayer.Speed.ToString();
            _canShootGroundTog.isOn = sprayer.CanShootDown;
            _canSHootAirTog.isOn = sprayer.CanShootUp;
            _sprayRangeInput.value = (int)sprayer.SprayRange;
            TP_Ammo_Spray ammo = Instantiate(sprayer.GetAmmo());
            ammo.gameObject.SetActive(true);
            _ammoSlot.SetTowerComponent(ammo, true);
            
            // Hide size option on edit to prevent size changing causing weapon overlapping
            _sizeDD_Object.SetActive(!sprayer.FileInUse());
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
        
        public override bool ErrorsPresent()
        {
            if (_ammoSlot.GetTowerComponent()) return false;
            const string message = "You must attach a spray to the ammo slot.";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
            return true;
        }
        
        public void ChangeSize() { _partSelector.SetSize((PartSize)_sizeDD.value, true); }

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Sprayer.FIRE_RATE_BOUNDS);
        }
    }
}
