using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
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

        /// <summary>
        /// Changes size based on size property.
        /// </summary>
        public override void ScaleToSize()
        {
            base.ScaleToSize();
            foreach (WeaponMountSlot slot in _slots)
            {
                slot.GetWeapon()?.CompensateScale();
            }
        }
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.WEAPON_MOUNT_STYLE_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Name", name},
            };

            if(_slots == null){FindSlots();}
            for (int i = 0; i < _slots.Length; i++)
            {
                dict["Weapon " + i] = _slots[i].GetWeapon().CustomPartFilePath;
                dict["Weapon " + i + " Rotation"] = _slots[i].SavedLocalRotation.ToString();
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
                _slots[i].SavedLocalRotation = HU_Functions.StringToQuaternion(jsonDict["Weapon " + i + " Rotation"]);
                _slots[i].SetWeapon(weapon);
            }
        }
        public override TC_UI_TP_Inventory GetInventory()
        {
            throw new System.NotImplementedException();
        }
        
        public override TC_Fac_TowerPartFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateFileName() {}

        public override string GetStats()
        {
            throw new System.NotImplementedException();
        }
        
        public override int Cost()
        {
            int result = 0;

            //TODO

            return result;
        }
        public override void SetIsPreview(bool preview)
        {
            base.SetIsPreview(preview);
            if(_slots == null){FindSlots();}
            foreach (WeaponMountSlot slot in _slots)
            {
                if(slot.GetWeapon()) {slot.GetWeapon().SetIsPreview(false);}
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