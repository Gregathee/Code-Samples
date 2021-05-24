using System.Collections;
using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Activates a TC_UI_TP_Inventory to enable only relevant tower components to the inventory for performance.
    /// </summary>
    public class TC_UI_Invoke_ActivateInventory : HU_InvocableObject
    {
        [SerializeField] TC_UI_TP_Inventory _inventory;
        public override void Invoke()
        {
            if(_invokeOnStart) StartCoroutine(ActivateInventory());
            else { _inventory.ActivateInventory();}
        }

        IEnumerator ActivateInventory()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            _inventory.ActivateInventory();
        }
    }
}
