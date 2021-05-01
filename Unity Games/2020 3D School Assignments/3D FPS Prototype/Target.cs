/* * (Greg Brandt) 
 * * (Assignment 5) 
 * * Manages target health
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;

    public void TakeDamage(float amount)
	{
		health -= amount;
		if(health <= 0) { StartCoroutine(DieAfterTime()); }
	}

	IEnumerator DieAfterTime()
	{
		yield return new WaitForSeconds(1f);
		Die();
	}

	void Die() { Destroy(gameObject); }
}
