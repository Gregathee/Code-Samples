using System;
using UnityEngine;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Object that moves along a path and gets shot at by turrets.
    /// </summary>
    public class Enemy : MonoBehaviour, IComparable<Enemy>
    {
        public bool Debug;
        public bool Stealthed;
        public bool Flying;

        [SerializeField] EnemyHealthBar enemyHealthBar;
        [SerializeField] int _startHitPoints = 1;
        [SerializeField] int _physicalResistance;
        [SerializeField] int _fireResistance;
        [SerializeField] int _iceResistance;
        [SerializeField] int _lightningResistance;
        [SerializeField] int _poisonResistance;
        [SerializeField] int _pierceResistance;
        [SerializeField] float _speed = 1;
        [SerializeField] float _timeTillNextSpawn;
        Transform[] _path;
        Vector3 _startPos = Vector3.zero;
        int _pathIndex;
        int _currentHitPoints;
        int _position;
        float _distanceToGo;
        bool _pathIsAssigned;
        bool _isDead;


        void Start()
        {
            _currentHitPoints = _startHitPoints;
            _startPos = transform.position;
            if (!Debug) return;
            _speed = UnityEngine.Random.Range(10f, 30f);
            float scale = UnityEngine.Random.Range(1f, 5f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
        void Update()
        {
            if (_pathIsAssigned) { Move(); }
            DistanceTraveled();
            name = _position.ToString();
            if (Debug) { _currentHitPoints = 1000000; }
        }

        public int GetCurrentHealth() { return _currentHitPoints; }

        public float GetNextSpawnTime() { return _timeTillNextSpawn; }

        public bool IsStealthed() { return Stealthed; }

        public int GetResistance(WeaknessPriority weakness)
        {
            switch (weakness)
            {
                case WeaknessPriority.Physical: return _physicalResistance;
                case WeaknessPriority.Fire: return _fireResistance;
                case WeaknessPriority.Ice: return _iceResistance;
                case WeaknessPriority.Lightning: return _lightningResistance;
                case WeaknessPriority.Poison: return _poisonResistance;
                case WeaknessPriority.Piercing: return _pierceResistance;
                case WeaknessPriority.Stealth: return Stealthed ? 0 : 1;
                case WeaknessPriority.Flying: return Flying ? 0 : 1;
                case WeaknessPriority.Ground: return Flying ? 1 : 0;
                default: return 100;
            }
        }

        public void Move()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Transform transform1 = transform;
            Vector3 position1 = transform1.position;
            Vector3 pushDirection = (_path[_pathIndex].position - position1).normalized;
            rb.velocity = pushDirection * _speed;
            float distance = Vector3.Distance(position1, _path[_pathIndex].position);
            if ((distance > -1f && distance < 1f) && _pathIndex < _path.Length - 1)
            {
                rb.velocity = Vector3.zero;
                if (_pathIndex == 0) { _distanceToGo -= Vector3.Distance(transform.position, _startPos); }
                else { _distanceToGo -= Vector3.Distance(transform.position, _path[_pathIndex - 1].position); }
                _pathIndex++;
            }
            else if (_pathIndex == _path.Length - 1)
            {
                if (Debug) { Die(); }
            }
        }

        public void SetSize(float newSize) { transform.localScale = new Vector3(newSize, newSize, newSize); }

        public void AssignPath(EnemyPath enemyPathIn)
        {
            _path = enemyPathIn.GetPath();
            _distanceToGo = enemyPathIn.GetPathLength();
            _pathIsAssigned = true;
        }

        public void TakeDamage(int damage)
        {
            _currentHitPoints -= damage;
            if (_currentHitPoints <= 0 && !Debug) { Die(); }
            enemyHealthBar.SetStatus((float)((float)_currentHitPoints / (float)_startHitPoints));
        }

        public float DistanceTraveled()
        {
            if (!this) return 0;
            if (_pathIndex == 0) { return _distanceToGo = Vector3.Distance(transform.position, _startPos); }
            return _distanceToGo - Vector3.Distance(transform.position, _path[_pathIndex - 1].position) + _distanceToGo;
        }

        //public int GetPosition() { return position; }
        public void SetPosition(int pos) { _position = pos; }

        public bool IsDead() { return _isDead; }

        public int CompareTo(Enemy other)
        {
            if (!other) return -1;
            DistanceTraveled();
            other.DistanceTraveled();
            int result = DistanceTraveled().CompareTo(other.DistanceTraveled());

            if (result == 0) { result = 1; }
            return result;
        }

        void Die() { _isDead = true; }
    }
}