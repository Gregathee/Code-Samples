using System;
using System.Collections.Generic;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using UnityEngine;
using TowerDefense.TowerParts.Ammo;
using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.Factories.Ammo
{
    /// <summary>
    /// Factory class that creates spray ammo.
    /// </summary>
    public class TC_Fac_Ammo_Spray :  TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_InputField _impactDamageIP;
        [SerializeField] TMP_InputField _damageOverTimeIP;
        [SerializeField] TMP_InputField _damageOverTimeDurationIP;
        [SerializeField] GameObject _dotTimeObject;
        [SerializeField] TMP_InputField _damageTicsPerSecondIP;
        [SerializeField] GameObject _ticsPerSecObject;
        [SerializeField] TMP_Dropdown _damageTypeDD;
        [SerializeField] TMP_InputField _penetrationIP;
        
        void Awake() { TP_Ammo_Spray.Factory = this; }

        void Update() { SanitizeInput(); }

        
        /// <summary>
        /// Constructs and returns a Spray ammo.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Ammo_Spray spray = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Ammo_Spray>();
            spray.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_AMMO_DIR + partName + ".json";
            spray.name = partName;
            spray.Damage = int.Parse(_impactDamageIP.text);
            spray.Dot = int.Parse(_damageOverTimeIP.text);
            spray.DotTime = int.Parse(_damageOverTimeDurationIP.text);
            spray.DotTicsPerSec = int.Parse(_damageTicsPerSecondIP.text);
            spray.DamageType = (WeaknessPriority)_damageTypeDD.value;
            spray.Penetration = int.Parse(_penetrationIP.text);
            return spray;
        }
        
        /// <summary>
        /// Deconstructs a spray ammo and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Ammo_Spray sprayAmmo = part.GetComponent<TP_Ammo_Spray>();
            _partSelector.JumpToPart(sprayAmmo);
            _impactDamageIP.text = sprayAmmo.Damage.ToString();
            _damageOverTimeIP.text = sprayAmmo.Dot.ToString();
            _damageOverTimeDurationIP.text = sprayAmmo.DotTime.ToString();
            _damageTicsPerSecondIP.text = sprayAmmo.DotTicsPerSec.ToString();
            _damageTypeDD.value = (int)sprayAmmo.DamageType;
            _penetrationIP.text = sprayAmmo.Penetration.ToString();
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

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            KeyValuePair<int, int> pair = TP_Ammo_Spray.DOT_TICS_PER_SECOND_BOUNDS;
            int dotTime = int.Parse(_damageOverTimeDurationIP.text);
            dotTime = pair.Value > dotTime ? dotTime : pair.Value;
            pair = new KeyValuePair<int, int>(pair.Key, dotTime);
            
            HU_Functions.SanitizeIntIP(ref _damageTicsPerSecondIP, pair);
            
            _dotTimeObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            _ticsPerSecObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            
            HU_Functions.SanitizeIntIP(ref _impactDamageIP, TP_Ammo_Spray.DAMAGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeIP, TP_Ammo_Spray.DOT_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeDurationIP, TP_Ammo_Spray.DOT_TIME_BOUNDS);
            if (_damageTicsPerSecondIP.text == "" && _damageTicsPerSecondIP.gameObject != EventSystem.current.currentSelectedGameObject || _damageTicsPerSecondIP.text == "-")
                _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "")
                if (int.Parse(_damageTicsPerSecondIP.text) < 0)
                    _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "" && _damageTicsPerSecondIP.text != "")
            {
                if (int.Parse(_damageTicsPerSecondIP.text) > int.Parse(_damageOverTimeDurationIP.text)) _damageTicsPerSecondIP.text = _damageOverTimeDurationIP.text;
            }
            HU_Functions.SanitizeIntIP(ref _penetrationIP, TP_Ammo_Spray.PENETRATION_BOUNDS);
        }
    }
}