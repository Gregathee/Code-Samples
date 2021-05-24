using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI.Inventory
{
    /// <summary>
    /// Manages inventory slots of a tower part inventory.
    /// </summary>

    struct TC_UI_TP_SlotCollection
    {
        public TowerComponent part;
        public List<TC_UI_TP_InventorySlot> slots;
    }
    
    public class TC_UI_TP_Inventory : MonoBehaviour
    {
        public static readonly string ROOT_DIR = "Assets/Resources/Prefabs/PlayerMadePrefabs/";
        public static readonly string PROJECTILE_AMMO_DIR = "Ammo/ProjectileAmmo/";
        public static readonly string SPRAY_AMMO_DIR = "Ammo/SprayAmmo/";
        public static readonly string PROJECTILE_WEAPON_DIR = "Weapons/ProjectileWeapons/";
        public static readonly string SPRAY_WEAPON_DIR = "Weapons/SprayerWeapons/";
        public static readonly string TARGETING_SYSTEM_DIR = "Weapons/AdvancedTargetingSystems/";
        public static readonly string MELEE_WEAPON_DIR = "Weapons/MeleeWeapons/";
        public static readonly string LASER_WEAPON_DIR = "Weapons/LaserWeapons/";
        public static readonly string BARRICADE_DIR = "/Walls/";
        public static readonly string TOWER_STATE_DIR = "TowerStates/";
        public static readonly string WEAPON_MOUNT_DIR = "TowerStates/WeaponMounts/";
        public static readonly string WEAPON_MOUNT_STYLE_DIR = "TowerStates/MountStyles/";
        public static readonly string TOWER_BASE_DIR = "TowerStates/Bases/";

        [SerializeField] GameObject[] _slotDisplays;
        [SerializeField] TC_ModelManager _modelManager;
        [SerializeField] TC_UI_SlotDetector _slotDetector;
        [SerializeField] Sprite _buttonSprite;
        [SerializeField] Color _normalColor = new Color(0,0,0,0);
        [SerializeField] Color _selectedColor = new Color(0, 1, 0.1f, 0.2f);
        [SerializeField] float _distanceFromCamera = 5;
        [SerializeField] TP_Directory _partDirectory;
        [SerializeField] TC_UI_FloatingToolTip _toolTipPrefab;
        

        delegate void ActivateType();
        ActivateType _activateType;

        List<TC_UI_TP_SlotCollection> _inventory = new List<TC_UI_TP_SlotCollection>();
        string[] _files;
        string _directory;

        delegate TowerComponent LoadTowerComponentFromFile(string file);
        LoadTowerComponentFromFile _loadTowerComponentFromFile;

        public void Initialize()
        {
            DetermineDirectory();
            Directory.CreateDirectory(_directory);
            _files = Directory.GetFiles(_directory);
            foreach (string file in _files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                TowerComponent towerComponent = _loadTowerComponentFromFile(file);
                AddPartToInventory(towerComponent);
                _modelManager.PlaceModel(towerComponent.gameObject);
            }
            SortSlots();
        }

        /// <summary>
        /// Sorts slots according to the assigned slot number.
        /// </summary>
        public void SortSlots()
        {
            foreach (GameObject slotDisplay in _slotDisplays)
            {
                TC_UI_TP_InventorySlot[] slots = slotDisplay.GetComponentsInChildren<TC_UI_TP_InventorySlot>();
                Array.Sort(slots);
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].transform.SetSiblingIndex(i);
                }
            }
        }

        /// <summary>
        /// Adds a reference to a tower component to inventory, constructs its UI element and inserts it to each inventory content display.
        /// </summary>
        /// <param name="towerComponent"></param>
        public void AddPartToInventory(TowerComponent towerComponent)
        {
            if(towerComponent.SlotNumber < 0){ towerComponent.SlotNumber = _inventory.Count;}
            TC_UI_TP_SlotCollection slotCollection;
            slotCollection.part = towerComponent;
            slotCollection.slots = new List<TC_UI_TP_InventorySlot>();
            foreach (GameObject slotDisplay in _slotDisplays)
            {
                slotCollection.slots.Add(ConstructTowerPartUI(ref towerComponent, slotDisplay));
            }
            _inventory.Add(slotCollection);
        }

        /// <summary>
        /// Removes reference to a tower component in inventory, deletes its UI elements in each inventory content display.
        /// </summary>
        /// <param name="towerComponent"></param>
        public void RemovePartFromInventory(TowerComponent towerComponent)
        {
            foreach (TC_UI_TP_SlotCollection slotCollection in _inventory)
            {
                if (slotCollection.part != towerComponent){ continue;}
                foreach (TC_UI_TP_InventorySlot slot in slotCollection.slots)
                {
                    if(slot.GetTowerComponent()) {Destroy(slot.GetTowerComponent().gameObject);}
                    Destroy(slot.gameObject);
                }
                _inventory.Remove(slotCollection);
                return;
            }
        }

        /// <summary>
        /// Wipes inventory and reloads from files
        /// </summary>
        public void ReloadInventory()
        {
            foreach (TC_UI_TP_SlotCollection slotCollection in _inventory)
            {
                foreach (TC_UI_TP_InventorySlot slot in slotCollection.slots)
                {
                    if(slot.GetTowerComponent()) {Destroy(slot.GetTowerComponent().gameObject);}
                    Destroy(slot.gameObject);
                }
            }
            _inventory.Clear();
            Initialize();
        }

        /// <summary>
        /// To save on performance disables all floating tower components except for the ones of the active inventory.
        /// </summary>
        public void ActivateInventory() { if(_activateType != null){_activateType();} }

        /// <summary>
        /// Returns a tower component from inventory by name.
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public TowerComponent FindTowerComponent(string componentName)
        {
            foreach (TC_UI_TP_SlotCollection slotCollection in _inventory)
            {
                if (slotCollection.part.name == componentName){ return slotCollection.part;}
            }
            Debug.Log("[TC_UI_TP_Inventory.FindTowerComponent] Could not find " + componentName);
            return null;
        }
        
        /// <summary>
        /// One step of constructing a inventory slot.
        /// </summary>
        /// <param name="towerComponent"></param>
        /// <param name="slotDisplay"></param>
        /// <returns></returns>
        static TC_UI_TP_InventorySlot BuildObjects(ref TowerComponent towerComponent, GameObject slotDisplay)
        {
            GameObject inventorySlot = new GameObject();
            inventorySlot.name = towerComponent.name + " inventory slot";
            inventorySlot.transform.SetParent(slotDisplay.transform, false);
            inventorySlot.AddComponent<RectTransform>().pivot = new Vector2(0, 1);
            
            GameObject slotImage = new GameObject();
            slotImage.name = "Slot image";
            slotImage.transform.SetParent(inventorySlot.transform, false);
            slotImage.AddComponent<RawImage>();

            return inventorySlot.AddComponent<TC_UI_TP_InventorySlot>();
        }
        
        /// <summary>
        /// One step of constructing a inventory slot.
        /// </summary>
        /// <param name="towerComponent"></param>
        /// <param name="slotDisplay"></param>
        /// <returns></returns>
         TC_UI_TP_InventorySlot ConstructTowerPartUI(ref TowerComponent towerComponent, GameObject slotDisplay )
         {
             TC_UI_TP_InventorySlot inventorySlot = BuildObjects(ref towerComponent, slotDisplay);
             TC_UI_TP_InventorySlotProperties slotProperties = CreateSlotProperties(ref towerComponent);
             inventorySlot.Initialize(slotProperties);
             inventorySlot.gameObject.AddComponent<TC_UI_PropagateDrag>();
             return inventorySlot;
         }

        /// <summary>
        /// One step of constructing a inventory slot.
        /// </summary>
        /// <param name="towerComponent"></param>
        /// <returns></returns>
         TC_UI_TP_InventorySlotProperties CreateSlotProperties(ref TowerComponent towerComponent)
         {
             TC_UI_TP_InventorySlotProperties slotProperties;
             slotProperties.TowerComponent = towerComponent;
             slotProperties.SlotDetector = _slotDetector;
             slotProperties.DistanceFromCamera = _distanceFromCamera;
             slotProperties.SelectedColor = _selectedColor;
             slotProperties.NormalColor = _normalColor;
             slotProperties.ButtonSprite = _buttonSprite;
             slotProperties.ToolTip = _toolTipPrefab;
             return slotProperties;
         }

        /// <summary>
        /// Determine which tower component directory to load tower components from on start depending on inspector settings.
        /// </summary>
         void DetermineDirectory()
         {
             _loadTowerComponentFromFile = TowerPart.LoadTowerPartFromFile;
             switch (_partDirectory)
             {
                 case TP_Directory.ProjectileAmmo: 
                     InitializeDirectory<TP_Ammo_Projectile>(ROOT_DIR + PROJECTILE_AMMO_DIR);
                     TP_Ammo_Projectile.Inventory = this;
                     break;
                 case TP_Directory.SprayAmmo: 
                     InitializeDirectory<TP_Ammo_Spray>(ROOT_DIR + SPRAY_AMMO_DIR);
                     TP_Ammo_Spray.Inventory = this;
                     break;
                 case TP_Directory.ProjectileWeapon: 
                     InitializeDirectory<TP_Wep_Projectile>(ROOT_DIR + PROJECTILE_WEAPON_DIR);
                     TP_Wep_Projectile.Inventory = this;
                     break;
                 case TP_Directory.SprayerWeapon: 
                     InitializeDirectory<TP_Wep_Sprayer>(ROOT_DIR + SPRAY_WEAPON_DIR); 
                     TP_Wep_Sprayer.Inventory = this;
                     break;
                 case TP_Directory.MeleeWeapon: 
                     InitializeDirectory<TP_Wep_Melee>(ROOT_DIR + MELEE_WEAPON_DIR); 
                     TP_Wep_Melee.Inventory = this;
                     break;
                 case TP_Directory.LaserWeapon: 
                     InitializeDirectory<TP_Wep_Laser>(ROOT_DIR + LASER_WEAPON_DIR); 
                     TP_Wep_Laser.Inventory = this;
                     break;
                 case TP_Directory.TargetingSystem: 
                     InitializeDirectory<TP_Wep_TargetingSystem>(ROOT_DIR + TARGETING_SYSTEM_DIR); 
                     TP_Wep_TargetingSystem.Inventory = this;
                     break;
                 case TP_Directory.Barricade: 
                     InitializeDirectory<TP_Barricade>(ROOT_DIR + BARRICADE_DIR); 
                     TP_Barricade.Inventory = this;
                     break;
                 case TP_Directory.TowerState: 
                     _loadTowerComponentFromFile = TComp_TowerState.LoadTowerPartFromFile;
                     Directory.CreateDirectory(ROOT_DIR + TOWER_BASE_DIR);
                     Directory.CreateDirectory(ROOT_DIR + WEAPON_MOUNT_DIR);
                     Directory.CreateDirectory(ROOT_DIR + WEAPON_MOUNT_STYLE_DIR);
                     InitializeDirectory<TComp_TowerState>(ROOT_DIR + TOWER_STATE_DIR);
                     TComp_TowerState.Inventory = this;
                     break;
             }
         }

        /// <summary>
        /// For some reason doing this on awake skips some step in the UI rendering process so it is remotely initialized.
        /// </summary>
        /// <param name="directory"></param>
        /// <typeparam name="T"></typeparam>
         void InitializeDirectory<T>(string directory) where T : TowerComponent
         {
             _directory = directory;
             _activateType = _modelManager.ActivateType<T>;
         }
    }
}
