using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns fish at random intervals and random positions on the left or right of screen.
/// </summary>
public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance;
    
    [SerializeField] float yBoundary = 4.5f;
    [SerializeField] float xBoundary = 10;
    [SerializeField] float minSpawnTime = 0.5f;
    [SerializeField] float maxSpawnTime = 2;
    [SerializeField] Fish[] _fishPrefabs;

    Coroutine _spawnFishRoutine;

    void Awake()
    {
        if (Instance) { Destroy(gameObject); }
        else { Instance = this; DontDestroyOnLoad(this); }
    }

    /// <summary>
    /// Starts spawning fish routine
    /// </summary>
    public void StartSpawn() { _spawnFishRoutine = StartCoroutine(SpawnFish()); }
    
    /// <summary>
    /// Stops spawing fish routine
    /// </summary>
    public void StopSpawn(){StopCoroutine(_spawnFishRoutine);}

    public float XBoundary() { return xBoundary;}

    /// <summary>
    /// Spawns fish at random intervals and random positions on the left or right of screen adjusting their facing accordingly
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnFish()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
           
            // Randomly determine which side of screen to spawn
            int side = Random.Range(0, 2);
            float x = xBoundary;
            
            //flip xBoundary and assign side to flip fish speed and fish facing
            if (side == 0)
            {
                x *= -1;
                side = 1;
            } else { side = -1;}

            // Spawn fish with randomized parameters
            float y = Random.Range(-yBoundary, yBoundary);
            int fishIndex = Random.Range(0, _fishPrefabs.Length);
            Fish fish = Instantiate(_fishPrefabs[fishIndex], new Vector2(x, y), new Quaternion());
            fish.speed *= Random.Range(fish.MinSpeed(), fish.MaxSpeed()) *  side;
            float scale = Random.Range(fish.MinScale(), fish.MaxScale());
            fish.transform.localScale = new Vector3(scale, scale, scale);
            
            // Determine facing of fish
            Vector3 localEulerAngles = fish.transform.localEulerAngles;
            localEulerAngles.y = side == -1 ? 180 : 0;
            fish.transform.localEulerAngles = localEulerAngles;
        }
    }
}
