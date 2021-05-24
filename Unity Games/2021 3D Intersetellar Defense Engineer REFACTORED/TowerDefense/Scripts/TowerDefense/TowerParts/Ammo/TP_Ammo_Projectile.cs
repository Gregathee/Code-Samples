using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HobbitUtilz;
using TowerDefense.Enemies;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.Factories.Ammo;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts.Weapon;

namespace TowerDefense.TowerParts.Ammo
{
    /// <summary>
    /// Ammunition for projectile weapons.
    /// </summary>
    public class TP_Ammo_Projectile : TP_Ammo
    {
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Ammo_Projectile Factory;
        
        //Key value pairs representing the lower (key) and upper (value) bounds of attributes
        public static readonly KeyValuePair<float, float> PENETRATION_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(0, 10);
        public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<int, int> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<int, int>(1, 5);
        public static readonly KeyValuePair<float, float> SPEED_BOUNDS = new KeyValuePair<float, float>(1, 50);
        public static readonly KeyValuePair<float, float> TRAVEL_TIME_BOUNDS = new KeyValuePair<float, float>(1, 3);
        public static readonly KeyValuePair<float, float> HOMING_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> AOE_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> ORDNANCE_PER_CLUSTER_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> CLUSTERS_BOUNDS = new KeyValuePair<float, float>(0, 3);
        
        [SerializeField] int _speed ;
        [SerializeField] int _travelTime = 5;
        [SerializeField] int _homing;
        [SerializeField] int _aoe;
        [SerializeField] int _ordnancePerCluster;
        [SerializeField] int _clusters;
        
        
        Transform _enemy;
        int _penetrated;

        delegate void TP_Behavior();
        TP_Behavior _behavior;

        protected override void Start()
        {
            base.Start();
            if (!IsPreview)
            {
                StartCoroutine(DestroyAfterTime());
                _behavior = Travel;
            }
            else { _behavior = base.Update; }
        }

        protected override void Update() { _behavior(); }
        
        void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;
            _penetrated++;
            enemy.TakeDamage(Damage);
        }
        
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
                {"Damage Type", ((int) damageType).ToString()},
                {"Damage", _damage.ToString()},
                {"DOT", _dot.ToString()},
                {"DOT Time", _dotTime.ToString()},
                {"DOT Tics Per Sec", _dotTicsPerSec.ToString()},
                {"Penetration", _penetration.ToString()},
                {"Speed", _speed.ToString() },
                {"Travel Time", _travelTime.ToString()},
                {"Homing", _homing.ToString()},
                {"AOE", _aoe.ToString()},
                {"Ordnance Per Cluster", _ordnancePerCluster.ToString()},
                {"Clusters", _clusters.ToString()}
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
            
            damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            _damage = int.Parse(jsonDict["Damage"]);
            _dot = int.Parse(jsonDict["DOT"]);
            _dotTime = int.Parse(jsonDict["DOT Time"]);
            _penetration = int.Parse(jsonDict["Penetration"]);
            _speed = int.Parse(jsonDict["Speed"]);
            _travelTime = int.Parse(jsonDict["Travel Time"]);
            _homing = int.Parse(jsonDict["Homing"]);
            _aoe = int.Parse(jsonDict["AOE"]);
            _ordnancePerCluster = int.Parse(jsonDict["Ordnance Per Cluster"]);
            _clusters = int.Parse(jsonDict["Clusters"]);
            
            ClampProperties();
        }
        
        public override void DeleteFile(bool forceDelete)
        {
            if(forceDelete) {ForceDelete(); return; }
            if (FileInUse()) { return;}
            string message = "Are you sure you want to delete " + TD_Globals.PartNameColor + name + TD_Globals.StandardWordColor + "?";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, false, ForceDelete);
        }
        
        /// <summary>
        /// On edit, update any weapon who references this.
        /// </summary>
        /// <param name="oldPart"></param>
        public override void UpdateDependencies(TowerComponent oldPart)
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_WEAPON_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro streamReader = new StreamReaderPro(file);
                Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
                if (jsonDict["Ammo File Path"] != oldPart.CustomPartFilePath){ continue;}
                TP_Wep_Projectile weapon = TP_Wep_Projectile.Inventory.FindTowerComponent(jsonDict["Name"]).GetComponent<TP_Wep_Projectile>();
                weapon.SetAmmoFilePath(CustomPartFilePath);
                weapon.SaveToFile();
            }
        }
        
        public override TC_UI_TP_Inventory GetInventory() { return Inventory; }

        public override TC_Fac_TowerPartFactory GetFactory() { return Factory; }
        
        public override void GenerateFileName()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_AMMO_DIR + name + ".json";
        }

        public override void SetIsPreview(bool preview)
        {
            base.SetIsPreview(preview);
            _behavior = (IsPreview = preview) ? (TP_Behavior) base.Update : Travel;
        }
        
        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost:" + TD_Globals.PartNameColor + Cost() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Damage Type:" + TD_Globals.PartNameColor + damageType + "\n"; 
            stats += TD_Globals.StandardWordColor + "Impact Damage:" + TD_Globals.PartNameColor + _damage + "\n";
            stats += TD_Globals.StandardWordColor + "Damage Over Time(DOT):" + TD_Globals.PartNameColor + _dot + "\n";

            if (_dot > 0)
            {
                stats += TD_Globals.StandardWordColor + "DOT Duration:" + TD_Globals.PartNameColor + _dot + "\n";
                stats += TD_Globals.StandardWordColor + "DOT Tics Per Second:" + TD_Globals.PartNameColor + _dotTicsPerSec + "\n";
            }
            
            stats += TD_Globals.StandardWordColor + "Penetration:" + TD_Globals.PartNameColor + _penetration + "\n";
            stats += TD_Globals.StandardWordColor + "Speed:" + TD_Globals.PartNameColor + _speed + "\n";
            stats += TD_Globals.StandardWordColor + "Travel Time:" + TD_Globals.PartNameColor + _travelTime + "\n";
            stats += TD_Globals.StandardWordColor + "Impact Blast Radius:" + TD_Globals.PartNameColor + _aoe + "\n";
            stats += TD_Globals.StandardWordColor + "Homing:" + TD_Globals.PartNameColor + _homing + "\n";
            stats += TD_Globals.StandardWordColor + "Clusters:" + TD_Globals.PartNameColor + _clusters + "\n";
            if(_clusters > 0){stats += TD_Globals.StandardWordColor + "Ordnance Per Cluster:" + TD_Globals.PartNameColor + _ordnancePerCluster + "\n";}
            
            return stats;
        }
        
        public override int Cost()
        {
            int result = 0;

            //TODO

            return result;
        }

        public int AOE { get => _aoe; set => _aoe = value; }
        public int Speed { get => _speed; set => _speed = value; }
        public int TravelTime { get => _travelTime; set => _travelTime = value; }
        public int Homing { get => _homing; set => _homing = value; }
        public int OrdnancePerCluster { get => _ordnancePerCluster; set => _ordnancePerCluster = value; }
        public int Clusters { get => _clusters; set => _clusters = value; }

        public void SetTarget(Transform targetIn) { _enemy = targetIn; }

        public float GetSpeed() { return Speed; }

        void ForceDelete()
        {
            File.Delete(CustomPartFilePath);
            Inventory.RemovePartFromInventory(this);
        }
        
        bool FileInUse()
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_WEAPON_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro streamReader = new StreamReaderPro(file);
                Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
                if (jsonDict["Ammo File Path"] != CustomPartFilePath) continue;
                string message = "This projectile is being used by " + TD_Globals.PartNameColor + jsonDict["Name"] + 
                    TD_Globals.StandardWordColor + ".";
                TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
                return true;
            }
            return false;
        }

        void ClampProperties()
        {
            _penetration = Mathf.RoundToInt(Mathf.Clamp(_penetration, PENETRATION_BOUNDS.Key, PENETRATION_BOUNDS.Value));
            _damage =   Mathf.RoundToInt(Mathf.Clamp(_damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
            _dot = Mathf.RoundToInt(Mathf.Clamp(_dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
            _dotTime = Mathf.RoundToInt(Mathf.Clamp(_dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
            _speed = Mathf.RoundToInt(Mathf.Clamp(_speed, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value)); 
            _travelTime = Mathf.RoundToInt(Mathf.Clamp(_travelTime, TRAVEL_TIME_BOUNDS.Key, TRAVEL_TIME_BOUNDS.Value));   
            _homing = Mathf.RoundToInt(Mathf.Clamp(_homing, HOMING_BOUNDS.Key, HOMING_BOUNDS.Value));  
            _aoe = Mathf.RoundToInt(Mathf.Clamp(_aoe, AOE_BOUNDS.Key, AOE_BOUNDS.Value));  
            _ordnancePerCluster = Mathf.RoundToInt(Mathf.Clamp(_ordnancePerCluster, ORDNANCE_PER_CLUSTER_BOUNDS.Key, ORDNANCE_PER_CLUSTER_BOUNDS.Value));    
            _clusters = Mathf.RoundToInt(Mathf.Clamp(_clusters, CLUSTERS_BOUNDS.Key, CLUSTERS_BOUNDS.Value));  
            
            KeyValuePair<int, int> pair = TP_Ammo_Projectile.DOT_TICS_PER_SECOND_BOUNDS;
            int dotTime = _dotTime;
            dotTime = pair.Value > dotTime ? dotTime : pair.Value;

            _dotTicsPerSec = Mathf.Clamp(_dotTicsPerSec, pair.Key, dotTime);
        }

        void Travel()
        {
            Transform transform1 = transform;
            transform1.localPosition += transform1.forward * (Speed * Time.deltaTime);

            if (_penetrated >= Penetration) { Destroy(gameObject); }
            if (_enemy == null) return;
            Vector3 targetPos = _enemy.position - transform.position;
            Quaternion targetHorizontalRotation = Quaternion.LookRotation(targetPos);
            transform.rotation = Quaternion.RotateTowards (transform.rotation, targetHorizontalRotation, Time.fixedDeltaTime * Homing);
        }

        IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(TravelTime);
            Destroy(gameObject);
        }
    }
}