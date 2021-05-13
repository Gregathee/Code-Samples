using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// A object that uses WeaponMountSlots to represents a weapon positions layout for a weapon mount. 
    /// </summary>
    public class TP_WeaponMountStyle : TowerPart
    {
        WeaponMountSlot[] _slots;

        void Start() { if (_slots == null) { FindSlots(); } }
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.WEAPON_MOUNT_STYLE_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Name", name},
            };

            for (int i = 0; i < _slots.Length; i++)
            {
                dict["Weapon " + i] = _slots[i].GetWeapon().CustomPartFilePath;
            }
            
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            CustomPartFilePath = jsonDict["File Path"];
            if (_slots == null) { FindSlots(); }
            for (int i = 0; i < _slots.Length; i++)
            {
                TP_Weapon weapon =  TowerPart.LoadTowerPartFromFile( jsonDict["Weapon " + i]).GetComponent<TP_Weapon>();
                _slots[i].SetWeapon(weapon);
            }
        }

        public WeaponMountSlot[] GetSlots() { return _slots; }

        public void ClearWeaponSlots() { foreach (WeaponMountSlot slot in _slots) { slot.ClearSlot(); } }

        void FindSlots()
        {
            _slots = GetComponentsInChildren<WeaponMountSlot>();
            for (int i = 0; i < _slots.Length; i++) { _slots[i].SetSlotNumber(i); }
        }
    }
}