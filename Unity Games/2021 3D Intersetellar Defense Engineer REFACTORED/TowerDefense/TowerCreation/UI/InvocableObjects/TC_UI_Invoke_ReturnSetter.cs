using HobbitUtilz.InvocableBehaviors;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI.InvocableObjects
{
    /// <summary>
    /// Sets a TC_UI_ReturnToSectionNavigator returnTo value to true;
    /// </summary>
    public class TC_UI_Invoke_ReturnSetter : HU_InvocableObject
    {
        [SerializeField] TC_UI_ReturnToSectionNavigator _navigator;
        [SerializeField] ReturnToValue _returnToValue;
        
        
        
        enum ReturnToValue { FromTower, ToTower, ToWeapon }
        public override void Invoke()
        {
            switch (_returnToValue)
            {
                case ReturnToValue.FromTower: _navigator.SetReturnFromTower(); break;
                case ReturnToValue.ToTower: _navigator.SetReturnToTower(); break;
                case ReturnToValue.ToWeapon: _navigator.SetReturnToWeapon(); break;
            }
        }
    }
}
