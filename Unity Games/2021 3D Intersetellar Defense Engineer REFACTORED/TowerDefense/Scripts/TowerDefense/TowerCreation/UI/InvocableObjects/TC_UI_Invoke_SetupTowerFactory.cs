using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Initializes TC_Fac_Tower on invocation.
    /// </summary>
    public class TC_UI_Invoke_SetupTowerFactory : HU_InvocableObject
    {
        [SerializeField] TC_Fac_Tower _factory;
        public override void Invoke() { _factory.SetupModels(); }
    }
}
