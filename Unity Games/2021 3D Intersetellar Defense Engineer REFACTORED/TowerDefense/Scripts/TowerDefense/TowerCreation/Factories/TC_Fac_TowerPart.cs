using System;
using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerCreation.Factories.Weapon;
using TowerDefense.TowerCreation.Factories.Ammo;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates tower parts
    /// </summary>
    public class TC_Fac_TowerPart : MonoBehaviour
    {
        public static TC_Fac_TowerPartFactory ActiveFactory;
        static TowerComponent _oldPartBeingEdited;
        bool _isEditing;

        [SerializeField] TC_UI_SectionNavigator _sectionNavigator;
        [SerializeField] HU_InvocableObject _sprayAmmoSceneSetup;
        [SerializeField] HU_InvocableObject _sprayerWeaponSceneSetup;
        [SerializeField] HU_InvocableObject _meleeWeaponSceneSetup;
        [SerializeField] HU_InvocableObject _laserWeaponSceneSetup;
        [SerializeField] HU_InvocableObject _targetingSystemSceneSetup;
        [SerializeField] GameObject[] _objectsToDeactivateOnEdit;
        [SerializeField] TC_ModelManager _modelManager;
        [SerializeField] TC_Fac_Ammo_Projectile _ammoProjectileFactory;
        [SerializeField] TC_Fac_Ammo_Spray _sprayAmmoFactory;
        [SerializeField] TC_Fac_Wep_Projectile _projectileWeaponFactory;
        [SerializeField] TC_Fac_Wep_Sprayer _sprayerWeaponFactory;
        [SerializeField] TC_Fac_Wep_Melee _meleeWeaponFactory;
        [SerializeField] TC_Fac_Wep_Laser _laserWeaponFactory;
        [SerializeField] TC_Fac_Wep_TargetingSystem _targetingWeaponFactory;
        [SerializeField] TC_Fac_Tower _towerFactory;
        [SerializeField] TC_Fac_Barricade _barricadeFactory;
        [SerializeField] TC_UI_ReturnToSectionNavigator _returnNavigator;

        public void Confirm()
        {
            if (ActiveFactory.ErrorsPresent()) { return;}
            const string message = "Are you sure you want to save this blueprint?";
            string input = _isEditing ? _oldPartBeingEdited.name : "";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, true, CreatePart, input);
        }

        public void CreatePart()
        {
            if(!_isEditing)if (_modelManager.NameInUse(TC_UI_ConfirmationManager.Instance.PartName()))
            {
                const string errorMessage = "This name is already in use";
                TC_UI_ConfirmationManager.Instance.PromptMessage(errorMessage, false, false);
                return;
            }
            CreateTowerPart();
        }

        public void EditTowerPart()
        {
            _oldPartBeingEdited = TC_UI_TP_InventorySlot.SlotFollowingCursor?.GetTowerComponent();
            if (!_oldPartBeingEdited)
            {
                const string message = "You must select something to edit.";
                TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
                return;
            }
            SetIsEditing(true);
            _sectionNavigator.SetScene();
            SetEditScene();
            _oldPartBeingEdited.GetComponent<ISerializableTowerComponent>().GetFactory().DisplayPartProperties(_oldPartBeingEdited);
        }

        public bool IsEditing() { return _isEditing;}

        public void SetIsEditing(bool edit)
        {
            _isEditing = edit;
            foreach (GameObject gameOb in _objectsToDeactivateOnEdit)
            {
                gameOb.SetActive(!edit);
            }
        }

        public void DeleteTowerPart()
        {
            TC_UI_TP_InventorySlot.SlotFollowingCursor?.GetTowerComponent()?.GetComponent<ISerializableTowerComponent>()?.DeleteFile(false);
        }

        public void DuplicateTowerPart()
        {
            if (!TC_UI_TP_InventorySlot.SlotFollowingCursor?.GetTowerComponent())
            {
                const string errorMessage = "You must select something to duplicate.";
                TC_UI_ConfirmationManager.Instance.PromptMessage(errorMessage, false, false);
                return;
            }

            _oldPartBeingEdited = TC_UI_TP_InventorySlot.SlotFollowingCursor.GetTowerComponent();
            
            const string message = "Are you sure you want to duplicate this blueprint?";
            string input = _oldPartBeingEdited.name;
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, true, DuplicatePart, input);
        }

        public ColoredTowerPart GetColoredTowerPart1() { return ActiveFactory.GetColoredTowerPart1(); }
        
        public ColoredTowerPart GetColoredTowerPart2() { return ActiveFactory.GetColoredTowerPart2(); }

        void DuplicatePart()
        {
            if (_modelManager.NameInUse(TC_UI_ConfirmationManager.Instance.PartName()))
            {
                const string errorMessage = "This name is already in use";
                TC_UI_ConfirmationManager.Instance.PromptMessage(errorMessage, false, false);
                return;
            }
            TowerComponent newPart = Instantiate(_oldPartBeingEdited);
            newPart.SlotNumber = -1;
            newPart.GetComponent<TComp_TowerState>()?.ClearPaths();
            newPart.GetComponent<TComp_TowerState>()?.ClearRoot();
            newPart.name = TC_UI_ConfirmationManager.Instance.PartName();
            SavePart(ref newPart);
        }

        void SetEditScene()
        {
            switch (_oldPartBeingEdited)
            {
                case TP_Ammo_Spray c: _sprayAmmoSceneSetup.Invoke(); break;
                case TP_Wep_Sprayer c: _sprayerWeaponSceneSetup.Invoke(); break;
                case TP_Wep_Melee c: _meleeWeaponSceneSetup.Invoke(); break;
                case TP_Wep_Laser c: _laserWeaponSceneSetup.Invoke(); break;
                case TP_Wep_TargetingSystem c: _targetingSystemSceneSetup.Invoke(); break;
            }
        }

        void CreateTowerPart()
        {
            TowerComponent newPart = ActiveFactory.CreateTowerPart(TC_UI_ConfirmationManager.Instance.PartName());
            if (_isEditing){EditPart(ref newPart);}
            SavePart(ref newPart);
        }

        void SavePart(ref TowerComponent newPart)
        {
            ISerializableTowerComponent serializableTowerComponent = newPart.GetComponent<ISerializableTowerComponent>();
            serializableTowerComponent.GenerateFileName();
            newPart.GetView().gameObject.SetActive(true);
            serializableTowerComponent.SaveToFile();
            serializableTowerComponent.GetInventory().AddPartToInventory(newPart);
            _modelManager.PlaceModel(newPart.gameObject);
            serializableTowerComponent.GetInventory().SortSlots();
            _returnNavigator.ReturnToSection();
        }
        
        void EditPart(ref TowerComponent newComponent)
        {
            newComponent.SlotNumber = _oldPartBeingEdited.SlotNumber;
            ISerializableTowerComponent oldPart = _oldPartBeingEdited.GetComponent<ISerializableTowerComponent>();
            ISerializableTowerComponent newPart = newComponent.GetComponent<ISerializableTowerComponent>();
            
            oldPart.DeleteFile(true);
            newPart.UpdateDependencies(_oldPartBeingEdited);
            SetIsEditing(false);
            Destroy(_oldPartBeingEdited.gameObject);
            _oldPartBeingEdited = null;
        }
    }
}