using HobbitUtilz.InvocableBehaviors;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Activates an array of tower part selectors
    /// </summary>
    public class TC_UI_Invoke_SelectorActivate : HU_InvocableObject
    {
        [SerializeField] TC_UI_TowerPartSelector[] _activatedTowerPartSelectors;
        [SerializeField] TC_UI_TowerPartSelector[] _deactivatedTowerPartSelectors;
        public override void Invoke()
        {
            foreach (TC_UI_TowerPartSelector selector in _activatedTowerPartSelectors) { selector.UnHide(false); }
            foreach (TC_UI_TowerPartSelector selector in _deactivatedTowerPartSelectors) { selector.Hide(false); } 
        }
    }
}
