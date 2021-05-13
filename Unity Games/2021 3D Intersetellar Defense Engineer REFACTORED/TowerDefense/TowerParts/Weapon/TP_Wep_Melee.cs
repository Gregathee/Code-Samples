using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Weapon
{
	/// <summary>
	/// Weapon that damages enemies by colliding with them.
	/// </summary>
	public class TP_Wep_Melee : TP_Weapon
	{
		public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
		public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(1, 10);
		public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
		public static readonly KeyValuePair<float, float> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<float, float>(1, 5);
		
		[SerializeField] WeaknessPriority _damageType;
		[SerializeField] int _damage;
		[SerializeField] int _dot;
		[SerializeField] int _dotTime;
		[SerializeField] int _dotTicsPerSec;
		
		public override void SaveToFile()
        {
	        CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.MELEE_WEAPON_DIR + name + ".json";
	        Dictionary<string, string> dict = new Dictionary<string, string>()
	        {
		        {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
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
            _damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            _damage = int.Parse(jsonDict["Damage"]);
            _dot = int.Parse(jsonDict["DOT"]);
            _dotTime = int.Parse(jsonDict["DOT Time"]);
            CustomPartFilePath = jsonDict["File Path"];
            ClampProperties();
        }
		
        public override void Fire(Transform target)
        {
	        throw new System.NotImplementedException();
        }

		public int Damage { get => _damage; set => _damage = value; }
		public WeaknessPriority DamageType { get => _damageType; set => _damageType = value; }
		
		void ClampProperties()
		{
			_damage =   Mathf.RoundToInt(Mathf.Clamp(_damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
			_dot = Mathf.RoundToInt(Mathf.Clamp(_dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
			_dotTime = Mathf.RoundToInt(Mathf.Clamp(_dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
			_dotTicsPerSec = Mathf.RoundToInt(Mathf.Clamp(_dotTicsPerSec, DOT_TICS_PER_SECOND_BOUNDS.Key, DOT_TICS_PER_SECOND_BOUNDS.Value));  
		}
		
		//TODO add combat functionality. 
	}
}