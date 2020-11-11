using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Ammo
{
    [SerializeField] int speed =0;
    [SerializeField] int travelTime = 5;
    [SerializeField] int homing = 0;
    [SerializeField] int aoe = 0;
    public string devmessage;
    Transform enemy;
    int penatrated = 0;

    public int AOE { get => aoe; set => aoe = value; }
    public int Speed { get => speed; set => speed = value; }
    public int TravelTime { get => travelTime; set => travelTime = value; }
    public int Homing { get => homing; set => homing = value; }

    private void Start()
    {
        devmessage = "I am brand new";
        if (!isPreview) StartCoroutine(DestroyAfterTime());
    }

    protected override void Update()
    {
        base.Update();
        if (!isPreview) Travel();
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            penatrated++;
            enemy.Damage(Damage);
        }
    }

    public void SetTarget(Transform targetIn) {enemy = targetIn;}

    public float GetSpeed() { return Speed; }

    void Travel()
    {
        transform.localPosition += transform.forward * Speed * Time.deltaTime;
        
        if(penatrated >= Penatration) { Destroy(gameObject); }
        if ( enemy != null)
        {
            Vector3 targetPos = enemy.position - transform.position;
            Quaternion targetHorizontalRotation = Quaternion.LookRotation(targetPos);
            transform.rotation = Quaternion.RotateTowards
            (transform.rotation, targetHorizontalRotation, Time.fixedDeltaTime * Homing);
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(TravelTime);
        Destroy(gameObject);
    }
}
