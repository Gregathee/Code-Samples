using System.Collections.Generic;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates melee weapons.
    /// </summary>
    public class TC_Fac_Wep_Melee : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] GameObject _sizeDD_Object;
        [SerializeField] TMP_InputField _damageIP;
        [SerializeField] TMP_Dropdown _damageTypeDD;
        [SerializeField] TMP_InputField _damageOverTimeIP;
        [SerializeField] TMP_InputField _damageOverTimeDurationIP;
        [SerializeField] GameObject _dotTimeObject;
        [SerializeField] TMP_InputField _damageTicsPerSecondIP;
        [SerializeField] GameObject _ticsPerSecObject;
        
        void Awake() { TP_Wep_Melee.Factory = this; }
        
        void Update() { SanitizeInput(); }
        
        /// <summary>
        /// Constructs and returns a melee weapon.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Wep_Melee meleeWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Melee>());
            meleeWeapon.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.MELEE_WEAPON_DIR + partName + ".json";
            meleeWeapon.name = partName;
            meleeWeapon.SetSize((PartSize)_sizeDD.value);
            meleeWeapon.Damage = int.Parse(_damageIP.text);
            meleeWeapon.DamageType = (WeaknessPriority)_damageTypeDD.value;
            return meleeWeapon;
        }
        
        /// <summary>
        /// Deconstructs a melee weapon and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Wep_Melee meleeWeapon = part.GetComponent<TP_Wep_Melee>();
            _partSelector.JumpToPart(meleeWeapon);
            _sizeDD.value = (int)meleeWeapon.GetSize();
            _damageIP.text = meleeWeapon.Damage.ToString();
            _damageTypeDD.value = (int)meleeWeapon.DamageType;
            
            // Hide size option to prevent size changing from causing weapon overlap.
            _sizeDD_Object.SetActive(!meleeWeapon.FileInUse());
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

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            KeyValuePair<int, int> pair = TP_Wep_Melee.DOT_TICS_PER_SECOND_BOUNDS;
            int dotTime = int.Parse(_damageOverTimeDurationIP.text);
            dotTime = pair.Value > dotTime ? dotTime : pair.Value;
            pair = new KeyValuePair<int, int>(pair.Key, dotTime);
            
            HU_Functions.SanitizeIntIP(ref _damageTicsPerSecondIP, pair);
            
            _dotTimeObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            _ticsPerSecObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            
            HU_Functions.SanitizeIntIP(ref _damageIP, TP_Wep_Melee.DAMAGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeIP, TP_Wep_Melee.DOT_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeDurationIP, TP_Wep_Melee.DOT_TIME_BOUNDS);
            if (_damageTicsPerSecondIP.text == "" && _damageTicsPerSecondIP.gameObject != EventSystem.current.currentSelectedGameObject || _damageTicsPerSecondIP.text == "-")
                _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "")
                if (int.Parse(_damageTicsPerSecondIP.text) < 0)
                    _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "" && _damageTicsPerSecondIP.text != "")
            {
                if (int.Parse(_damageTicsPerSecondIP.text) > int.Parse(_damageOverTimeDurationIP.text)) _damageTicsPerSecondIP.text = _damageOverTimeDurationIP.text;
            }
        }
    }
}
