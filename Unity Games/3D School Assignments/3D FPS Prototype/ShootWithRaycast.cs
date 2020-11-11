/* * (Greg Brandt) 
 * * (Assignment 5) 
 * * Inflicts damage and applies force to targets
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWithRaycast : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera cam;
	public ParticleSystem muzzleFlash;

	private void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}

	void Shoot()
	{
		muzzleFlash.Play();
		RaycastHit hitInfo;
		Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, range);
		
		Rigidbody rb = hitInfo.rigidbody;
		if(rb) rb.AddForceAtPosition((hitInfo.point - cam.transform.position) * damage, hitInfo.point, ForceMode.Impulse);
		Target target = hitInfo.transform.gameObject.GetComponent<Target>();
		if(target) { target.TakeDamage(damage);}
		
	}

}
