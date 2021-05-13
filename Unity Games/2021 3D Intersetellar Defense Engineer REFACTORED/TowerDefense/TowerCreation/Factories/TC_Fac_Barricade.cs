using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates Barricades
    /// </summary>
    public class TC_Fac_Barricade : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Barricade>
    {
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_InputField _constructionTimeIP;
        [SerializeField] TMP_InputField _durabilityIP;
        public TP_Barricade CreateTowerPart()
        {
            HU_Functions.SanitizeIntIP(ref _constructionTimeIP, TP_Barricade.CONSTRUCTION_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _durabilityIP, TP_Barricade.DURABILITY_BOUNDS);
            
            TP_Barricade barricade = Instantiate(_partSelector.GetCurrentPart()).GetComponent<TP_Barricade>();
            barricade.Durability = int.Parse(_durabilityIP.text);
            barricade.ConstructionTime = float.Parse(_constructionTimeIP.text);
            
            return barricade;
        }
    }
}
