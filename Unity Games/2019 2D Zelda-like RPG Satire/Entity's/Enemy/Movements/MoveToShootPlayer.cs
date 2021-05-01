using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Detects player, moves towards until within attack range, pauses then instantiates projectile
public class MoveToShootPlayer : MovementDirective 
{
    [SerializeField] Projectile projectile = null;
    [SerializeField] Color colorChange = new Vector4(0,0,0,1);
    float deltaX;
    float deltaY;
    bool isShooting = false;

    public override void Move()
    {
        base.Move();
        if (playerWithInDetectRange && !playerWithInAttackRange && canWalk && !GameManager.gameManager.IsPaused())
        {
            var pushDirection = (playerPos - enemyPos).normalized;
            rb.AddForce(pushDirection * speed, ForceMode2D.Impulse);
            animator.SetBool("CanMove", true);
        }
        else animator.SetBool("CanMove", false);
        if (playerWithInAttackRange && !GameManager.gameManager.IsPaused() && !isShooting) StartCoroutine(ShootPlayer());
        deltaX = rb.GetPointVelocity(enemyPos).x;
        deltaY = rb.GetPointVelocity(enemyPos).y;
        if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
        {
            if (deltaY > 0) ChangeFaceing(true, false, false, false);
            else ChangeFaceing(false, true, false, false);
        }
        else if (Mathf.Abs(deltaY) < Mathf.Abs(deltaX))
        {
            if (deltaX > 0) ChangeFaceing(false, false, false, true);
            else ChangeFaceing(false, false, true, false);
        }
    }
    Vector2 playerDirection;

    bool playerUp;
    bool playerRight;
    void DetectFacing()
    {
        playerDirection = enemyPos - playerPos;
        //is player left or right?
        if (playerDirection.x <= 0)
        {
            PlayerOnRight();
        }
        else
        {
            PlayerOnLeft();
        }
        //is player more up or down than left or right?
        if (Mathf.Abs(playerDirection.x) >= Mathf.Abs(playerDirection.y))
        {
            //player is more right or left than up or down
            if (!playerRight) ChangeFaceing(false, false, true, false);
            //player is more right or left than up or down
            if (playerRight) ChangeFaceing(false, false, false, true);
        }
        else
        {
            //player is more up or down than right or left
            if (!playerUp) ChangeFaceing(false, true, false, false);
            //player is more up or down than right or left
            if (playerUp) ChangeFaceing(true, false, false, false);
        }
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

    

    //Intentially blank
    public override void StopCharge() { }

    public void ChangeFaceing(bool up, bool down, bool left, bool right)
    {
        animator.SetBool("IsFacingUp", up);
        animator.SetBool("IsFacingDown", down);
        animator.SetBool("IsFacingLeft", left);
        animator.SetBool("IsFacingRight", right);
    }

    //Pauses, changes colors, instantiates projectile
    IEnumerator ShootPlayer() 
    {
        isShooting = true;
        renderer = enemy.GetComponentInChildren<SpriteRenderer>();
        if (!enemy.IsStunned())
        {
            Color originalColor = renderer.color;
            canWalk = false;
            renderer.color = colorChange;
            yield return new WaitForSeconds(timeTillAttack / 3);
            renderer.color = originalColor;
            if (!enemy.IsStunned())
            {
                while (GameManager.gameManager.IsPaused()) { yield return null; }
                if(!enemy.IsStunned())yield return new WaitForSeconds(timeTillAttack / 3);
                renderer.color = colorChange;
                DetectFacing();
                if (!enemy.IsStunned()) while (GameManager.gameManager.IsPaused()) { yield return null; }
                yield return new WaitForSeconds(timeTillAttack / 3);
                renderer.color = originalColor;
                DetectFacing();
                if (!enemy.IsStunned()) while (GameManager.gameManager.IsPaused()) { yield return null; }
                if (!enemy.IsStunned())
                {
                    Projectile tempProjectile;
                    tempProjectile = Instantiate(projectile, enemyPos, new Quaternion());
                    tempProjectile.ConnectEnemySender(enemy);
                }
            }
            canWalk = true;
        }
        isShooting = false;
    }
}
