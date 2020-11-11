using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object that follows tip of sword and damages entitys
public class SwordTip : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] string attackableTag = null;
    [SerializeField] Entity swordWielder = null;
    public bool canDamage = false;

    //Target is the location of the sword tip and is inactive when not swinging
    private void Update()
    {
        if (target != null)
        {
            if (target.gameObject.activeInHierarchy)
            {
                transform.position = target.position;
                canDamage = true;
            }
            else{ canDamage = false; }
        }
    }

    //Damage entity
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag(attackableTag) || collision.CompareTag("Enemy")) 
        {
            if ( canDamage){ swordWielder.DamageEnemy(ref collision);}
        }
    }

}
