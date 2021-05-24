using System;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.UI.Inventory;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Base of a tower. Mostly cosmetic and no combat functionality.
    /// </summary>
    public class TP_TowerBase : ColoredTowerPart
    {
        public static TC_UI_TP_Inventory Inventory;
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TOWER_BASE_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Name", name},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            CustomPartFilePath = jsonDict["File Path"];
            
            Mat1.color = HU_Functions.StringToColor(jsonDict["Mat 1 Color"]);
            Mat2.color = HU_Functions.StringToColor(jsonDict["Mat 2 Color"]);
            Mat3.color = HU_Functions.StringToColor(jsonDict["Mat 3 Color"]);
            SetMaterials(Mat1, Mat2, Mat3);
        }
        public override TC_UI_TP_Inventory GetInventory()
        {
            throw new NotImplementedException();
        }
        public override TC_Fac_TowerPartFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }

        public override void GenerateFileName() {}

        public override string GetStats()
        {
            throw new NotImplementedException();
        }
        
        public override int Cost()
        {
            int result = 0;


            return result;
        }
    }
}