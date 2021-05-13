using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// A barricade that is placed to slow down enemies. 
    /// </summary>
    public class TP_Barricade : ColoredTowerPart
    {
        [SerializeField] float _constructionTime;
        [SerializeField] int _durability;
        
        public static readonly KeyValuePair<float, float> CONSTRUCTION_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<int, int> DURABILITY_BOUNDS = new KeyValuePair<int, int>(1, 100);
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.BARRICADE_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
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
            _durability = int.Parse(jsonDict["Durability"]);
            _constructionTime = float.Parse(jsonDict["Construction Time"]);
        }
        
        public int Durability { get => _durability; set => _durability = value; }
        public float ConstructionTime { get => _constructionTime; set => _constructionTime = value; }
        
        //TODO add combat functionality.
    }
}
