using HobbitUtilz;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerCreation.UI;

namespace TowerDefense.TowerCreation.Factories.Ammo

{
    /// <summary>
    /// Factory class that creates projectile ammo
    /// </summary>
    public class TC_Fac_Ammo_Projectile : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Ammo_Projectile>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_InputField _impactDamageIP;
        [SerializeField] TMP_InputField _damageOverTimeIP;
        [SerializeField] TMP_InputField _damageOverTimeDurationIP;
        [SerializeField] TMP_InputField _damageTicsPerSecondIP;
        [SerializeField] TMP_Dropdown _damageTypeDD;
        [SerializeField] TMP_InputField _penetrationIP;
        [SerializeField] TMP_InputField _speedIP;
        [SerializeField] TMP_InputField _travelTimeIP;
        [SerializeField] TMP_InputField _blastRadiusIP;
        [SerializeField] TMP_InputField _homingIP;
        [SerializeField] TMP_InputField _ordnanceIP;
        [SerializeField] TMP_InputField _clustersIP;
        public TP_Ammo_Projectile CreateTowerPart()
        {
            SanitizeInput();
            TP_Ammo_Projectile projectile = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Ammo_Projectile>();
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

        void SanitizeInput()
        {
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
            HU_Functions.SanitizeIntIP(ref _speedIP, TP_Ammo_Projectile.SPEED_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _travelTimeIP, TP_Ammo_Projectile.TRAVEL_TIME_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _blastRadiusIP, TP_Ammo_Projectile.AOE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _homingIP, TP_Ammo_Projectile.HOMING_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _ordnanceIP, TP_Ammo_Projectile.ORDNANCE_PER_CLUSTER_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _clustersIP, TP_Ammo_Projectile.CLUSTERS_BOUNDS);
        }
    }
}