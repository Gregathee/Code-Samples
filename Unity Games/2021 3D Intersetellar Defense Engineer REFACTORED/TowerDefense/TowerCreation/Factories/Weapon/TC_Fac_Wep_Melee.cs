using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates melee weapons.
    /// </summary>
    public class TC_Fac_Wep_Melee : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Wep_Melee>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _damageIP;
        [SerializeField] TMP_Dropdown _damageTypeDD;
        [SerializeField] TMP_InputField _damageOverTimeIP;
        [SerializeField] TMP_InputField _damageOverTimeDurationIP;
        [SerializeField] TMP_InputField _damageTicsPerSecondIP;
        
        public TP_Wep_Melee CreateTowerPart()
        {
            SanatizeInput();
            TP_Wep_Melee meleeWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Melee>());
            meleeWeapon.SetSize((PartSize)_sizeDD.value);
            meleeWeapon.Damage = int.Parse(_damageIP.text);
            meleeWeapon.DamageType = (WeaknessPriority)_damageTypeDD.value;
            return meleeWeapon;
        }

        void SanatizeInput()
        {
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
