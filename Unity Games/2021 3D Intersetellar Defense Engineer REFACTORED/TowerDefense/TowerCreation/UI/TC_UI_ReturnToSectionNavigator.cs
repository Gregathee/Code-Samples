using HobbitUtilz.InvocableBehaviors;
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

        bool _returnToTower;
        bool _returnToWeapon;
        bool _returnFromTower;

        public void ReturnToSection()
        {
            if (_returnToWeapon) { _returnToWeaponInvocation.Invoke(); _returnToWeapon = false; return; }
            if(_returnToTower) {_returnToTowerInvocation.Invoke(); _returnToTower = false; return; }
            if(_returnFromTower) {_returnFromTowerInvocation.Invoke(); _returnFromTower = false; return; }
            _defaultReturnInvocation.Invoke();
        }

        /// <summary>
        /// When the player selects create a weapon from the tower creation, return back to tower creation.
        /// </summary>
        public void SetReturnToTower() { _returnToTower = true; }
        
        /// <summary>
        /// When the player selects create ammo from the tower creation, return back to weapon creation.
        /// </summary>
        public void SetReturnToWeapon() { _returnToWeapon = true;}
        
        /// <summary>
        /// Return tower creation state back to default.
        /// </summary>
        public void SetReturnFromTower() { _returnFromTower = true; }
    }
}