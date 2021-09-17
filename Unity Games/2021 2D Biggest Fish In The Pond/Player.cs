using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Controls player's state and movements
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] GameObject _targetPoint;
    [SerializeField] GameObject _bubblePrefab;
    [SerializeField] GameObject _startMenu;
    [SerializeField] GameObject _winScreen;
    [SerializeField] GameObject _bounusCoinScreen;
    [SerializeField] TMP_Text _bounusCoinText;
    [SerializeField] TMP_Text _hpText;
    [SerializeField] int _minBubbles;
    [SerializeField] int _maxBubbles;
    [SerializeField] float _maxSize = 10;
    [SerializeField] float _speed = 5;
    [SerializeField] float _minDistance = 0.1f;
    [SerializeField] float _sizeOnEatModifier = 10;
    [SerializeField] float _coinsOnEatModifier = 100;
    [SerializeField] float _minBubbleInterval;
    [SerializeField] float _maxBubbleInterval;

    float _hp;

    void Start()
    {
        transform.position = Vector3.zero;
        float size = UpgradeManager.Instance.GetSize();
        transform.localScale = new Vector3(size, size, size);
        UpdateHealth();
    }

    void Update()
    {
        if (_startMenu.activeInHierarchy) { return; }
        if (transform.localScale.x > _maxSize) { Die(false); }
        SetTargetPoint();
        if (Vector2.Distance(transform.position, _targetPoint.transform.position) < _minDistance) { return; }
        DetermineRotation();
        MoveToTarget();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Fish fish = other.GetComponent<Fish>();
        
        // if fish is bigger than player
        if (fish.transform.localScale.x > transform.localScale.x)
        {
            _hp -= (fish.transform.localScale.x - transform.localScale.x) * _sizeOnEatModifier;
            if(_hp <= 0){ Die(true); return; }
            _hpText.text = Mathf.RoundToInt(_hp * 100).ToString();
        }
        else
        {
            // Increase size and add coins based on size of fish eaten
            Vector3 localScale = transform.localScale;
            float sizeIncrease = fish.transform.localScale.x / _sizeOnEatModifier;
            
            UpgradeManager.Instance.AddCoins(Mathf.RoundToInt(sizeIncrease * _coinsOnEatModifier));
            Monetizer.instance.coinsEarnedThisRound += Mathf.RoundToInt(sizeIncrease * _coinsOnEatModifier);
            
            localScale.x += sizeIncrease;
            localScale.y = localScale.x;
            transform.localScale = localScale;
            
            Destroy(other.gameObject);
            
            // Play bubbles animation and sound
            string[] strings = new[] {"Bubbles_1", "Bubbles_2"};
            string track = strings[Random.Range(0, strings.Length)];
            AudioManager.instance.PlaySFX(track);
            StartCoroutine(SpawnBubbles());
        }
    }
    
    /// <summary>
    /// Resets health to base health and updates in-game heath text
    /// </summary>
    public void UpdateHealth()
    {
        _hp = UpgradeManager.Instance.GetHP();
        _hpText.text = Mathf.RoundToInt(_hp * 100).ToString();
    }

    /// <summary>
    /// Sets target for player to move towards based on mouse click position
    /// </summary>
    void SetTargetPoint()
    {
        if (!Input.GetMouseButton(0)) return;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        _targetPoint.transform.position = targetPosition;
    }

    /// <summary>
    /// Changes facing to left or right depending on target position
    /// </summary>
    void DetermineRotation()
    {
        Vector3 localEulerAngles = transform.localEulerAngles;
        localEulerAngles.y = _targetPoint.transform.position.x > transform.position.x ? 180 : 0;
        transform.localEulerAngles = localEulerAngles;
    }

    /// <summary>
    /// Moves player towards target
    /// </summary>
    void MoveToTarget()
    {
        Vector3 direction = (_targetPoint.transform.position - transform.position).normalized;
        transform.position += direction * (_speed + UpgradeManager.Instance.GetSpeed()) * Time.deltaTime;
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    /// <param name="playAd">Should an add be played or should the win screen pop up?</param>
    void Die(bool playAd)
    {
        UpgradeManager.Instance.Save();
        
        transform.position = Vector3.zero;
        
        float size = UpgradeManager.Instance.GetSize();
        transform.localScale = new Vector3(size, size, size);

        UpdateHealth();
        
        _startMenu.SetActive(true);
        
        FishSpawner.Instance.StopSpawn();
        
        AudioManager.instance.PlaySFX("LongBubbles_1");
        
        if (playAd) { Monetizer.instance.DisplayInterstitialAd(); }
        else { _winScreen.SetActive(true); }
        
        // Destroy all remaining fish
        Fish[] fishes = GameObject.FindObjectsOfType<Fish>();
        foreach(Fish fish in fishes) { Destroy(fish.gameObject); }
        
        // Pop up bonus coin opportunity if any coins were earned this round.
        if (Monetizer.instance.coinsEarnedThisRound > 0)
        {
            _bounusCoinScreen.SetActive(true);
            _bounusCoinText.text = Monetizer.instance.coinsEarnedThisRound.ToString();
        }
    }

    /// <summary>
    /// Plays bubble animation
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBubbles()
    {
        int bubbles = Random.Range(_minBubbles, _maxBubbles);
        for (int i = 0; i < bubbles; i++)
        {
            Instantiate(_bubblePrefab, transform.position, new Quaternion());
            yield return new WaitForSeconds(Random.Range(_minBubbleInterval, _maxBubbleInterval));
        }
    }
}