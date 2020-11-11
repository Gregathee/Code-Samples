using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProjectile : Weapon
{
    [SerializeField] public Ammo ammo = null;
    [SerializeField] GameObject barrel;
    [SerializeField] int fireRate = 1;
    [SerializeField] Recoil recoil = Recoil.Level1;
    [SerializeField] Accuracy accuracy = Accuracy.Level1;
    [SerializeField] int rotationSpeed = 1;
    [SerializeField] bool canShootUp;
    [SerializeField] bool canShootDown;
    float accuracyIncrements = 2.5f;
    float recoilIncrements = 2.5f;
    float fRecoil = 0;
    float fAccuracy = 0;

    public Recoil Recoil { get => recoil; set => recoil = value; }
    public  Accuracy Accuracy { get => accuracy; set => accuracy = value; }
    public int RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public bool CanShootUp { get => canShootUp; set => canShootUp = value; }
    public bool CanShootDown { get => canShootDown; set => canShootDown = value; }
    public int FireRate { get => fireRate; set => fireRate = value; }
    public void SetAmmo(Ammo ammoIn) { ammo = ammoIn; }
    public Ammo GetAmmo() { return ammo; }

    public ref GameObject GetBarrel() { return ref barrel; }

    private void Start()
    {
        fAccuracy = ((float)accuracy) * accuracyIncrements;
        fRecoil = ((float)Recoil) * recoilIncrements;
    }

    public override void Fire(Transform target)
    {
        if (canFire)
        {
            float y = Random.Range(0, fAccuracy);
            //add verticle deviation
            int right = Random.Range(0, 2);
            if (right == 1) y *= -1;
            Vector3 startingVRotation = new Vector3(barrel.transform.eulerAngles.x, barrel.transform.eulerAngles.y + y, barrel.transform.eulerAngles.z);
            Quaternion startingQRotation = Quaternion.Euler(startingVRotation);
            //Instantiate(ammo, firePoint.position, startingQRotation).SetTarget(target);
            Debug.Log("Fix Fire Method");
            RecoilBarrel();
            StartCoroutine(StartCooldown());
        }
    }

    void RecoilBarrel()
    {
        Vector3 startingVRotation = new Vector3(barrel.transform.eulerAngles.x - fRecoil, 0, 0);
        Quaternion startingQRotation = Quaternion.Euler(startingVRotation);
        barrel.transform.localRotation = startingQRotation;
    }

    protected IEnumerator StartCooldown()
    {
        canFire = false;
        float wait = 1f / fireRate;
        yield return new WaitForSeconds(wait);
        canFire = true;
    }
}
