using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using UnityEngine;
using TowerDefense.TowerParts.Ammo;
using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.Factories.Ammo
{
    /// <summary>
    /// Factory class that creates spray ammo.
    /// </summary>
    public class TC_Fac_Ammo_Spray : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Ammo_Spray>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_InputField _impactDamageIP;
        [SerializeField] TMP_InputField _damageOverTimeIP;
        [SerializeField] TMP_InputField _damageOverTimeDurationIP;
        [SerializeField] TMP_InputField _damageTicsPerSecondIP;
        [SerializeField] TMP_Dropdown _damageTypeDD;
        [SerializeField] TMP_InputField _penetrationIP;
        
        public TP_Ammo_Spray CreateTowerPart()
        {
            SanitizeInput();
            TP_Ammo_Spray spray = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Ammo_Spray>();
            spray.Damage = int.Parse(_impactDamageIP.text);
            spray.Dot = int.Parse(_damageOverTimeIP.text);
            spray.DotTime = int.Parse(_damageOverTimeDurationIP.text);
            spray.DotTicsPerSec = int.Parse(_damageTicsPerSecondIP.text);
            spray.DamageType = (WeaknessPriority)_damageTypeDD.value;
            spray.Penetration = int.Parse(_penetrationIP.text);
            return spray;
        }

        void SanitizeInput()
        {
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