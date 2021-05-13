using HobbitUtilz.InvocableBehaviors;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Invocable Objects that reset a given TC_UI_SectionNavigator to a given index.
    /// </summary>
    public class TC_UI_Invoke_SectionNavReset : HU_InvocableObject
    {
        [SerializeField] TC_UI_SectionNavigator _partSelector;
        [SerializeField] int index;
        public override void Invoke() { _partSelector.Reset(index); }
    }
}
