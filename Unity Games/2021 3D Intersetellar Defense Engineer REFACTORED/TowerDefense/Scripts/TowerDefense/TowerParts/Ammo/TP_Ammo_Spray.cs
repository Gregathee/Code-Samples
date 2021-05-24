using System;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.Factories.Ammo;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerParts.Ammo
{
    /// <summary>
    /// Ammunition for sprayer weapons.
    /// </summary>
    public class TP_Ammo_Spray : TP_Ammo
    {
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Ammo_Spray Factory;
        
        public static readonly KeyValuePair<float, float> PENETRATION_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(0, 10);
        public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<int, int> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<int, int>(1, 5);
        
        public override void SaveToFile()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Slot Number", SlotNumber.ToString()},
                {"Damage Type", ((int) damageType).ToString()},
                {"Damage", _damage.ToString()},
                {"DOT", _dot.ToString()},
                {"DOT Time", _dotTime.ToString()},
                {"DOT Tics Per Sec", _dotTicsPerSec.ToString()},
                {"Penetration", _penetration.ToString()}
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            CustomPartFilePath = jsonDict["File Path"];
            SlotNumber = int.Parse(jsonDict["Slot Number"]);
            
            Mat1.color = HU_Functions.StringToColor(jsonDict["Mat 1 Color"]);
            Mat2.color = HU_Functions.StringToColor(jsonDict["Mat 2 Color"]);
            Mat3.color = HU_Functions.StringToColor(jsonDict["Mat 3 Color"]);
            SetMaterials(Mat1, Mat2, Mat3);
            
            damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            _damage = int.Parse(jsonDict["Damage"]);
            _dot = int.Parse(jsonDict["DOT"]);
            _dotTime = int.Parse(jsonDict["DOT Time"]);
            _penetration = int.Parse(jsonDict["Penetration"]);
            
            ClampProperties();
        }

        public override void DeleteFile(bool forceDelete)
        {
            if(forceDelete) {ForceDelete(); return; }
            if (FileInUse()) { return;}
            string message = "Are you sure you want to delete " + TD_Globals.PartNameColor + name + TD_Globals.StandardWordColor + "?";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, false, ForceDelete);
        }
        
        /// <summary>
        /// On edit, updates any weapon that references this.
        /// </summary>
        /// <param name="oldPart"></param>
        public override void UpdateDependencies(TowerComponent oldPart)
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_WEAPON_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro streamReader = new StreamReaderPro(file);
                Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
                if (jsonDict["Ammo File Path"] != oldPart.CustomPartFilePath){ continue;}
                TP_Wep_Sprayer weapon = TP_Wep_Sprayer.Inventory.FindTowerComponent(jsonDict["Name"]).GetComponent<TP_Wep_Sprayer>();
                weapon.SetAmmoFilePath(CustomPartFilePath);
                weapon.SaveToFile();
            }
        }
        
        public override TC_UI_TP_Inventory GetInventory() { return Inventory; }
        public override TC_Fac_TowerPartFactory GetFactory() { return Factory; }
        
        public override void GenerateFileName()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_AMMO_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Damage Type: " + TD_Globals.PartNameColor + damageType + "\n"; 
            stats += TD_Globals.StandardWordColor + "Impact Damage: " + TD_Globals.PartNameColor + _damage + "\n";
            stats += TD_Globals.StandardWordColor + "Damage Over Time(DOT): " + TD_Globals.PartNameColor + _dot + "\n";

            if (_dot > 0)
            {
                stats += TD_Globals.StandardWordColor + "DOT Duration: " + TD_Globals.PartNameColor + _dot + "\n";
                stats += TD_Globals.StandardWordColor + "DOT Tics Per Second: " + TD_Globals.PartNameColor + _dotTicsPerSec + "\n";
            }
            
            stats += TD_Globals.StandardWordColor + "Penetration: " + TD_Globals.PartNameColor + _penetration + "\n";
            
            return stats;
        }
        
        public override int Cost()
        {
            int result = 0;
        
            //TODO
            
            return result;
        }

        void ForceDelete()
        {
            File.Delete(CustomPartFilePath);
            Inventory.RemovePartFromInventory(this);
        }
        
        bool FileInUse()
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_WEAPON_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro streamReader = new StreamReaderPro(file);
                Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
                string message = "This projectile is being used by " + TD_Globals.PartNameColor + jsonDict["Name"] + 
                        TD_Globals.StandardWordColor + ".";
                TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
                return true;
            }
            return false;
        }

        //TODO add combat functionality

        void ClampProperties()
        {
            _penetration = Mathf.RoundToInt(Mathf.Clamp(_penetration, PENETRATION_BOUNDS.Key, PENETRATION_BOUNDS.Value));
            _damage =   Mathf.RoundToInt(Mathf.Clamp(_damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
            _dot = Mathf.RoundToInt(Mathf.Clamp(_dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
            _dotTime = Mathf.RoundToInt(Mathf.Clamp(_dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
            _dotTicsPerSec = Mathf.RoundToInt(Mathf.Clamp(_dotTicsPerSec, DOT_TICS_PER_SECOND_BOUNDS.Key, DOT_TICS_PER_SECOND_BOUNDS.Value));
            
            KeyValuePair<int, int> pair = DOT_TICS_PER_SECOND_BOUNDS;
            int dotTime = _dotTime;
            dotTime = pair.Value > dotTime ? dotTime : pair.Value;

            _dotTicsPerSec = Mathf.Clamp(_dotTicsPerSec, pair.Key, dotTime);
        }
    }
}
