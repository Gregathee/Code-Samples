using System;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerParts;
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

        [SerializeField] Camera _cameraPrefab;
        [SerializeField] GameObject[] _slotDisplays;
        [SerializeField] TC_ModelManager _modelManager;
        [SerializeField] TC_UI_SlotDetector _slotDetector;
        [SerializeField] Color _normalColor = new Color(0,0,0,0);
        [SerializeField] Color _selectedColor = new Color(0, 1, 0.1f, 0.2f);
        [SerializeField] float _distanceFromCamera = 5;
        [SerializeField] TP_Directory _partDirectory;

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
        }

        public void AddPartToInventory(TowerComponent towerComponent)
        {
            TC_UI_TP_SlotCollection slotCollection;
            slotCollection.part = towerComponent;
            slotCollection.slots = new List<TC_UI_TP_InventorySlot>();
            foreach (GameObject slotDisplay in _slotDisplays)
            {
                slotCollection.slots.Add(ConstructTowerPartUI(ref towerComponent, slotDisplay));
            }
            _inventory.Add(slotCollection);
        }
        
        static TC_UI_TP_InventorySlot BuildObjects(ref TowerComponent towerComponent, GameObject slotDisplay)
        {
            GameObject inventorySlot = new GameObject();
            inventorySlot.name = towerComponent.name + " inventory slot";
            GameObject slotImage = new GameObject();
            slotImage.name = "Slot image";
            inventorySlot.transform.SetParent(slotDisplay.transform);
            slotImage.transform.SetParent(inventorySlot.transform);
            inventorySlot.AddComponent<RectTransform>().pivot = new Vector2(0, 1);
            slotImage.AddComponent<RawImage>();
            //slotImage.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            inventorySlot.AddComponent<BoxCollider2D>();
            return inventorySlot.AddComponent<TC_UI_TP_InventorySlot>();
        }
        
         TC_UI_TP_InventorySlot ConstructTowerPartUI(ref TowerComponent towerComponent, GameObject slotDisplay )
         {
             TC_UI_TP_InventorySlot inventorySlot = BuildObjects(ref towerComponent, slotDisplay);
             AddButtonToObject(ref inventorySlot);
             AddEventToObject(ref inventorySlot);
             inventorySlot.Initialize(towerComponent, _slotDetector, _distanceFromCamera);
             return inventorySlot;
         }

         void AddButtonToObject(ref TC_UI_TP_InventorySlot inventorySlot)
         {
             RectTransform rectTrans = inventorySlot.GetComponentInChildren<RawImage>().GetComponent<RectTransform>();
             rectTrans.offsetMin = Vector2.zero;
             rectTrans.offsetMax = Vector2.zero;
             rectTrans.anchorMin = Vector2.zero; 
             rectTrans.anchorMax = new Vector2(1, 1);
             inventorySlot.gameObject.AddComponent<Image>();
             Button button = inventorySlot.gameObject.AddComponent<Button>();
             ColorBlock colorBlock = button.colors;
             colorBlock.selectedColor = _selectedColor;
             colorBlock.normalColor = _normalColor;
             colorBlock.highlightedColor = _selectedColor;
             colorBlock.pressedColor = _selectedColor;
             button.colors = colorBlock;
         }

         void AddEventToObject(ref TC_UI_TP_InventorySlot inventorySlot)
         {
             InventoryEventTrigger inventoryEventTrigger = inventorySlot.gameObject.AddComponent<InventoryEventTrigger>();
             inventoryEventTrigger.SetInventorySlot(inventorySlot);
             inventorySlot.AddEvents();
         }

         void DetermineDirectory()
         {
             _loadTowerComponentFromFile = TowerPart.LoadTowerPartFromFile;
             switch (_partDirectory)
             {
                 case TP_Directory.ProjectileAmmo: _directory = ROOT_DIR + PROJECTILE_AMMO_DIR; break;
                 case TP_Directory.SprayAmmo: _directory = ROOT_DIR + SPRAY_AMMO_DIR; break;
                 case TP_Directory.ProjectileWeapon: _directory = ROOT_DIR + PROJECTILE_WEAPON_DIR; break;
                 case TP_Directory.SprayerWeapon: _directory = ROOT_DIR + SPRAY_WEAPON_DIR; break;
                 case TP_Directory.MeleeWeapon: _directory = ROOT_DIR + MELEE_WEAPON_DIR; break;
                 case TP_Directory.LaserWeapon: _directory = ROOT_DIR + LASER_WEAPON_DIR; break;
                 case TP_Directory.TargetingSystem: _directory = ROOT_DIR + TARGETING_SYSTEM_DIR; break;
                 case TP_Directory.Barricade: _directory = ROOT_DIR + BARRICADE_DIR; break;
                 case TP_Directory.TowerState: 
                     _directory = ROOT_DIR + TOWER_STATE_DIR;
                     _loadTowerComponentFromFile = TComp_TowerState.LoadTowerPartFromFile;
                     Directory.CreateDirectory(ROOT_DIR + TOWER_BASE_DIR);
                     Directory.CreateDirectory(ROOT_DIR + WEAPON_MOUNT_DIR);
                     Directory.CreateDirectory(ROOT_DIR + WEAPON_MOUNT_STYLE_DIR);
                     break;
             }
         }
    }
}
