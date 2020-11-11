using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MovementDirective
{
    [SerializeField] MovementDirective[] movementDirectives = null;
    bool hasGivenSpeech = false;
    public int movementIndex = 0;
    protected King king;
    bool isChecking;
    readonly int waitTime = 30;

    //Called by king to connect it to its movementDirective and sub movementDirectives.
    public override void ConnectEnemyToMovement(Enemy enemy)
    {
        base.ConnectEnemyToMovement(enemy);
        foreach (MovementDirective subMovement in movementDirectives)
        {
            subMovement.ConnectEnemyToMovement(enemy);
        }
        king = enemy.GetComponent<King>();
    }

    //Chooses a sub MovementDirective
    public override void Move()
    {
        
        base.Move();
        if (!hasGivenSpeech) { movementIndex = movementDirectives.Length - 1; }
        phaseComplete = true;
        foreach (MovementDirective movement in movementDirectives)
        {
            if (!movement.PhaseComplete())
            {
                phaseComplete = false;
            }
        }
        if (hasGivenSpeech && phaseComplete){ ChooseDirective();}
        else if (hasGivenSpeech){movementDirectives[movementIndex].Move();}
        //Used for when player meets boss before given sword
        //Boss gives before sword speech and gives player a moment to leave before prompting again
        if (!hasGivenSpeech && !isChecking && playerWithInDetectRange)
        {
            movementDirectives[movementIndex].Move();
            StartCoroutine(Wait());
        }
    }


    IEnumerator Wait()
    {
        isChecking = true;
        yield return new WaitForSeconds(waitTime);
        isChecking = false;
    }

    void ChooseDirective()
    {
        if (phaseComplete)
        {
            movementIndex = Random.Range(0, movementDirectives.Length - 1);
            if (movementDirectives[movementIndex].CooledDown())
            {
                StartCoroutine(PhaseTimer(movementDirectives[movementIndex]));
            }
            else movementIndex = 0;
            animator.SetBool("IsSpining", false);
            animator.SetBool("IsShooting", false);
            animator.SetBool("IsSwinging", false);
        }
    }

    IEnumerator PhaseTimer(MovementDirective movement)
    {
        movement.StartPhase();
        for (float i = movement.GetPhaseTime(); i > 0; i--)
        {
            while (GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove()) yield return null;
            yield return new WaitForSeconds(1);
        }
        animator.SetBool("IsSpining", false);
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsSwinging", false);
        movement.EndPhase();
        StartCoroutine(movement.StartCoolDown());
        StopCharge();
    }


    public void ConcludeSpeech() { hasGivenSpeech = true; }

    public override void StopCharge() { }
}
