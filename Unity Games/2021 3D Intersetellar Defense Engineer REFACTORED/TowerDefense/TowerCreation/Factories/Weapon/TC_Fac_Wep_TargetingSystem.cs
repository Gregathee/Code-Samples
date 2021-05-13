using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates advanced targeting systems.
    /// </summary>
    public class TC_Fac_Wep_TargetingSystem : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Wep_TargetingSystem>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] Toggle _basicPriorityTog;
        [SerializeField] Toggle _advancedPriorityTog;
        [SerializeField] TMP_Dropdown _targetingLevelDD;
        [SerializeField] Toggle _detectStealthTog;
        public TP_Wep_TargetingSystem CreateTowerPart()
        {
            TP_Wep_TargetingSystem targetingSystem = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_TargetingSystem>());
            targetingSystem.SetSize((PartSize)_sizeDD.value);
            targetingSystem.MovementTypePriorityTargeting = _basicPriorityTog.isOn;
            targetingSystem.AdvancedPositionPriorityTarget = _advancedPriorityTog.isOn;
            targetingSystem.WeaknessTargetingLevel = (WeaknessTargetingLevel)_targetingLevelDD.value;
            targetingSystem.CanDetectStealth = _detectStealthTog.isOn;
            return targetingSystem;
        }
    }
}
