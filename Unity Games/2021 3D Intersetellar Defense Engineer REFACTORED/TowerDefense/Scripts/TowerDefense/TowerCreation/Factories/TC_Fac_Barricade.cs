using System;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates Barricades
    /// </summary>
    public class TC_Fac_Barricade : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_InputField _constructionTimeIP;
        [SerializeField] TMP_InputField _durabilityIP;
        
        void Awake() { TP_Barricade.Factory = this; }

        void Update()
        {
            
            HU_Functions.SanitizeIntIP(ref _constructionTimeIP, TP_Barricade.CONSTRUCTION_TIME_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _durabilityIP, TP_Barricade.DURABILITY_BOUNDS);
        }

        public override TowerComponent CreateTowerPart(string partName)
        {
            HU_Functions.SanitizeIntIP(ref _constructionTimeIP, TP_Barricade.CONSTRUCTION_TIME_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _durabilityIP, TP_Barricade.DURABILITY_BOUNDS);
            
            TP_Barricade barricade = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Barricade>();
            barricade.CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.BARRICADE_DIR + partName + ".json";
            barricade.name = partName;
            barricade.Durability = int.Parse(_durabilityIP.text);
            barricade.ConstructionTime = float.Parse(_constructionTimeIP.text);
            
            return barricade;
        }
        public override void DisplayPartProperties(TowerComponent part)
        {
            TP_Barricade barricade = part.GetComponent<TP_Barricade>();
            _partSelector.JumpToPart(barricade);
            _durabilityIP.text = barricade.Durability.ToString();
            _constructionTimeIP.text = barricade.ConstructionTime.ToString();
        }
        
        public override ColoredTowerPart GetColoredTowerPart1()
        {
            return _partSelector.GetCurrentPart().GetComponent<ColoredTowerPart>();
        }
        public override ColoredTowerPart GetColoredTowerPart2() { return null; }
    }
}
