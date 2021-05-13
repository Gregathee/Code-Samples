using System;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Base of a tower. Mostly cosmetic and no combat functionality.
    /// </summary>
    public class TP_TowerBase : ColoredTowerPart
    {
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
            // Set Material colors
        }
    }
}