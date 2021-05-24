using UnityEngine;

namespace TowerDefense.TowerParts.UX
{
    /// <summary>
    /// A beam that indicates if a turret is looking at an enemy
    /// </summary>
    public class TP_UX_TargetingBeam : MonoBehaviour
    {
        bool _enemyDetected;
        GameObject _target;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _target = other.gameObject;
                _enemyDetected = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _target = null;
                _enemyDetected = false;
            }
        }

        public bool EnemyDetected(ref GameObject aimPreview)
        {
            if (aimPreview == _target && _enemyDetected) return true;
            else return false;
        }
    }
}