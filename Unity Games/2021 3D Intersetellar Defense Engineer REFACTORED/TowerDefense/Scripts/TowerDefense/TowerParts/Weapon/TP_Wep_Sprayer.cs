using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.Factories.Weapon;
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
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Wep_Sprayer Factory;
        
        public static readonly KeyValuePair<float, float> FIRE_RATE_BOUNDS = new KeyValuePair<float, float>(0.2F, 10);

        [SerializeField] TP_Ammo_Spray _ammo;
        [SerializeField] protected Transform firePoint;
        [SerializeField] string _ammoFilePath;
        [SerializeField] float _fireRate = 1;
        [SerializeField] int _speed;
        [SerializeField] bool _canShootUp;
        [SerializeField] bool _canShootDown;
        [SerializeField] TurretAngle _sprayRange;
        
        bool canFire = true;
        
        public override  void SaveToFile()
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
                {"Ammo File Path", _ammoFilePath},
                {"Can Shoot Up", _canShootUp.ToString()},
                {"Can Shoot Down", _canShootDown.ToString() },
                {"Fire Rate", _fireRate.ToString()},
                {"Speed", _speed.ToString()},
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
            if (_ammoFilePath == "") { Debug.Log("[TP_Wep_Sprayer] " + name + " does not have ammo.");}
            CustomPartFilePath = jsonDict["File Path"];
            SlotNumber = int.Parse(jsonDict["Slot Number"]);
            
            Mat1.color = HU_Functions.StringToColor(jsonDict["Mat 1 Color"]);
            Mat2.color = HU_Functions.StringToColor(jsonDict["Mat 2 Color"]);
            Mat3.color = HU_Functions.StringToColor(jsonDict["Mat 3 Color"]);
            SetMaterials(Mat1, Mat2, Mat3);
            
            _sprayRange = (TurretAngle)(Mathf.Clamp(int.Parse(jsonDict["Spray Range"]), 0, 9));
            _canShootUp =bool.Parse(jsonDict["Can Shoot Up"]);
            _canShootDown = bool.Parse(jsonDict["Can Shoot Down"]);
            _fireRate = float.Parse(jsonDict["Fire Rate"]);
            _fireRate = Mathf.RoundToInt(Mathf.Clamp(_fireRate, FIRE_RATE_BOUNDS.Key, FIRE_RATE_BOUNDS.Value));
            _speed = int.Parse(jsonDict["Speed"]);
            
            _ammo = TowerPart.LoadTowerPartFromFile(_ammoFilePath).GetComponent<TP_Ammo_Spray>();
            _ammo.transform.SetParent(transform);
            _ammo.transform.localPosition = new Vector3(0, -500, 0);
            _ammo.gameObject.SetActive(false);
            
            SetSize((PartSize)int.Parse(jsonDict["Size"]), true);
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
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.SPRAY_WEAPON_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Size: " + TD_Globals.PartNameColor + GetSize() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Fire Rate: " + TD_Globals.PartNameColor + _fireRate + "\n";
            if(_canShootDown) stats += stats + TD_Globals.PartNameColor + "Can damage ground units." + "\n";
            if(_canShootUp) stats += stats + TD_Globals.PartNameColor + "Can damage air units." + "\n";
            
            return stats;
        }
        
        public override int Cost()
        {
            int result = 0;

            //TODO
            
            return result;
        }

        public bool CanShootUp { get => _canShootUp; set => _canShootUp = value; }
        public bool CanShootDown { get => _canShootDown; set => _canShootDown = value; }
        public float FireRate { get => _fireRate; set => _fireRate = value; }
        public int Speed { get => _speed; set => _speed = value; }
        public TurretAngle SprayRange { get => _sprayRange; set => _sprayRange = value; }
        public void SetAmmoFilePath(string ammoIn) { _ammoFilePath = ammoIn; }
        public string GetAmmoPrefabPath() { return _ammoFilePath; }
        public TP_Ammo_Spray GetAmmo() { return _ammo;}
        public void SetAmmo(TP_Ammo_Spray ammo) { _ammo = ammo;}
        
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