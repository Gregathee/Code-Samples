using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Ammo
{
    /// <summary>
    /// Ammunition for sprayer weapons.
    /// </summary>
    public class TP_Ammo_Spray : TP_Ammo
    {
        public static readonly KeyValuePair<float, float> PENETRATION_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<float, float>(1, 5);
        
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_AMMO_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Damage Type", ((int) damageType).ToString()},
                {"Damage", damage.ToString()},
                {"DOT", dot.ToString()},
                {"DOT Time", dotTime.ToString()},
                {"DOT Tics Per Sec", dotTicsPerSec.ToString()},
                {"Penetration", penetration.ToString()}
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            damage = int.Parse(jsonDict["Damage"]);
            dot = int.Parse(jsonDict["DOT"]);
            dotTime = int.Parse(jsonDict["DOT Time"]);
            penetration = int.Parse(jsonDict["Penetration"]);
            CustomPartFilePath = jsonDict["File Path"];
            ClampProperties();
        }
        
        //TODO add combat functionality

        void ClampProperties()
        {
            penetration = Mathf.RoundToInt(Mathf.Clamp(penetration, PENETRATION_BOUNDS.Key, PENETRATION_BOUNDS.Value));
            damage =   Mathf.RoundToInt(Mathf.Clamp(damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
            dot = Mathf.RoundToInt(Mathf.Clamp(dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
            dotTime = Mathf.RoundToInt(Mathf.Clamp(dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
            dotTicsPerSec = Mathf.RoundToInt(Mathf.Clamp(dotTicsPerSec, DOT_TICS_PER_SECOND_BOUNDS.Key, DOT_TICS_PER_SECOND_BOUNDS.Value));
        }
    }
}
