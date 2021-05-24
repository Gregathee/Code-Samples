using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Clears an assignment slot on invocation.
    /// </summary>
    public class TC_UI_Invoke_ClearAssignSlot : HU_InvocableObject
    {
        [SerializeField] TC_TComp_AssignmentSlot _slot;
        public override void Invoke() { _slot.ClearSlot(); }
    }
}
