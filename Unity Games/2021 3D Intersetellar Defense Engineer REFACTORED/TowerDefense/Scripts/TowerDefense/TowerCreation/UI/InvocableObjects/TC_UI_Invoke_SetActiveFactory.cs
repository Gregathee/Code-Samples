using System;
using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    [System.Serializable]
    struct InventoryActiveObjectPair
    {
        public GameObject GameObject;
        public TC_Fac_TowerPartFactory Factory;
    }
    
    /// <summary>
    /// Sets active factory based on what object is active in hierarchy 
    /// </summary>
    public class TC_UI_Invoke_SetActiveFactory : HU_InvocableObject
    {
        [SerializeField] InventoryActiveObjectPair[] _inventoryActiveObjectPairs;
        public override void Invoke()
        {
            foreach (InventoryActiveObjectPair pair in _inventoryActiveObjectPairs)
            {
                if (!pair.GameObject.activeInHierarchy) continue;
                pair.Factory.SetAsActiveFactory();
                return;
            }
        }
    }
}
