using System.Collections.Generic;
using System.IO;
using TowerDefense.Enemies;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts.UX;
using UnityEngine;
using TowerDefense.TowerParts.Weapon;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Rotate able disk that sits on a tower base that weapons get attached to via a weapon mount style.
    /// </summary>
    public class TP_WeaponMount : ColoredTowerPart
    {
        public static readonly KeyValuePair<int, int> ROTATION_SPEED_BOUNDS = new KeyValuePair<int, int>(1, 100);
        public static readonly KeyValuePair<int, int> RANGE_BOUNDS = new KeyValuePair<int, int>(1, 100);
        
        [SerializeField] TP_WeaponMountStyle _mountStyle;
        [SerializeField] TP_Wep_TargetingSystem _targetingSystem;
        [SerializeField] TP_UX_AngleIndicator _angleIndicator;
        [SerializeField] GameObject _rangeIndicator;
        [SerializeField] TP_UX_TargetingBeam _targetingBeam;
        [SerializeField] Transform _home;
        [SerializeField] GameObject _aimPreview;
        [SerializeField] TurretAngle _turretAngle;
        
        [SerializeField] bool _advancedTargetingSystemActive;
        [SerializeField] bool _canShootUp;
        [SerializeField] bool _canShootDown;
        
        [SerializeField] int _rotationalSpeed = 1;
        [SerializeField] int _range = 1;
        
        List<Enemy> _enemies = new List<Enemy>();
        SphereCollider _detectionRadius;
        Enemy _target;
        TP_Weapon[] _weapons;
        Vector3 _horizontalTargetPosition;
        Vector3 _verTargetPos;
        Vector3 _currentTargetPosition = Vector3.zero;
        Vector3 _previewPosition;
        
        float _turnRadius = 360;
        
        bool _aimingAtHome;
        bool _previewMode = true;

        protected override void Update()
        {
            base.Update();
            if (_previewMode) return;
            
            ManageEnemies();
            if (_targetingBeam.EnemyDetected(ref _aimPreview)) { Fire(); }
            if (TurretAngle != TurretAngle.A0 && _target) { RotateHorizontal(); }
            if (_target)
                if (_canShootUp) { RotateVertical(); }
            SetBeamRotation();
            _aimPreview.transform.position = _previewPosition;
        }
        
        public override void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.WEAPON_MOUNT_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"Name", name},
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Rotation Speed", _rotationalSpeed.ToString()},
                {"Range", _range.ToString()},
                {"Turret Angle", ((int)_turretAngle).ToString()},
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            _rotationalSpeed = Mathf.RoundToInt(Mathf.Clamp(int.Parse(jsonDict["Rotation Speed"]), ROTATION_SPEED_BOUNDS.Key, ROTATION_SPEED_BOUNDS.Value));
            _range = Mathf.RoundToInt(Mathf.Clamp(int.Parse(jsonDict["Range"]), RANGE_BOUNDS.Key, RANGE_BOUNDS.Value));
            _turretAngle = ((TurretAngle)int.Parse(jsonDict["Turret Angle"]));
            CustomPartFilePath = jsonDict["File Path"];
        }

        public int RotationalSpeed { get => _rotationalSpeed; set => _rotationalSpeed = value; }
        public int Range { get => _range; set => _range = value; }
        public TurretAngle TurretAngle { get => _turretAngle; set => _turretAngle = value; }
        public void InitializeWeaponMount()
        {
            InitializeTurrets();
            SetIndicatorRanges();
            SetTurretAnge();
            _previewMode = false;
        }

        public void SetStyle(TP_WeaponMountStyle style)
        {
            _mountStyle = style;
            _mountStyle.transform.SetParent(transform);
            _mountStyle.transform.localPosition = Vector3.zero;
            _mountStyle.SetIsPreview(false);
        }
        public TP_WeaponMountStyle GetStyle() { return _mountStyle; }

        public bool EnemyBeyondTurnRadius(Enemy enemy)
        {
            if (!enemy) return true;
            Vector3 position = transform.position;
            Quaternion homeRotation = Quaternion.LookRotation(_home.transform.position - position);
            float relativeRotationToTarget = Quaternion.LookRotation(enemy.transform.position - position).eulerAngles.y - homeRotation.eulerAngles.y;
            if (relativeRotationToTarget < 0) relativeRotationToTarget = 360 + relativeRotationToTarget;
            return relativeRotationToTarget > _turnRadius / 2 && relativeRotationToTarget < 360 - (_turnRadius / 2);
        }

        void OnTriggerStay(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !EnemyBeyondTurnRadius(enemy) && !_enemies.Contains(enemy))
            {
                if ((enemy.Flying && _canShootUp) || (!enemy.Flying && _canShootDown)) { _enemies.Add(enemy); }
            }
        }

        void OnTriggerExit(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null) { _enemies.Remove(enemy); }
        }

        void InitializeTurrets()
        {
            WeaponMountSlot[] slots = _mountStyle.GetSlots();
            _weapons = new TP_Weapon[slots.Length];
            for (int i = 0; i < _weapons.Length; i++) { _weapons[i] = slots[i].GetWeapon(); }
        }

        void SetIndicatorRanges()
        {
            _detectionRadius = GetComponent<SphereCollider>();
            _detectionRadius.radius = Range;
            _angleIndicator.SetAngle(TurretAngle, Range);
            _angleIndicator.ShowIndicators(true);
            _rangeIndicator.transform.localScale = new Vector3(Range, Range, Range);
            _targetingBeam.transform.localScale = new Vector3(1, 1, Range * 1.5f);
        }

        void SetTurretAnge()
        {
            switch (TurretAngle)
            {
                case TurretAngle.A0: _turnRadius = 0; break;
                case TurretAngle.A45: _turnRadius = 45; break;
                case TurretAngle.A90: _turnRadius = 90; break;
                case TurretAngle.A135: _turnRadius = 135; break;
                case TurretAngle.A180: _turnRadius = 180; break;
                case TurretAngle.A225: _turnRadius = 225; break;
                case TurretAngle.A270: _turnRadius = 270; break;
                case TurretAngle.A315: _turnRadius = 315; break;
                case TurretAngle.A360: _turnRadius = 360; break;
            }
        }

        void SetBeamRotation()
        {
            // TP_Wep_Projectile weaponBarrel;
            // GameObject barrel;
            // weaponBarrel = _weapons[0].GetComponent<TP_Wep_Projectile>();
            // barrel = weaponBarrel.GetBarrel();
            // _beam.transform.rotation = barrel.transform.rotation;
        }

        void ManageEnemies()
        {
            RemoveNullAndBlindSpotTargets();
            _enemies.Sort();
            if (_enemies.Count <= 0) return;
            if (_enemies[0] != null) { FindTarget(); }
        }

        void RemoveNullAndBlindSpotTargets()
        {
            int count = _enemies.Count;
            
            for (int i = 0; i < count;)
            {
                if (_enemies[i] == null || EnemyBeyondTurnRadius(_enemies[i]))
                {
                    _enemies.RemoveAt(i);
                    count = _enemies.Count;
                }
                else i++;
            }
        }

        void FindTarget() { _target = !_advancedTargetingSystemActive ? _enemies[0] : _targetingSystem.PrioritizeTarget(this, ref _enemies); }

        void Fire()
        {
            if (!_target) return;
            foreach (TP_Weapon weapon in _weapons) { weapon.Fire(_target.transform); }
        }

        void RotateHorizontal()
        {
            //Rigidbody firePointRB = _weapons[0].GetFirePoint().GetComponent<Rigidbody>();
            //float shotSpeed = weapons[0].GetProjectile().GetSpeed();
            Debug.Log("Fix RotateHorizontal Method");
            Vector3 shooterPosition = transform.position;
            Vector3 targetPosition = Vector3.zero;

            DetermineTargetPosition(out Rigidbody targetRB, out targetPosition);

            //Vector3 shooterVelocity = firePointRB ? firePointRB.velocity : Vector3.zero;
            Vector3 targetVelocity = targetRB ? targetRB.velocity : Vector3.zero;

            //SetHorizontalTargetPosition(ref shooterPosition, ref shooterVelocity, ref shotSpeed, ref targetPosition, ref targetVelocity);
            SetHorizontalRotation();
            ContainRotationWithinTurretAngle();
        }

        void SetHorizontalRotation()
        {
            Vector3 newRotation = Vector3.RotateTowards(transform.forward, _horizontalTargetPosition, RotationalSpeed * Time.deltaTime, 1);
            Quaternion newNewRotation = new Quaternion();
            newNewRotation.SetLookRotation(newRotation, Vector3.up);
            transform.rotation = newNewRotation;
        }

        /// <summary>
        /// Set horizontal target position to position of target, only in terms of x and y.
        /// </summary>
        /// <param name="shooterPosition"></param>
        /// <param name="shooterVelocity"></param>
        /// <param name="shotSpeed"></param>
        /// <param name="targetPosition"></param>
        /// <param name="targetVelocity"></param>
        void SetHorizontalTargetPosition(ref Vector3 shooterPosition, ref Vector3 shooterVelocity, ref float shotSpeed, ref Vector3 targetPosition, ref Vector3 targetVelocity)
        {
            _horizontalTargetPosition = HU_Functions.FirstOrderIntercept
                (ref shooterPosition, ref shooterVelocity, ref shotSpeed, ref targetPosition, ref targetVelocity);
            AimHome();
            if (_target) _previewPosition = new Vector3(_horizontalTargetPosition.x, _target.transform.position.y, _horizontalTargetPosition.z);
            _horizontalTargetPosition -= transform.position;
            _currentTargetPosition = _horizontalTargetPosition;
            _horizontalTargetPosition.y = 0;
            _horizontalTargetPosition = _horizontalTargetPosition.normalized;
        }

        //return turret to home position to prevent attempts to rotate into blind spot
        void AimHome()
        {
            if (!(_turnRadius < 360) || !_target) return;
            if (EnemyBeyondTurnRadius(_target)) return;
            
            float relativeRotationToTarget = GetRelativeRotationToTarget();
            float rotationFromHome = GetRotationFromHome();
            //Debug.Log("T " + relativeRotationToTarget + " H " + homeRotation.eulerAngles.y + " H2 " + rotationFromHome);

            bool targetLessThan180 = relativeRotationToTarget <= 180;
            bool targetMoreThan180 = relativeRotationToTarget >= 180;
            bool homeLessThan180 = rotationFromHome <= 180;
            bool homeMoreThan180 = rotationFromHome >= 180;
            
            if ((targetLessThan180 &&  homeMoreThan180) || (targetMoreThan180 && homeLessThan180) && (rotationFromHome < -1 || rotationFromHome > 1))
            { //Correct turret from following target into blind spot.  
                _aimingAtHome = true;
            }
            else { _aimingAtHome = false; }
        }

        /// <summary>
        /// Returns the angle, in degrees, of the target and the forward vector
        /// </summary>
        /// <returns></returns>
        float GetRelativeRotationToTarget()
        {
            Vector3 position = transform.position;
            Quaternion homeRotation = Quaternion.LookRotation(_home.transform.position - position);
            float relativeRotationToTarget = Quaternion.LookRotation(_target.transform.position - position).eulerAngles.y - homeRotation.eulerAngles.y;
            if (relativeRotationToTarget < 0) { relativeRotationToTarget += 360;}
            return relativeRotationToTarget;
        }

        /// <summary>
        /// Returns the angle, in degrees, of the rotation of the weapon mount.
        /// </summary>
        /// <returns></returns>
        float GetRotationFromHome()
        {
            float rotationFromHome = transform.localEulerAngles.y;
            if (rotationFromHome < 0) { rotationFromHome += 360;}
            return rotationFromHome;
        }

        void DetermineTargetPosition(out Rigidbody targetRB, out Vector3 targetPosition)
        {
            if (!_target || _aimingAtHome)
            {
                targetPosition = _home.position;
                targetRB = new Rigidbody();
            }
            else //return turret to home position to prevent attempts to rotate into blind spot
            {
                targetPosition = _target.transform.position;
                targetRB = _target.GetComponent<Rigidbody>();
            }
        }

        /// <summary>
        /// Keeps turret from rotating out of its turret range.
        /// </summary>
        void ContainRotationWithinTurretAngle()
        {
            //Barrel rotates into blind spot
            if (!(transform.localEulerAngles.y > _turnRadius / 2) || !(transform.localEulerAngles.y < 360 - (_turnRadius / 2))) return;
            
            //Which side barrel entered blind spot
            if (transform.localEulerAngles.y > _turnRadius / 2 && transform.localEulerAngles.y < 180) 
            {
                transform.localEulerAngles = new Vector3(0, _turnRadius / 2, 0); //reset barrel to edge of blind spot
            }
            else { transform.localEulerAngles = new Vector3(0, 360 - (_turnRadius / 2), 0); } //reset barrel to edge of blind spot
        }

        void RotateVertical()
        {
            foreach (TP_Weapon weapon in _weapons)
            {
                // TP_Wep_Projectile weaponBarrel;
                // GameObject barrel;
                // if ((weaponBarrel = weapon.GetComponent<TP_Wep_Projectile>()) && _target != null)
                // {
                //     barrel = weaponBarrel.GetBarrel();
                //     SetVerticalTargetPosition();
                //     SetVerticalRotation(ref barrel);
                // }
            }
        }

        void SetVerticalTargetPosition()
        {
            _verTargetPos = _currentTargetPosition;
            _currentTargetPosition.y = _target.transform.position.y;
            if (!_target.Flying) _verTargetPos.y = -0.5f;
            _verTargetPos = _verTargetPos.normalized;
        }

        void SetVerticalRotation(ref GameObject barrel)
        {
            Vector3 newRotation = Vector3.RotateTowards(barrel.transform.forward, _verTargetPos, RotationalSpeed * Time.deltaTime, 1);
            Quaternion newNewRotation = new Quaternion();
            newNewRotation.SetLookRotation(newRotation, Vector3.up);
            barrel.transform.localRotation = new Quaternion(newNewRotation.x, 0, 0, newNewRotation.w);
        }
    }
}