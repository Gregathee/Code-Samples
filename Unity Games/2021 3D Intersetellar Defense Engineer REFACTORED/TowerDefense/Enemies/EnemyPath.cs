using UnityEngine;

namespace TowerDefense.Enemies
{
    public class EnemyPath : MonoBehaviour
    {
        [SerializeField] Transform[] _wayPoints;
        float _pathLength;

        void Awake() { for (int i = _wayPoints.Length - 1; i > 0; i--) { _pathLength += Vector3.Distance(_wayPoints[i].position, _wayPoints[i - 1].position); } }

        public Transform[] GetPath() { return _wayPoints; }
        public float GetPathLength() { return _pathLength; }
    }
}