using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;
using TowerDefense.TowerParts.Ammo;

namespace TowerDefense.TowerParts.Weapon
{
    /// <summary>
    /// Weapon that does damage in an AOE around its origin.
    /// </summary>
    public class TP_Wep_Sprayer : TP_Weapon
    {
        public static readonly KeyValuePair<float, float> FIRE_RATE_BOUNDS = new KeyValuePair<float, float>(0.2F, 10);
        
        [SerializeField] protected Transform firePoint;
        [SerializeField] string _ammoFilePath;
        [SerializeField] float _fireRate = 1;
        [SerializeField] bool _canShootUp;
        [SerializeField] bool _canShootDown;
        [SerializeField] TurretAngle _sprayRange;
        
        bool canFire = true;
        
        public override  void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_WEAPON_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Ammo File Path", _ammoFilePath},
                {"Can Shoot Up", HU_Functions.BoolToInt(_canShootUp).ToString()},
                {"Can Shoot Down", HU_Functions.BoolToInt(_canShootDown).ToString() },
                {"Fire Rate", _fireRate.ToString()},
                {"Spray Range", ((int)_sprayRange).ToString()},
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override  void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            _ammoFilePath = jsonDict["Ammo File Path"];
            _sprayRange = (TurretAngle)(Mathf.Clamp(int.Parse(jsonDict["Spray Range"]), 0, 9));
            _canShootUp = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Up"]));
            _canShootDown = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Down"]));
            _fireRate = float.Parse(jsonDict["Fire Rate"]);
            _fireRate = Mathf.RoundToInt(Mathf.Clamp(_fireRate, FIRE_RATE_BOUNDS.Key, FIRE_RATE_BOUNDS.Value));
            CustomPartFilePath = jsonDict["File Path"];
        }

        public override void Fire(Transform target)
        {
            throw new System.NotImplementedException();
        }

        public bool CanShootUp { get => _canShootUp; set => _canShootUp = value; }
        public bool CanShootDown { get => _canShootDown; set => _canShootDown = value; }
        public float FireRate { get => _fireRate; set => _fireRate = value; }
        public TurretAngle SprayRange { get => _sprayRange; set => _sprayRange = value; }
        public void SetAmmoFilePath(string ammoIn) { _ammoFilePath = ammoIn; }
        public string GetAmmoPrefabPath() { return _ammoFilePath; }
        
        public Transform GetFirePoint() { return firePoint; }

        IEnumerator StartCooldown()
        {
            canFire = false;
            float wait = 1f / _fireRate;
            yield return new WaitForSeconds(wait);
            canFire = true;
        }
        
        //TODO add combat functionality.
    }
}