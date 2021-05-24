using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Clears TC_Fac_Tower models.
    /// </summary>
    public class TC_UI_Invoke_ClearModels : HU_InvocableObject
    {
        [SerializeField] TC_Fac_Tower _factory;
        public override void Invoke() { _factory.ClearModels(); }
    }
}
