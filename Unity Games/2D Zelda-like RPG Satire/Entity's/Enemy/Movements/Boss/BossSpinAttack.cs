using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpinAttack : MoveToPlayerMovement
{
    bool hasLoopedOnce = false;
    float originalSpeed;
    [SerializeField]float speedSlowModifier = 1.5f;


    public override void Move()
    {
        //Gets bosses original base movement speed so it can be changed and reverted back to normal
        if(!hasLoopedOnce)
        {
            originalSpeed = enemy.moveSpeed;
            hasLoopedOnce = true;
        }
        enemy.StopSwimingSword();
        base.Move();
        if (!enemy.GetAnimator().GetBool("IsSpining"))
        {
            enemy.GetAnimator().SetBool("IsSpining", true);
        }
        else { GetComponentInParent<BossMovement>().movementIndex = 0;}
        //While spining movement speed is slowed
        if(animator.GetBool("IsSpining")) { enemy.moveSpeed = originalSpeed / speedSlowModifier; }
        if(!animator.GetBool("IsSpining")) { enemy.moveSpeed = originalSpeed; }
    }
}
