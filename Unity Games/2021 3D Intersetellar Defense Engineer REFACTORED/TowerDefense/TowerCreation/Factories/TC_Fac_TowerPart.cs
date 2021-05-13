using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerCreation.Factories.Weapon;
using TowerDefense.TowerCreation.Factories.Ammo;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates tower parts
    /// </summary>
    public class TC_Fac_TowerPart : MonoBehaviour
    {
        public static bool IsAttachingWeapons;
        public bool Editing;

        [SerializeField] TC_ModelManager _modelManager;
        [SerializeField] TC_Fac_Ammo_Projectile _ammoProjectileFactory;
        [SerializeField] TC_UI_TP_Inventory _projectileAmmoInventory;
        [SerializeField] TC_Fac_Ammo_Spray _sprayAmmoFactory;
        [SerializeField] TC_UI_TP_Inventory _sprayAmmoInventory;
        [SerializeField] TC_Fac_Wep_Projectile _projectileWeaponFactory;
        [SerializeField] TC_UI_TP_Inventory _projectileWeaponInventory;
        [SerializeField] TC_Fac_Wep_Sprayer _sprayerWeaponFactory;
        [SerializeField] TC_UI_TP_Inventory _sprayerWeaponInventory;
        [SerializeField] TC_Fac_Wep_Melee _meleeWeaponFactory;
        [SerializeField] TC_UI_TP_Inventory _meleeWeaponInventory;
        [SerializeField] TC_Fac_Wep_Laser _laserWeaponFactory;
        [SerializeField] TC_UI_TP_Inventory _laserWeaponInventory;
        [SerializeField] TC_Fac_Wep_TargetingSystem _targetingWeaponFactory;
        [SerializeField] TC_UI_TP_Inventory _targetingWeaponInventory;
        [SerializeField] TC_Fac_Tower _towerFactory;
        [SerializeField] TC_UI_TP_Inventory _towerInventory;
        [SerializeField] TC_Fac_Barricade _barricadeFactory;
        [SerializeField] TC_UI_TP_Inventory _barricadeInventory;


        public void CreatePart()
        {
            if (_ammoProjectileFactory.gameObject.activeInHierarchy) { CreateTowerPart(_ammoProjectileFactory, ref _projectileAmmoInventory); return; }
            if(_sprayAmmoFactory.gameObject.activeInHierarchy){ CreateTowerPart(_sprayAmmoFactory, ref _sprayAmmoInventory); return; }
            if(_projectileWeaponFactory.gameObject.activeInHierarchy){ CreateTowerPart(_projectileWeaponFactory, ref _projectileWeaponInventory); return; }
            if(_sprayerWeaponFactory.gameObject.activeInHierarchy){ CreateTowerPart(_sprayerWeaponFactory, ref _sprayerWeaponInventory); return; }
            if(_meleeWeaponFactory.gameObject.activeInHierarchy){ CreateTowerPart(_meleeWeaponFactory, ref _meleeWeaponInventory); return; }
            if(_laserWeaponFactory.gameObject.activeInHierarchy){ CreateTowerPart(_laserWeaponFactory, ref _laserWeaponInventory); return; }
            if(_targetingWeaponFactory.gameObject.activeInHierarchy){ CreateTowerPart(_targetingWeaponFactory, ref _targetingWeaponInventory); return; }
            if(_towerFactory.gameObject.activeInHierarchy){ CreateTowerPart(_towerFactory, ref _towerInventory); return; }
            if(_barricadeFactory.gameObject.activeInHierarchy){ CreateTowerPart(_barricadeFactory, ref _barricadeInventory); return; }
        }

        public void SetAttachingWeapons(bool isAttaching) { IsAttachingWeapons = isAttaching;}

        void CreateTowerPart<T>(TC_Fac_ITowerPartFactory<T> factory, ref TC_UI_TP_Inventory inventory) where T : TowerComponent, ISerializableTowerComponent
        {
            T towerPart = factory.CreateTowerPart();
            inventory.AddPartToInventory(towerPart);
            _modelManager.PlaceModel(towerPart.gameObject);
            towerPart.GetView().gameObject.SetActive(true);
            if (Editing)
            {
                // TODO
            }
            else { towerPart.SaveToFile(); }
        }
        
    }
}