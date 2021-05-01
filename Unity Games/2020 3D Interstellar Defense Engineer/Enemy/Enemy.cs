using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour, IComparable<Enemy>
{
    [SerializeField] HealthBar healthBar = null;
    [SerializeField] int startHitPoints = 1;
    [SerializeField] float speed = 1;
    [SerializeField] float timeTillNextSpawn;
    [SerializeField] float physicalResistance = 0;
    [SerializeField] float fireResistance =0;
    [SerializeField] float iceResistance = 0;
    [SerializeField] float lightningResistance = 0;
    [SerializeField] float poisonResistance = 0;
    [SerializeField] float pierceResistance = 0;
    public bool stealthed = false;
    public bool flying = false;
    Transform[] path = null;
    Vector3 startPos = Vector3.zero;
    int pathIndex = 0;
    int currentHitPoints = 0;
    int position = 0;
    float distanceToGo = 0;
    bool pathIsAssigned = false;
    bool isDead = false;
    public bool debug = false;


    private void Start()
    {
        currentHitPoints = startHitPoints;
        startPos = transform.position;
        if(debug) 
        {
            speed = UnityEngine.Random.Range(10f,30f); 
            float scale = UnityEngine.Random.Range(1f, 5f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
    void Update()
    {
        if (pathIsAssigned) { Move(); }
        DistanceTraveled();
        name = position.ToString();
        if(debug) { currentHitPoints = 1000000; }
    }

    public int GetCurrentHealth() { return currentHitPoints; }

    public float GetNextSpawnTime() { return timeTillNextSpawn; }

    public bool IsStealthed() { return stealthed; }

    public float GetResistance(WeaknessPriority weakness)
    {
        switch(weakness)
        {
            case WeaknessPriority.Physical: return physicalResistance;
            case WeaknessPriority.Fire: return fireResistance;
            case WeaknessPriority.Ice: return iceResistance;
            case WeaknessPriority.Lightning: return lightningResistance;
            case WeaknessPriority.Poison: return poisonResistance;
            case WeaknessPriority.Piercing: return pierceResistance;
            case WeaknessPriority.Stealth: if (stealthed) return 0; else return 1;
            case WeaknessPriority.Flying: if (flying) return 0; else return 1;
            case WeaknessPriority.Ground: if (flying) return 1; else return 0;
            default: return 100;
        }
    }

    public void Move()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        var pushDirection = (path[pathIndex].position - transform.position).normalized;
        rb.velocity = pushDirection * speed;
        float distance = Vector3.Distance(transform.position, path[pathIndex].position);
        if ((distance > -1f && distance < 1f) && pathIndex < path.Length -1)
        {
            rb.velocity = Vector3.zero;
            if (pathIndex == 0)
            {
                distanceToGo -= Vector3.Distance(transform.position, startPos);
            }
            else
            {
                distanceToGo -= Vector3.Distance(transform.position, path[pathIndex - 1].position);
            }
            pathIndex++;
        }
        else if (pathIndex == path.Length-1) 
        {
            if (debug)
            {
                Die();
            }
        }
    }
    
    public void SetSpeed(float newSpeed) { speed = newSpeed; }

    public void SetSize(float newSize) { transform.localScale = new Vector3(newSize, newSize, newSize); }

    public void AssignPath(Path path) 
    {
        this.path = path.GetPath();
        distanceToGo = path.GetPathLength();
        pathIsAssigned = true;
    }

    public void Damage(int damage) 
    {
        currentHitPoints -= damage;
        if(currentHitPoints <= 0 && !debug) { Die(); }
        healthBar.SetStatus((float)((float)currentHitPoints / (float)startHitPoints));
    }

    public float DistanceTraveled()
    {
        if (this)
        {
            if (pathIndex == 0)
            {
                return distanceToGo = Vector3.Distance(transform.position, startPos);
            }
            else
            {
                return distanceToGo - Vector3.Distance(transform.position, path[pathIndex - 1].position) + distanceToGo;
            }
        }
        else { return 0; }
    }

    //public int GetPosition() { return position; }
    public void SetPosition(int pos) { position = pos; }

    void Die() { isDead = true; }

    public bool IsDead() { return isDead; }

    public int CompareTo(Enemy other)
    {
        if (other)
        {
            this.DistanceTraveled();
            other.DistanceTraveled();
            int result = DistanceTraveled().CompareTo(other.DistanceTraveled());
            
            if (result == 0) { result = 1; }
            return result;
        }
        else { Debug.Log("Ger"); return 1; }
    }
}
