using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnContainer 
{
    [SerializeField] Enemy enemy = null;
    [SerializeField] float timeTillNextSpawn = 0;

    public ref Enemy GetEnemy() { return ref enemy; }
    public float GetTimeTillNextSpawn() { return timeTillNextSpawn; }
}
