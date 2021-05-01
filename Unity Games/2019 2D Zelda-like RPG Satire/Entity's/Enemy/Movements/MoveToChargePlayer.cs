using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Detects player and moves towards until in charge distance then charges
public class MoveToChargePlayer : MovementDirective
{
    [SerializeField] float speedMultiplyer = 2;
    [SerializeField] Color colorChange = new Vector4(0, 0, 0, 1);
    Vector2 playersLastPosition;
    bool isCharging = false;

    private void Update() 
    { 
        if (enemy.IsStunned()) isCharging = false;
        if (isCharging) enemy.isCharging = true; 
        else enemy.isCharging = false;
    }

    public override void Move()
    {
        base.Move();
        var pushDirection = (playerPos - enemyPos).normalized;
        //Walk to player
        if (playerWithInDetectRange && !playerWithInAttackRange && canWalk && !isCharging)
        {
            rb.AddForce(pushDirection * speed, ForceMode2D.Impulse);
        }
        //Enable charging
        if (playerWithInAttackRange && canWalk && !isCharging) StartCoroutine(ChargePlayer());
        //Charge to player
        if (isCharging)
        {
            pushDirection = (playersLastPosition - enemyPos).normalized;
            float force = speed * speedMultiplyer;
            rb.AddForce(pushDirection * force, ForceMode2D.Impulse);
            if (Vector2.Distance(enemyPos, playersLastPosition) < 0.1f) isCharging = false;
        }
    }

    //Called by Enemy upon collition with player
    public override void StopCharge() { isCharging = false; }

    //Stops, changes colors to indicate to player that it's about to charge
    IEnumerator ChargePlayer()
    {
        if (!enemy.IsStunned())
        {
            Color originalColor = renderer.color;
            canWalk = false;
            renderer.color = colorChange;
            yield return new WaitForSeconds(timeTillAttack / 3);
            renderer.color = originalColor;
            if (!enemy.IsStunned())
            {
                yield return new WaitForSeconds(timeTillAttack / 3);
                if (!enemy.IsStunned())
                {
                    renderer.color = colorChange;
                    yield return new WaitForSeconds(timeTillAttack / 3);
                    renderer.color = originalColor;
                }
                else
                {
                    isCharging = false;
                }
                if (!enemy.IsStunned())
                {
                    playersLastPosition = playerTransform.position;
                    isCharging = true;
                    StartCoroutine(ChargeTimer());
                }
                else
                {
                    isCharging = false;
                }
            }
            canWalk = true;
        }
    }

    IEnumerator ChargeTimer()
    {
        yield return new WaitForSeconds(5);
        isCharging = false;
    }
}
