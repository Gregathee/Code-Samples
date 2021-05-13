using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Weapon
{
	/// <summary>
	/// Weapon that does not have ammunition, does constant damage, can be manually aimed, ignores resistances, damage increases the longer its been in contact with the same enemy.
	/// </summary>
	public class TP_Wep_Laser : TP_Weapon
	{
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
	        CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.LASER_WEAPON_DIR + name + ".json";
	        Dictionary<string, string> dict = new Dictionary<string, string>()
	        {
		        {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Ammo Prefab Path", _ammoPrefabPath},
                {"Damage", _damage.ToString()},
                {"Rotation Speed", _rotationSpeed.ToString() },
                {"Can Shoot Up", HU_Functions.BoolToInt(_canShootUp).ToString()},
                {"Can Shoot Down", HU_Functions.BoolToInt(_canShootDown).ToString() },
                {"Fire Rate", _fireRate.ToString()},
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            _damage = int.Parse((jsonDict["Damage"]));
            _rotationSpeed = int.Parse(jsonDict["Rotation Speed"]);
            _canShootUp = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Up"]));
            _canShootDown = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Down"]));
            _fireRate = float.Parse(jsonDict["Fire Rate"]);
            CustomPartFilePath = jsonDict["File Path"];
            ClampProperties();
        }
        
        public override void Fire(Transform target)
        {
	        throw new System.NotImplementedException();
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