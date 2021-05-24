using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Redirects menu back to a desired section in the event there is compound creation.
    /// For example, if the ammo creator is accessed through the weapon creator, this will redirect back to weapon creator.
    /// </summary>
    public class TC_UI_ReturnToSectionNavigator : MonoBehaviour
    {
        [SerializeField] HU_InvocableObject _returnFromTowerInvocation;
        [SerializeField] HU_InvocableObject _returnToTowerInvocation;
        [SerializeField] HU_InvocableObject _returnToWeaponInvocation;
        [SerializeField] HU_InvocableObject _defaultReturnInvocation;
        [SerializeField] TC_Fac_TowerPart _factory;

        bool _returnToTower;
        bool _returnToWeapon;
        bool _returnFromTower;
        bool _editingTower;
        bool _editingWeapon;

        public void ReturnToSection(bool promptWarning = false)
        {
            if (!promptWarning) { Return(); return; }
            const string message = "Are you sure you want to return?\n\nYou will lose all unsaved changes.";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, false, Return);
        }

        /// <summary>
        /// When the player selects create a weapon from the tower creation, return back to tower creation.
        /// </summary>
        public void SetReturnToTower()
        {
            _returnToTower = true;
            _editingTower = _factory.IsEditing();
            _factory.SetIsEditing(false);
        }

        /// <summary>
        /// When the player selects create ammo from the tower creation, return back to weapon creation.
        /// </summary>
        public void SetReturnToWeapon()
        {
            _returnToWeapon = true;
            _editingWeapon = _factory.IsEditing();
            _factory.SetIsEditing(false);
        }
        
        /// <summary>
        /// Return tower creation state back to default.
        /// </summary>
        public void SetReturnFromTower() { _returnFromTower = true; }

        void Return()
        {
            if (_returnToWeapon)
            {
                _returnToWeaponInvocation.Invoke();
                _factory.SetIsEditing(_editingWeapon);
                _editingWeapon = false;
                _returnToWeapon = false; 
                return;
            }
            if (_returnToTower)
            {
                _returnToTowerInvocation.Invoke();
                _factory.SetIsEditing(_editingTower);
                _editingTower = false;
                _returnToTower = false; 
                return;
            }

            _factory.SetIsEditing(false);
            
            if (_returnFromTower)
            {
                _returnFromTowerInvocation.Invoke(); 
                _returnFromTower = false;
                return;
            }
            _defaultReturnInvocation.Invoke();
        }
    }
}