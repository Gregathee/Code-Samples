/*
 * Astroid.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: Rotates, scales up and moves asteroid slightly down
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Astroid : MonoBehaviour
{
    [Tooltip("Speed asteroid moves to the bottom of the screen.")]
    [SerializeField] float downwardSpeed;
    [Tooltip("Rotation speed of asteroid. Use negative number to reverse rotation")]
    [SerializeField] float rotationSpeed;
    [Tooltip("Speed that asteroid will grow after being spawned.")]
    [SerializeField] float scaleSpeed;
    [Tooltip("Scale size that when reached, asteroid will cause damage and explode.")]
    [SerializeField] float scaleLimit;
    [Tooltip("Force applied to meteorites on explosion.")]
    [SerializeField] float meteoriteForce = 10;
    [Tooltip("Sprites for asteroids to be chosen at random.")]
    [SerializeField] Sprite[] sprites;
    [Tooltip("Objects that are instantiated on explosion.")]
    [SerializeField] GameObject[] meteoritePrefabs;
    [Tooltip("Object spawned on explosion.")]
    [SerializeField] GameObject explosion;

    List<GameObject> meteorites = new List<GameObject>();
    Image damageIndicator;
    Color originalColor;
    int rotationDirection;
    bool exploded = false;
    bool flashingIndicator = false;
    AstroidMiniGame miniGameManager;

    void Start()
    {
        miniGameManager = FindObjectOfType<AstroidMiniGame>();
        damageIndicator = GameObject.FindGameObjectWithTag("DamageIndicator").GetComponent<Image>();
        originalColor = damageIndicator.color;
        transform.localScale = Vector3.zero;
        int randomInt = Random.Range(0, 2);
        if(randomInt == 0) { rotationDirection = 1; }
        else { rotationDirection = -1; }
        GetComponent<Image>().sprite = sprites[Random.Range(0, sprites.Length)];
        StartCoroutine(IncreaseScale());
    }

    void Update()
    {
        float scale = transform.localScale.x + scaleSpeed * Time.deltaTime;
        if(scale > scaleLimit) {StartCoroutine(Explode(false)); }
        transform.localScale = new Vector3(scale, scale, scale);

        Vector3 position = transform.position;
        position.y -= downwardSpeed * Time.deltaTime;
        transform.position = position;

        Vector3 rotation = transform.eulerAngles;
        rotation.z += (rotationSpeed * rotationDirection * Time.deltaTime);
        transform.eulerAngles = rotation;

        if((miniGameManager.requiredAstroids == 0 || miniGameManager.damageTillFailure == 0) && !exploded) { StartCoroutine(Explode(true)); }
    }

    IEnumerator IncreaseScale()
	{
        float originalScaleSpeed = scaleSpeed;
        while(transform.localScale.x < scaleLimit && !exploded)
		{
            yield return new WaitForSeconds(0.5f);
            scaleSpeed += originalScaleSpeed;
            downwardSpeed += downwardSpeed/2;
		}
	}

    public void StartExploding() { StartCoroutine(Explode(true)); }

    /// <summary>
    /// Mark true if colision with rocket caused the explosion. Damage will be caused if marked false.
    /// </summary>
    /// <param name="fromRocket"></param>
    /// <returns></returns>
    public IEnumerator Explode(bool fromRocket)
	{
        if (!exploded)
        {
            exploded = true;
            GetComponent<Image>().color = new Color(0, 0, 0, 0);
            int meteoriteNumber = Random.Range(1, meteoritePrefabs.Length);
            explosion = Instantiate(explosion, transform.position, new Quaternion(), transform.parent);
            explosion.transform.localScale = transform.localScale;
            for (int i = 0; i <= meteoriteNumber; i++)
            {
                GameObject meteorite = Instantiate(meteoritePrefabs[i], transform.position, new Quaternion(), transform.parent);
                
                //add metorites to list to keep track of to destroy later
                meteorites.Add(meteorite);
                float x = Random.Range(-1f, 1f);
                float y = Random.Range(-1f, 1f);
                Vector2 direction = new Vector2(x, y) * meteoriteForce;
                meteorite.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
            }
            if (!fromRocket)
            {
                miniGameManager.TakeDamage();
                StartCoroutine(FlashDamageIndicator());
				while (flashingIndicator) { yield return null; }
            }
			else 
            {
                if(miniGameManager.requiredAstroids > 0)miniGameManager.requiredAstroids--;
                yield return new WaitForSeconds(1f); 
            }
            foreach (GameObject meteorite in meteorites) { Destroy(meteorite); }
            Destroy(this);
        }
	}

    IEnumerator FlashDamageIndicator()
	{
        flashingIndicator = true;
        miniGameManager.damageText.SetActive(true);
        AudioManager.instance.PlaySFX("Hull Damage");
        for (int i = 0; i <= 5; i++)
		{
            yield return new WaitForSeconds(0.08f);
            originalColor.a = i/10f;
            damageIndicator.color = originalColor;
		}
        for (int i = 4; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.08f);
            originalColor.a = i / 10f;
            damageIndicator.color = originalColor;
        }
        flashingIndicator = false;
        miniGameManager.damageText.SetActive(false);
    }
}