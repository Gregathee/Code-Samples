using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Behavior of spawned bubbles
/// </summary>
public class Bubble : MonoBehaviour
{
    [SerializeField] float _minSpeed;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _minSize;
    [SerializeField] float _maxSize;
    Vector3 _target;
    float speed;

    void Start()
    {
        float scale = Random.Range(_minSize, _maxSize);
        transform.localScale = new Vector3(scale, scale, scale);
        speed = Random.Range(_minSpeed, _maxSpeed);

        float x = Random.Range(-1.0f, 1.0f);
        _target = transform.position + new Vector3(x, 1, 0);
    }

    void Update()
    {
        // If bubble has reached its destination, create a new random one
        if (Vector3.Distance(transform.position, _target) > 0.1f)
        {
            float x = Random.Range(-1.0f, 1.0f);
            _target = transform.position + new Vector3(x, 1, 0);
        }
        
        // Move towards target
        Vector3 direction = (_target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        // Destroy if to high
        if(transform.position.y > 20){Destroy(gameObject);}
    }
}
