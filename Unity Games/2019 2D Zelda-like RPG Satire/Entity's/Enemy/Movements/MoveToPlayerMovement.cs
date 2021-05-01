using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Detects player and moves towards
public class MoveToPlayerMovement : MovementDirective
{
    public override void Move()
    {
        base.Move();
        if (playerWithInDetectRange)
        {
            var pushDirection = (playerPos - enemyPos).normalized;
            rb.AddForce(pushDirection * speed, ForceMode2D.Impulse);
            animator.SetBool("CanMove", true);
        }
        else animator.SetBool("CanMove", false);
    }

    private void Update()
    {
        
        King king = enemy.GetComponent<King>();
        if((GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove()) && king == null)
        {
            animator.SetBool("InRange", false);
        }
        else if(king == null)
        {
            animator.SetBool("InRange", true);
        }
    }

    //Intentially blank
    public override void StopCharge() { }
}
