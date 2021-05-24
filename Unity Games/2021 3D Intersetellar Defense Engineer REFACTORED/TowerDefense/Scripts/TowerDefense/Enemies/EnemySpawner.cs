using UnityEngine;

namespace TowerDefense.Enemies
{
    /// <summary>
    /// Spawns enemies in waves. Incomplete.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] EnemyWave[] _waves;
        int _waveIndex;

        public void SpawnWave()
        {}
    }
}