using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSprayer : Weapon
{
    [SerializeField] public Ammo ammo = null;
    [SerializeField] int fireRate = 1;
    [SerializeField] bool canShootUp;
    [SerializeField] bool canShootDown;

    public bool CanShootUp { get => canShootUp; set => canShootUp = value; }
    public bool CanShootDown { get => canShootDown; set => canShootDown = value; }
    public int FireRate { get => fireRate; set => fireRate = value; }
    public void SetAmmo(Ammo ammoIn) { ammo = ammoIn; }
    public Ammo GetAmmo() { return ammo; }

    public override void Fire(Transform target)
	{
		throw new System.NotImplementedException();
	}

    protected IEnumerator StartCooldown()
    {
        canFire = false;
        float wait = 1f / fireRate;
        yield return new WaitForSeconds(wait);
        canFire = true;
    }
}
