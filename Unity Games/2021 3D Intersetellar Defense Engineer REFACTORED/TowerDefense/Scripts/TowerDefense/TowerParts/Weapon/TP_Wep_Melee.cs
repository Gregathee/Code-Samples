using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.Factories.Weapon;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Weapon
{
	/// <summary>
	/// Weapon that damages enemies by colliding with them.
	/// </summary>
	public class TP_Wep_Melee : TP_Weapon
	{
		public static TC_UI_TP_Inventory Inventory;
		public static TC_Fac_Wep_Melee Factory;
		
		public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
		public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(0, 10);
		public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
		public static readonly KeyValuePair<int, int> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<int, int>(1, 5);
		
		[SerializeField] WeaknessPriority _damageType;
		[SerializeField] int _damage;
		[SerializeField] int _dot;
		[SerializeField] int _dotTime;
		[SerializeField] int _dotTicsPerSec;
		
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
                {"Size", ((int) size).ToString()},
                {"Slot Number", SlotNumber.ToString()},
                {"Damage Type", ((int) _damageType).ToString()},
                {"Damage", _damage.ToString()},
                {"DOT", _dot.ToString()},
                {"DOT Time", _dotTime.ToString()},
                {"DOT Tics Per Sec", _dotTicsPerSec.ToString()},
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
            
            _damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            _damage = int.Parse(jsonDict["Damage"]);
            _dot = int.Parse(jsonDict["DOT"]);
            _dotTime = int.Parse(jsonDict["DOT Time"]);
            
            SetSize((PartSize)int.Parse(jsonDict["Size"]), true);
            ClampProperties();
        }
		
        public override void Fire(Transform target)
        {
	        throw new System.NotImplementedException();
        }
        
        public override void RemoveFromInventory() { Inventory.RemovePartFromInventory(this); }
        
        public override TC_UI_TP_Inventory GetInventory() { return Inventory; }
        public override TC_Fac_TowerPartFactory GetFactory() { return Factory; }
        
        public override void GenerateFileName()
        {
	        CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.MELEE_WEAPON_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
	        string stats = "";

	        stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Size: " + TD_Globals.PartNameColor + GetSize() + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Damage Type: " + TD_Globals.PartNameColor + _damageType + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Impact Damage: " + TD_Globals.PartNameColor + _damage + "\n";
	        stats += TD_Globals.StandardWordColor + "Damage Over Time(DOT): " + TD_Globals.PartNameColor + _dot + "\n";

	        if (_dot > 0)
	        {
		        stats += TD_Globals.StandardWordColor + "DOT Duration:" + TD_Globals.PartNameColor + _dot + "\n";
		        stats += TD_Globals.StandardWordColor + "DOT Tics Per Second:" + TD_Globals.PartNameColor + _dotTicsPerSec + "\n";
	        }
            
	        return stats;
        }
        
        public override int Cost()
        {
	        int result = 0;

	        //TODO

	        return result;
        }

		public int Damage { get => _damage; set => _damage = value; }
		public WeaknessPriority DamageType { get => _damageType; set => _damageType = value; }
		
		void ClampProperties()
		{
			_damage =   Mathf.RoundToInt(Mathf.Clamp(_damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
			_dot = Mathf.RoundToInt(Mathf.Clamp(_dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
			_dotTime = Mathf.RoundToInt(Mathf.Clamp(_dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
			
			KeyValuePair<int, int> pair = DOT_TICS_PER_SECOND_BOUNDS;
			int dotTime = _dotTime;
			dotTime = pair.Value > dotTime ? dotTime : pair.Value;

			_dotTicsPerSec = Mathf.Clamp(_dotTicsPerSec, pair.Key, dotTime);
		}
		
		//TODO add combat functionality. 
	}
}