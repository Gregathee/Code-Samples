using System;
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
    /// Factory class that creates advanced targeting systems.
    /// </summary>
    public class TC_Fac_Wep_TargetingSystem : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] GameObject _sizeDD_Object;
        [SerializeField] Toggle _basicPriorityTog;
        [SerializeField] GameObject _advancedPriorityObject;
        [SerializeField] Toggle _advancedPriorityTog;
        [SerializeField] GameObject _targetLevelObject;
        [SerializeField] TMP_Dropdown _targetingLevelDD;
        [SerializeField] Toggle _detectStealthTog;

        void Awake() { TP_Wep_TargetingSystem.Factory = this; }

        void Update()
        {
            // hide properties under contradictory situations
            _advancedPriorityObject.SetActive(_basicPriorityTog.isOn);
            _targetLevelObject.SetActive(_advancedPriorityTog.isOn && _advancedPriorityTog.gameObject.activeInHierarchy);
        }
        
        /// <summary>
        /// Constructs and returns a targeting system.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            TP_Wep_TargetingSystem targetingSystem = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_TargetingSystem>());
            targetingSystem.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TARGETING_SYSTEM_DIR + partName + ".json";
            targetingSystem.name = partName;
            targetingSystem.SetSize((PartSize)_sizeDD.value);
            targetingSystem.MovementTypePriorityTargeting = _basicPriorityTog.isOn;
            targetingSystem.AdvancedPositionPriorityTarget = _advancedPriorityTog.isOn;
            targetingSystem.WeaknessTargetingLevel = (WeaknessTargetingLevel)_targetingLevelDD.value;
            targetingSystem.CanDetectStealth = _detectStealthTog.isOn;
            return targetingSystem;
        }
        
        /// <summary>
        /// Deconstructs a targeting system and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Wep_TargetingSystem targetingSystem = part.GetComponent<TP_Wep_TargetingSystem>();
            
            _sizeDD_Object.SetActive(!targetingSystem.FileInUse());
            _partSelector.JumpToPart(targetingSystem);
            _sizeDD.value = (int)targetingSystem.GetSize();
            _basicPriorityTog.isOn = targetingSystem.MovementTypePriorityTargeting;
            _advancedPriorityTog.isOn = targetingSystem.AdvancedPositionPriorityTarget;
            _sizeDD.value = (int)targetingSystem.WeaknessTargetingLevel;
            _detectStealthTog.isOn = targetingSystem.CanDetectStealth;
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
    }
}
