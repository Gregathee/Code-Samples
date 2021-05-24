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
	/// Weapon that does not have ammunition, does constant damage, can be manually aimed, ignores resistances, damage increases the longer its been in contact with the same enemy.
	/// </summary>
	public class TP_Wep_Laser : TP_Weapon
	{
		public static TC_UI_TP_Inventory Inventory;
		public static TC_Fac_Wep_Laser Factory;
		
		public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
		public static readonly KeyValuePair<float, float> ROTATION_SPEED_BOUNDS = new KeyValuePair<float, float>(1, 100);
		public static readonly KeyValuePair<float, float> FIRE_RATE_BOUNDS = new KeyValuePair<float, float>(0.2F, 10);
		
		[SerializeField] string _ammoPrefabPath;
		[SerializeField] int _damage;
		[SerializeField] float _fireRate;
        [SerializeField] bool _canShootUp;
        [SerializeField] bool _canShootDown;
        [SerializeField] int _rotationSpeed = 1;
        [SerializeField] Color laserColor;

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
                {"Ammo Prefab Path", _ammoPrefabPath},
                {"Damage", _damage.ToString()},
                {"Rotation Speed", _rotationSpeed.ToString() },
                {"Can Shoot Up", _canShootUp.ToString()},
                {"Can Shoot Down", _canShootDown.ToString() },
                {"Fire Rate", _fireRate.ToString()},
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
            
            _damage = int.Parse((jsonDict["Damage"]));
            _rotationSpeed = int.Parse(jsonDict["Rotation Speed"]);
            _canShootUp = bool.Parse(jsonDict["Can Shoot Up"]);
            _canShootDown = bool.Parse(jsonDict["Can Shoot Down"]);
            _fireRate = float.Parse(jsonDict["Fire Rate"]);
            
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
	        CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.LASER_WEAPON_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
	        string stats = "";

	        stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Size: " + TD_Globals.PartNameColor + GetSize() + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Damage: " + TD_Globals.PartNameColor + _damage + "\n"; 
	        stats += TD_Globals.StandardWordColor + "Damage Rate: " + TD_Globals.PartNameColor + _fireRate + "\n"; 
	        if(_canShootDown) stats += stats + TD_Globals.PartNameColor + "Can damage ground units." + "\n";
	        if(_canShootUp) stats += stats + TD_Globals.PartNameColor + "Can damage air units." + "\n";
	        stats += TD_Globals.StandardWordColor + "Rotation speed: " + TD_Globals.PartNameColor + _rotationSpeed + "\n";
	        
            
	        return stats;
        }
        
        public override int Cost()
        {
	        int result = 0;

	        //TODO

	        return result;
        }

        public int Damage { get => _damage; set => _damage = value; }
        public float FireRate { get => _fireRate; set => _fireRate = value; }
        public int RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        public bool CanShootUp { get => _canShootUp; set => _canShootUp = value; }
        public bool CanShootDown { get => _canShootDown; set => _canShootDown = value; }
        public Color LaserColor { get => laserColor; set => laserColor = value; }
        
        void ClampProperties()
        {
	        _damage = Mathf.RoundToInt(Mathf.Clamp(_damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
	        _rotationSpeed = Mathf.RoundToInt(Mathf.Clamp(_rotationSpeed, ROTATION_SPEED_BOUNDS.Key, ROTATION_SPEED_BOUNDS.Value));
	        _fireRate = Mathf.RoundToInt(Mathf.Clamp(_fireRate, FIRE_RATE_BOUNDS.Key, FIRE_RATE_BOUNDS.Value));
        }
        
        //TODO add combat functionality.
	}
}