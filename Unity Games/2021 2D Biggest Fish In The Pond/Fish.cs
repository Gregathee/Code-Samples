using UnityEngine;

/// <summary>
/// Controls behavior of fish
/// </summary>
public class Fish : MonoBehaviour
{
    [SerializeField] float _minSpeed = 1;
    [SerializeField] float _maxSpeed = 7.5f;
    [SerializeField] float _minScale = 0.1f;
    [SerializeField] float _maxScale = 10;
    public float speed = 1;

    void Update()
    {
        Vector3 position = transform.position;
        position.x += speed * Time.deltaTime;
        transform.position = position;

        if (Mathf.Abs(transform.position.x) > FishSpawner.Instance.XBoundary() + 5) { Destroy(gameObject); }
    }

    public float MinSpeed() { return _minSpeed; }
    public float MaxSpeed() { return _maxSpeed; }
    public float MinScale() { return _minScale; }
    public float MaxScale() { return _maxScale; }
}
