using HobbitUtilz.InvocableBehaviors;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Initializes TC_UI_Confirmation so it does not need to be active at the start of the scene. 
    /// </summary>
    public class TC_UI_Invoke_InitializeConfirmation : HU_InvocableObject
    {
        [SerializeField] TC_UI_ConfirmationManager _confirmationManager;
        
        public override void Invoke() { _confirmationManager.Initialize(); }
    }
}
