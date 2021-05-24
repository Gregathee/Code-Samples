using System.Collections;
using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    // this was created to work around a bizarre behavior involving certain Unity behaviors being skipped during initialization.  
    
    /// <summary>
    /// Initializes an inventory on Invoke
    /// </summary>
    public class TC_UI_Invoke_InitializeInventory : HU_InvocableObject
    {
        [SerializeField] TC_UI_TP_Inventory[] _inventories;
        public override void Invoke()
        {
            StartCoroutine(InvokeRoutine());
        }

        IEnumerator InvokeRoutine()
        {
            yield return new WaitForEndOfFrame();
            foreach(TC_UI_TP_Inventory inventory in _inventories)
                inventory.Initialize();
        }
    }
}
