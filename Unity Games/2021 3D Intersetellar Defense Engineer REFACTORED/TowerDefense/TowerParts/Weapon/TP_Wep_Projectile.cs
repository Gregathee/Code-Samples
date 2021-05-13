using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Weapon
{
    /// <summary>
    /// Weapon that fires projectiles that travel over time.
    /// </summary>
    public class TP_Wep_Projectile : TP_Weapon
    {
        const float ACCURACY_INCREMENTS = 2.5f;
        const float RECOIL_INCREMENTS = 2.5f;
        
        public static readonly KeyValuePair<float, float> ROTATION_SPEED_BOUNDS = new KeyValuePair<float, float>(1, 100);
        public static readonly KeyValuePair<float, float> FIRE_RATE_BOUNDS = new KeyValuePair<float, float>(0.2F, 10);
        public static readonly KeyValuePair<float, float> CRIT_CHANCE_BOUNDS = new KeyValuePair<float, float>(0, 100);
        public static readonly KeyValuePair<float, float> CRIT_DAMAGE_BOUNDS = new KeyValuePair<float, float>(1, 10);
        
        [SerializeField] protected Transform firePoint;
        [SerializeField] string _ammoFilePath;
        [SerializeField] GameObject _barrel;
        [SerializeField] Recoil _recoil = Recoil.Level1;
        [SerializeField] Accuracy _accuracy = Accuracy.Level1;
        [SerializeField] bool _canShootUp;
        [SerializeField] bool _canShootDown;
        [SerializeField] int _rotationSpeed = 1;
        [SerializeField] float _fireRate = 1;
        [SerializeField] int _critChance;
        [SerializeField] int _critDamage;
        
        bool canFire = true;
        float _fRecoil;
        float _fAccuracy;
        
        protected override void Start()
        {
            _fAccuracy = ((float)_accuracy) * ACCURACY_INCREMENTS;
            _fRecoil = ((float)Recoil) * RECOIL_INCREMENTS;
        }
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.PROJECTILE_WEAPON_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Ammo File Path", _ammoFilePath},
                {"Recoil", ((int)_recoil).ToString() },
                {"Accuracy", ((int)_accuracy).ToString() },
                {"Rotation Speed", _rotationSpeed.ToString() },
                {"Can Shoot Up", HU_Functions.BoolToInt(_canShootUp).ToString()},
                {"Can Shoot Down", HU_Functions.BoolToInt(_canShootDown).ToString() },
                {"Fire Rate", _fireRate.ToString()},
                {"Crit Chance", _critChance.ToString()},
                {"Crit Damage", _critDamage.ToString()},
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            _ammoFilePath = jsonDict["Ammo File Path"];
            _recoil = (Recoil)(Mathf.Clamp(int.Parse(jsonDict["Recoil"]), 0, 9));
            _accuracy = (Accuracy)(Mathf.Clamp(int.Parse(jsonDict["Accuracy"]), 0, 9));
            _rotationSpeed = int.Parse(jsonDict["Rotation Speed"]);
            _canShootUp = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Up"]));
            _canShootDown = HU_Functions.IntToBool(int.Parse(jsonDict["Can Shoot Down"]));
            _fireRate = float.Parse(jsonDict["Fire Rate"]);
            _critChance = int.Parse(jsonDict["Crit Chance"]);
            _critDamage = int.Parse(jsonDict["Crit Damage"]);
            CustomPartFilePath = jsonDict["File Path"];
            ClampProperties();
        }

        public override void Fire(Transform target)
        {
            if (!canFire) return;
            float y = Random.Range(0, _fAccuracy);
            //add vertical deviation
            int right = Random.Range(0, 2);
            if (right == 1) y *= -1;
            Vector3 eulerAngles = _barrel.transform.eulerAngles;
            Vector3 startingVRotation = new Vector3(eulerAngles.x, eulerAngles.y + y, eulerAngles.z);
            Quaternion startingQRotation = Quaternion.Euler(startingVRotation);
            //Instantiate(ammo, firePoint.position, startingQRotation).SetTarget(target);
            Debug.Log("Fix Fire Method");
            RecoilBarrel();
            StartCoroutine(StartCooldown());
        }

        public Recoil Recoil { get => _recoil; set => _recoil = value; }
        public Accuracy Accuracy { get => _accuracy; set => _accuracy = value; }
        public int RotationSpeed { get => _rotationSpeed; set => _rotationSpeed = value; }
        public bool CanShootUp { get => _canShootUp; set => _canShootUp = value; }
        public bool CanShootDown { get => _canShootDown; set => _canShootDown = value; }
        public float FireRate { get => _fireRate; set => _fireRate = value; }
        public int CritChance { get => _critChance; set => _critChance = value; }
        public int CritDamage { get => _critDamage; set => _critDamage = value; }

        public void SetAmmoFilePath(string ammoIn) { _ammoFilePath = ammoIn; }
        public string GetAmmoPrefabPath() { return _ammoFilePath; }
        public ref GameObject GetBarrel() { return ref _barrel; }
        public Transform GetFirePoint() { return firePoint; }

        void ClampProperties()
        {
            _rotationSpeed = Mathf.RoundToInt(Mathf.Clamp(_rotationSpeed, ROTATION_SPEED_BOUNDS.Key, ROTATION_SPEED_BOUNDS.Value));
            _fireRate = Mathf.RoundToInt(Mathf.Clamp(_fireRate, FIRE_RATE_BOUNDS.Key, FIRE_RATE_BOUNDS.Value));
            _critChance = Mathf.RoundToInt(Mathf.Clamp(_critChance, CRIT_CHANCE_BOUNDS.Key, CRIT_CHANCE_BOUNDS.Value));
            _critDamage = Mathf.RoundToInt(Mathf.Clamp(_critDamage, CRIT_DAMAGE_BOUNDS.Key, CRIT_DAMAGE_BOUNDS.Value));
        }

        void RecoilBarrel()
        {
            Vector3 startingVRotation = new Vector3(_barrel.transform.eulerAngles.x - _fRecoil, 0, 0);
            Quaternion startingQRotation = Quaternion.Euler(startingVRotation);
            _barrel.transform.localRotation = startingQRotation;
        }

        IEnumerator StartCooldown()
        {
            canFire = false;
            float wait = 1f / _fireRate;
            yield return new WaitForSeconds(wait);
            canFire = true;
        }
    }
}