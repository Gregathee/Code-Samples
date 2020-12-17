/*
 * Rocket.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: Script to propel astroid mini game rocket and shrink scale to apear going forward in 3D space
 */

using System.Collections;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Tooltip("Speed that projectile moves upward relative to it self.")]
    [SerializeField] float upwardSpeed;
    [Tooltip("Speed that projectile shrinks to apear as if going forward in 3D space")]
    [SerializeField] float scaleSpeed;
    [Tooltip("SFX for breaking astroids")]
    public string[] breakAsteroid;

    void Start() { StartCoroutine(DecreaseScale()); }

    void Update()
    {
        float scale = transform.localScale.x - scaleSpeed * Time.deltaTime;
        if (scale <= 0) { Destroy(gameObject); }
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position += (transform.up).normalized * upwardSpeed * Time.deltaTime; ;
    }

    void OnTriggerEnter2D(Collider2D other)
	{
        Astroid astroid = other.GetComponent<Astroid>();
		if (astroid) 
        {
            AudioManager.instance.PlaySFX(breakAsteroid[Random.Range(0, breakAsteroid.Length - 1)]);
            astroid.StartExploding();
            Destroy(gameObject);
        }
	}

    IEnumerator DecreaseScale()
    {
        float originalScaleSpeed = scaleSpeed;
        while (transform.localScale.x >= 0)
        {
            yield return new WaitForSeconds(0.5f);
            scaleSpeed += originalScaleSpeed;
            upwardSpeed -= upwardSpeed / 2;
        }
    }
}