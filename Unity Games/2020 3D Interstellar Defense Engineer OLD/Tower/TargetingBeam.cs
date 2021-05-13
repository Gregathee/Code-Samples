using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingBeam : MonoBehaviour
{
    bool enemyDetected = false;
    GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
            enemyDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
            enemyDetected = false;
        }
    }

    public bool EnemyDetected(ref GameObject aimPrevew) 
    {
        if (aimPrevew == target && enemyDetected) return true;
        else return false; 
    }
}

  
 

