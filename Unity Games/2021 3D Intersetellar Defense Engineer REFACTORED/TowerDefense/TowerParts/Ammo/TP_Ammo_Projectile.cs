using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HobbitUtilz;
using TowerDefense.Enemies;
using TowerDefense.TowerCreation.UI.Inventory;

namespace TowerDefense.TowerParts.Ammo
{
    /// <summary>
    /// Ammunition for projectile weapons.
    /// </summary>
    public class TP_Ammo_Projectile : TP_Ammo
    {
        //Key value pairs representing the lower (key) and upper (value) bounds of attributes
        public static readonly KeyValuePair<float, float> PENETRATION_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> DOT_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> DOT_TIME_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> DOT_TICS_PER_SECOND_BOUNDS = new KeyValuePair<float, float>(1, 5);
        public static readonly KeyValuePair<float, float> SPEED_BOUNDS = new KeyValuePair<float, float>(1, 50);
        public static readonly KeyValuePair<float, float> TRAVEL_TIME_BOUNDS = new KeyValuePair<float, float>(1, 3);
        public static readonly KeyValuePair<float, float> HOMING_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> AOE_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> ORDNANCE_PER_CLUSTER_BOUNDS = new KeyValuePair<float, float>(1, 10);
        public static readonly KeyValuePair<float, float> CLUSTERS_BOUNDS = new KeyValuePair<float, float>(1, 3);
        
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
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_AMMO_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Damage Type", ((int) damageType).ToString()},
                {"Damage", damage.ToString()},
                {"DOT", dot.ToString()},
                {"DOT Time", dotTime.ToString()},
                {"DOT Tics Per Sec", dotTicsPerSec.ToString()},
                {"Penetration", penetration.ToString()},
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
            damageType = (WeaknessPriority)(int.Parse(jsonDict["Damage Type"]));
            damage = int.Parse(jsonDict["Damage"]);
            dot = int.Parse(jsonDict["DOT"]);
            dotTime = int.Parse(jsonDict["DOT Time"]);
            penetration = int.Parse(jsonDict["Penetration"]);
            _speed = int.Parse(jsonDict["Speed"]);
            _travelTime = int.Parse(jsonDict["Travel Time"]);
            _homing = int.Parse(jsonDict["Homing"]);
            _aoe = int.Parse(jsonDict["AOE"]);
            _ordnancePerCluster = int.Parse(jsonDict["Ordnance Per Cluster"]);
            _clusters = int.Parse(jsonDict["Clusters"]);
            CustomPartFilePath = jsonDict["File Path"];
            ClampProperties();
        }

        public override void SetIsPreview(bool preview)
        {
            base.SetIsPreview(preview);
            _behavior = (IsPreview = preview) ? (TP_Behavior) base.Update : Travel;
        }

        public int AOE { get => _aoe; set => _aoe = value; }
        public int Speed { get => _speed; set => _speed = value; }
        public int TravelTime { get => _travelTime; set => _travelTime = value; }
        public int Homing { get => _homing; set => _homing = value; }
        public int OrdnancePerCluster { get => _ordnancePerCluster; set => _ordnancePerCluster = value; }
        public int Clusters { get => _clusters; set => _clusters = value; }

        public void SetTarget(Transform targetIn) { _enemy = targetIn; }

        public float GetSpeed() { return Speed; }

        void ClampProperties()
        {
            penetration = Mathf.RoundToInt(Mathf.Clamp(penetration, PENETRATION_BOUNDS.Key, PENETRATION_BOUNDS.Value));
            damage =   Mathf.RoundToInt(Mathf.Clamp(damage, DAMAGE_BOUNDS.Key, DAMAGE_BOUNDS.Value));
            dot = Mathf.RoundToInt(Mathf.Clamp(dot, DOT_BOUNDS.Key, DOT_BOUNDS.Value));
            dotTime = Mathf.RoundToInt(Mathf.Clamp(dotTime, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value));  
            dotTicsPerSec = Mathf.RoundToInt(Mathf.Clamp(dotTicsPerSec, DOT_TICS_PER_SECOND_BOUNDS.Key, DOT_TICS_PER_SECOND_BOUNDS.Value));  
            _speed = Mathf.RoundToInt(Mathf.Clamp(_speed, DOT_TIME_BOUNDS.Key, DOT_TIME_BOUNDS.Value)); 
            _travelTime = Mathf.RoundToInt(Mathf.Clamp(_travelTime, TRAVEL_TIME_BOUNDS.Key, TRAVEL_TIME_BOUNDS.Value));   
            _homing = Mathf.RoundToInt(Mathf.Clamp(_homing, HOMING_BOUNDS.Key, HOMING_BOUNDS.Value));  
            _aoe = Mathf.RoundToInt(Mathf.Clamp(_aoe, AOE_BOUNDS.Key, AOE_BOUNDS.Value));  
            _ordnancePerCluster = Mathf.RoundToInt(Mathf.Clamp(_ordnancePerCluster, ORDNANCE_PER_CLUSTER_BOUNDS.Key, ORDNANCE_PER_CLUSTER_BOUNDS.Value));    
            _clusters = Mathf.RoundToInt(Mathf.Clamp(_clusters, CLUSTERS_BOUNDS.Key, CLUSTERS_BOUNDS.Value));  
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