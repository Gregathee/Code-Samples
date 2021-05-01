using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveToPlayer : MoveToPlayerMovement
{
    [SerializeField] float swingDistance = 1f;
    float deltaX;
    float deltaY;
    Vector2 playerDirection;
    bool playerUp;
    bool playerRight;

    //Determines which direction to face and sets conditions for animator
    public override void Move()
    {
        base.Move();
        deltaX = rb.GetPointVelocity(enemyPos).x;
        deltaY = rb.GetPointVelocity(enemyPos).y;
        if (Mathf.Abs(deltaY) >= Mathf.Abs(deltaX))
        {
            if (deltaY > 0) ChangeFaceing(true, false, false, false);
            else ChangeFaceing(false, true, false, false);
        }
        else
        {
            if (deltaX > 0) ChangeFaceing(false, false, false, true);
            else ChangeFaceing(false, false, true, false);
        }
        animator.SetFloat("HorizontalDirection", deltaX);
        animator.SetFloat("VerticalDirection", deltaY);
        if (Vector2.Distance(enemyPos, playerPos) < swingDistance) 
        {
            playerDirection = enemyPos - playerPos;
            Swing();
        }
    }

    void Swing()
    {
        //is player left or right?
        if (playerDirection.x <= 0)
        {
            PlayerOnRight();
        }
        else
        {
            PlayerOnLeft(); 
        }
        SwingInDirection();
    }

    void PlayerOnRight()
    {
        playerRight = true;
        //is player up or down?
        if (playerDirection.y <= 0)
        {
            playerUp = true;
        }
        else
        {
            playerUp = false;
        }
    }

    void PlayerOnLeft()
    {
        playerRight = false;
        //is player up or down
        if (playerDirection.y <= 0)
        {
            playerUp = true;
        }
        else
        {
            playerUp = false;
        }
    }

    void SwingInDirection()
    {
        //is player more up or down than left or right?
        if (Mathf.Abs(playerDirection.x) >= Mathf.Abs(playerDirection.y))
        {
            //player is more right or left than up or down
            if (!playerRight) SwingSword(false, false, true, false);
            //player is more right or left than up or down
            if (playerRight) SwingSword(false, false, false, true);
        }
        else
        {
            //player is more up or down than right or left
            if (!playerUp) SwingSword(false, true, false, false);
            //player is more up or down than right or left
            if (playerUp) SwingSword(true, false, false, false);
        }
    }

    void SwingSword(bool up, bool down, bool left, bool right)
    {
        ChangeFaceing(up, down, left, right);
        enemy.SwingSword();
    }

    public void ChangeFaceing(bool up, bool down, bool left, bool right)
    {
        animator.SetBool("IsFacingUp", up);
        animator.SetBool("IsFacingDown", down);
        animator.SetBool("IsFacingLeft", left);
        animator.SetBool("IsFacingRight", right);
    }
}
