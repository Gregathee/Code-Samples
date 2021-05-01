using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class for MovementDirectives
public abstract class MovementDirective : MonoBehaviour
{
    [SerializeField] protected float timeTillAttack = 1;
    [SerializeField] protected float attackRange = 1;
    [SerializeField] protected float phaseTime = 10;
    [SerializeField] protected float phaseCoolDown = 10;
    protected Enemy enemy;
    protected Rigidbody2D rb;
    protected Transform enemyTransform;
    protected Transform playerTransform;
    protected Animator animator;
    protected new SpriteRenderer renderer = null;
    protected bool canWalk = true;
    public bool phaseComplete = true;
    protected Vector2 enemyPos;
    protected Vector2 playerPos;
    protected float speed = 1;
    protected float distanceToPlayer;
    protected bool playerWithInDetectRange = false;
    protected bool playerWithInAttackRange = false;
    protected bool isConnectedToEnemy = false;
    protected bool cooledDown = true;

    //Is called by enemy to connect it to its MovementDirective
    //Collects components from enemy
    public virtual void ConnectEnemyToMovement(Enemy enemy)
    {
        this.enemy = enemy;
        rb = enemy.GetComponent<Rigidbody2D>();
        enemyTransform = enemy.transform;
        animator = enemy.GetAnimator();
        renderer = enemy.GetComponent<SpriteRenderer>();
        isConnectedToEnemy = true;
    }

    //Calculates varables for Child classes
    public virtual void Move()
    {
        if (animator)
        {
            if(GameManager.gameManager.IsPaused()) { animator.enabled = false; }
            else { animator.enabled = true; }
            animator.speed = GameManager.gameManager.GetDifficultyModifier();
        }
        if (isConnectedToEnemy)
        {
            playerTransform = GameManager.player.transform;
            enemyPos = enemyTransform.position;
            playerPos = playerTransform.position;
            speed = enemy.GetModifiedMoveSpeed() * Time.fixedDeltaTime;
            distanceToPlayer = Vector2.Distance(enemyTransform.position, playerTransform.position);
            playerWithInDetectRange = distanceToPlayer <= enemy.GetPlayerDetectionRadius();
            playerWithInAttackRange = distanceToPlayer <= attackRange;
        }
    }

    //Used by bosses
    public bool PhaseComplete() { return phaseComplete; }
    public void StartPhase() { phaseComplete = false; }
    public void EndPhase() { phaseComplete = true; }
    public float GetPhaseTime() { return phaseTime; }
    public float GetCoolDown() { return phaseCoolDown; }
    public bool CooledDown() { return cooledDown; }

    public IEnumerator StartCoolDown()
    {
        cooledDown = false;
        for (float i = phaseCoolDown; i > 0; i--)
        {
            while (GameManager.gameManager.IsPaused() || !GameManager.gameManager.EntitiesCanMove()) yield return null;
            yield return new WaitForSeconds(1);
        }
        cooledDown = true;
    }

    //Used by charging enemies
    public abstract void StopCharge();



}
