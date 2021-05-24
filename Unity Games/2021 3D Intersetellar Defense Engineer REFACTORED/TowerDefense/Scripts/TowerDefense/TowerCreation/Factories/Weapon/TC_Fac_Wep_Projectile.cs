using System;
using HobbitUtilz;
using TMPro;
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
    /// Factory class that creates projectile weapons
    /// </summary>
    public class TC_Fac_Wep_Projectile : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_TComp_AssignmentSlot _ammoSlot;
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] GameObject _sizeDD_Object;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] TMP_Dropdown _recoilDD;
        [SerializeField] TMP_Dropdown _accuracyDD;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canSHootAirTog;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] TMP_InputField _critChanceIP;
        [SerializeField] TMP_InputField _critDamageIP;
        [SerializeField] GameObject _critDamageObject;
        
        void Awake() { TP_Wep_Projectile.Factory = this; }

        void Update() { SanitizeInput(); }

        /// <summary>
        /// Constructs and returns a projectile weapon.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Wep_Projectile projectileWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Projectile>());
            projectileWeapon.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_WEAPON_DIR + partName + ".json";
            projectileWeapon.name = partName;
            projectileWeapon.SetAmmoFilePath(_ammoSlot.GetTowerComponent().CustomPartFilePath);
            projectileWeapon.SetSize((PartSize)_sizeDD.value);
            projectileWeapon.FireRate = float.Parse(_fireRateIP.text);
            projectileWeapon.Recoil = (Recoil)_recoilDD.value;
            projectileWeapon.Accuracy = (Accuracy)_accuracyDD.value;
            projectileWeapon.CanShootDown = _canShootGroundTog.isOn;
            projectileWeapon.CanShootUp = _canSHootAirTog.isOn;
            projectileWeapon.RotationSpeed = int.Parse(_rotationSpeedIP.text);
            projectileWeapon.CritChance = int.Parse(_critChanceIP.text);
            projectileWeapon.CritDamage = int.Parse(_critDamageIP.text);

            TP_Ammo_Projectile ammo = _ammoSlot.GetTowerComponent().GetComponent<TP_Ammo_Projectile>();
            ammo = TowerPart.LoadTowerPartFromFile(ammo.CustomPartFilePath).GetComponent<TP_Ammo_Projectile>();
            
            projectileWeapon.SetAmmo(ammo);
            projectileWeapon.GetAmmo().transform.SetParent(transform);
            projectileWeapon.GetAmmo().transform.localPosition = new Vector3(0, -500, 0);
            projectileWeapon.GetAmmo().gameObject.SetActive(false);
            return projectileWeapon;
        }
        
        /// <summary>
        /// Deconstructs a projectile weapon and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Wep_Projectile projectileWeapon = part.GetComponent<TP_Wep_Projectile>();
            
            _partSelector.JumpToPart(projectileWeapon);
            _sizeDD.value = (int)projectileWeapon.GetSize();
            _fireRateIP.text = projectileWeapon.FireRate.ToString();
            _recoilDD.value = (int)projectileWeapon.Recoil;
            _accuracyDD.value = (int)projectileWeapon.Accuracy;
            _canShootGroundTog.isOn = projectileWeapon.CanShootDown;
            _canSHootAirTog.isOn = projectileWeapon.CanShootUp;
            _rotationSpeedIP.text = projectileWeapon.RotationSpeed.ToString();
            _critChanceIP.text = projectileWeapon.CritChance.ToString();
            _critDamageIP.text = projectileWeapon.CritDamage.ToString();
            TP_Ammo_Projectile ammo = Instantiate(projectileWeapon.GetAmmo());
            ammo.gameObject.SetActive(true);
            _ammoSlot.SetTowerComponent(ammo, true);
            
            // hide size option on edit to prevent size changing from causing weapon overlap.
            _sizeDD_Object.SetActive(!projectileWeapon.FileInUse());
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
            const string message = "You must attach a projectile to the ammo slot.";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
            return true;
        }

        public void ChangeSize() { _partSelector.SetSize((PartSize)_sizeDD.value, true); }

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Projectile.FIRE_RATE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_Wep_Projectile.ROTATION_SPEED_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _critChanceIP, TP_Wep_Projectile.CRIT_CHANCE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _critDamageIP, TP_Wep_Projectile.CRIT_DAMAGE_BOUNDS);
            
            _critDamageObject.SetActive(float.Parse(_critChanceIP.text) > 0);
        }
    }
}
