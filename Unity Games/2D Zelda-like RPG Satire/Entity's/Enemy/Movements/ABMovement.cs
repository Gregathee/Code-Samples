using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Move from point A to point B horizontally or vertically
public class ABMovement : MovementDirective
{
    [SerializeField] Transform pointA = null;
    [SerializeField] Transform pointB = null;
    [SerializeField] bool horizontal;
    bool isWalkingToA = false;
    Vector2 posA;
    Vector2 posB;
    bool time = true;

    private void Start()
    {
        posA = pointA.position;
        posB = pointB.position;
    }

    private void Update()
    {
        if (GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove())
        {
            animator.SetBool("CanMove", false);
        }
        else
        {
            animator.SetBool("CanMove", true);
        }
    }

    public override void Move()
    {
        Vector2 pushDirection;
        base.Move();
        if (isWalkingToA) 
        {
            pushDirection = (posA - enemyPos).normalized;
            if(horizontal) animator.SetBool("IsWalkingLeft", true);
            else animator.SetBool("IsWalkingUp", true);
        }
        else 
        { 
            pushDirection = (posB - enemyPos).normalized;
            if (horizontal) animator.SetBool("IsWalkingLeft", false);
            else animator.SetBool("IsWalkingUp", false);
        }
        rb.AddForce(pushDirection * speed, ForceMode2D.Impulse);
        if(time)
        {
            StartCoroutine( Wait());
        }
    }

    //Run in a direction for a time if entitys can move then change direction
    IEnumerator Wait()
    {
        time = false;
        for(int i = 0; i < 10; i++)
        {
            while (GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove())
            { yield return null; }
            yield return new WaitForSeconds(0.1f);
            while (GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove())
            { yield return null; }
        }
        
        if (isWalkingToA)
        {
            isWalkingToA = false;
        }
        else
        {
            isWalkingToA = true;
        }
        time = true;
    }

    public override void StopCharge() { }
}
