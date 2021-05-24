using System;
using System.Collections.Generic;
using HobbitUtilz;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;

namespace TowerDefense.TowerCreation.Factories.Ammo

{
    /// <summary>
    /// Factory class that creates projectile ammo
    /// </summary>
    public class TC_Fac_Ammo_Projectile :  TC_Fac_TowerPartFactory
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
        [SerializeField] TMP_InputField _speedIP;
        [SerializeField] TMP_InputField _travelTimeIP;
        [SerializeField] TMP_InputField _blastRadiusIP;
        [SerializeField] TMP_InputField _homingIP;
        [SerializeField] TMP_InputField _ordnanceIP;
        [SerializeField] GameObject _ordanceObject;
        [SerializeField] TMP_InputField _clustersIP;
        void Awake() { TP_Ammo_Projectile.Factory = this; }

        void Update() { SanitizeInput(); }
        
        /// <summary>
        /// Constructs and returns a projectile ammo.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            SanitizeInput();
            TP_Ammo_Projectile projectile = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Ammo_Projectile>();
            projectile.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_AMMO_DIR + partName + ".json";
            projectile.name = partName;
            projectile.Damage = int.Parse(_impactDamageIP.text);
            projectile.Dot = int.Parse(_damageOverTimeIP.text);
            projectile.DotTime = int.Parse(_damageOverTimeDurationIP.text);
            projectile.DotTicsPerSec = int.Parse(_damageTicsPerSecondIP.text);
            projectile.DamageType = (WeaknessPriority)_damageTypeDD.value;
            projectile.Penetration = int.Parse(_penetrationIP.text);
            projectile.Speed = int.Parse(_speedIP.text);
            projectile.TravelTime = int.Parse(_travelTimeIP.text);
            projectile.AOE = int.Parse(_blastRadiusIP.text);
            projectile.Homing = int.Parse(_homingIP.text);
            projectile.OrdnancePerCluster = int.Parse(_ordnanceIP.text);
            projectile.Clusters = int.Parse(_clustersIP.text);
            return projectile;
        }
        
        /// <summary>
        /// Deconstructs a projectile ammo and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Ammo_Projectile projectile = part.GetComponent<TP_Ammo_Projectile>();
            _partSelector.JumpToPart(projectile);
            _impactDamageIP.text = projectile.Damage.ToString();
            _damageOverTimeIP.text = projectile.Dot.ToString();
            _damageOverTimeDurationIP.text = projectile.DotTime.ToString();
            _damageTicsPerSecondIP.text = projectile.DotTicsPerSec.ToString();
            _damageTypeDD.value = (int)projectile.DamageType;
            _penetrationIP.text = projectile.Penetration.ToString();
            _speedIP.text = projectile.Speed.ToString();
            _travelTimeIP.text = projectile.TravelTime.ToString();
            _blastRadiusIP.text = projectile.AOE.ToString();
            _homingIP.text = projectile.Homing.ToString();
            _ordnanceIP.text = projectile.OrdnancePerCluster.ToString();
            _clustersIP.text = projectile.Clusters.ToString();
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
            KeyValuePair<int, int> pair = TP_Ammo_Projectile.DOT_TICS_PER_SECOND_BOUNDS;
            int dotTime = int.Parse(_damageOverTimeDurationIP.text);
            dotTime = pair.Value > dotTime ? dotTime : pair.Value;
            pair = new KeyValuePair<int, int>(pair.Key, dotTime);
            
            HU_Functions.SanitizeIntIP(ref _damageTicsPerSecondIP, pair);
            
            _dotTimeObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            _ticsPerSecObject.SetActive(int.Parse(_damageOverTimeIP.text) > 0);
            _ordanceObject.SetActive(int.Parse(_clustersIP.text) > 0);
            
            HU_Functions.SanitizeIntIP(ref _impactDamageIP, TP_Ammo_Projectile.DAMAGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeIP, TP_Ammo_Projectile.DOT_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _damageOverTimeDurationIP, TP_Ammo_Projectile.DOT_TIME_BOUNDS);
            if (_damageTicsPerSecondIP.text == "" && _damageTicsPerSecondIP.gameObject != EventSystem.current.currentSelectedGameObject || _damageTicsPerSecondIP.text == "-")
                _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "")
                if (int.Parse(_damageTicsPerSecondIP.text) < 0)
                    _damageTicsPerSecondIP.text = "0";
            if (_damageTicsPerSecondIP.text != "" && _damageTicsPerSecondIP.text != "")
            {
                if (int.Parse(_damageTicsPerSecondIP.text) > int.Parse(_damageOverTimeDurationIP.text)) _damageTicsPerSecondIP.text = _damageOverTimeDurationIP.text;
            }
            HU_Functions.SanitizeIntIP(ref _penetrationIP, TP_Ammo_Projectile.PENETRATION_BOUNDS);
            _blastRadiusIP.gameObject.SetActive(int.Parse(_penetrationIP.text) == 1);
            HU_Functions.SanitizeIntIP(ref _speedIP, TP_Ammo_Projectile.SPEED_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _travelTimeIP, TP_Ammo_Projectile.TRAVEL_TIME_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _blastRadiusIP, TP_Ammo_Projectile.AOE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _homingIP, TP_Ammo_Projectile.HOMING_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _ordnanceIP, TP_Ammo_Projectile.ORDNANCE_PER_CLUSTER_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _clustersIP, TP_Ammo_Projectile.CLUSTERS_BOUNDS);
        }
    }
}