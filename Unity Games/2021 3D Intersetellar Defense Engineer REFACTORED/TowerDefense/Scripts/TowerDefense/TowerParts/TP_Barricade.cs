using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// A barricade that is placed to slow down enemies. 
    /// </summary>
    public class TP_Barricade : ColoredTowerPart
    {
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Barricade Factory;
        
        [SerializeField] float _constructionTime;
        [SerializeField] int _durability;
        
        public static readonly KeyValuePair<float, float> CONSTRUCTION_TIME_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<int, int> DURABILITY_BOUNDS = new KeyValuePair<int, int>(1, 100);
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
                {"Construction Time", _constructionTime.ToString()},
                {"Durability", _durability.ToString()}
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
            
            _durability = int.Parse(jsonDict["Durability"]);
            _constructionTime = float.Parse(jsonDict["Construction Time"]);

            _durability = Mathf.Clamp(_durability, DURABILITY_BOUNDS.Key, DURABILITY_BOUNDS.Value);
            _constructionTime = Mathf.Clamp(_constructionTime, CONSTRUCTION_TIME_BOUNDS.Key, CONSTRUCTION_TIME_BOUNDS.Value);
        }

        public override void DeleteFile(bool forceDelete)
        {
            if (forceDelete) { ForceDelete(); return; }
            string message = "Are you sure you want to delete " + TD_Globals.PartNameColor + name + TD_Globals.StandardWordColor + "?";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, false, ForceDelete);
        }
        public override TC_UI_TP_Inventory GetInventory() { return Inventory; }
        
        public override TC_Fac_TowerPartFactory GetFactory() { return Factory; }
        
        public override void GenerateFileName()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.BARRICADE_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n";  
            stats += TD_Globals.StandardWordColor + "Durability: " + TD_Globals.PartNameColor + _durability + "\n"; 
            stats += TD_Globals.StandardWordColor + "Construction Time: " + TD_Globals.PartNameColor + _constructionTime + "\n"; 
            
            return stats;
        }
        
        public override int Cost()
        {
            int result = 0;


            return result;
        }

        public int Durability { get => _durability; set => _durability = value; }
        public float ConstructionTime { get => _constructionTime; set => _constructionTime = value; }
        
        void ForceDelete()
        {
            File.Delete(CustomPartFilePath);
            Inventory.RemovePartFromInventory(this);
        }
        
        //TODO add combat functionality.
    }
}
