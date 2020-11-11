using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IQuestCompleter, IQuestGiver
{
    [SerializeField] protected MovementDirective movement = null;
    [SerializeField] float PlayerDetectionRadius = 1.5f;
    [SerializeField] protected HeartReplenisher heartReplenisher = null;
    [SerializeField] bool dontKillIfTooFar = false;
    [SerializeField] QuestGiver questGiver;
    [SerializeField] bool split = false;
    [SerializeField] bool splitChild = false;
    [SerializeField] float splitProtectionTime = 0.25f;
    Transform playerTransform = null;
    Collider2D attackerCollider;
    [HideInInspector]new public SpriteRenderer renderer = null;
    protected float distanceFromPlayer;
    float modifiedMoveSpeed = 0f;
    int senderID = 0;
    public bool isCharging = false;
    
    protected new void Start()
    {
        base.Start();
        movement.ConnectEnemyToMovement(this);
        renderer = GetComponent<SpriteRenderer>();
        
        senderID = Random.Range(0, 1000);
        if (splitChild)
        {
            invulnerabilityMod.MakeInvulnerable(ref splitProtectionTime);
            Stun();
        }
        int level = GameManager.gameManager.GetDifficultyLevel();
        if(level == 1) 
        {
            hitPoints--;
            maxHitPoints--;
        }
        if(level == 3)
        {
            maxHitPoints++;
            hitPoints++;
        }
        if(level == 4)
        {
            maxHitPoints++;
            hitPoints++;
        }
    }

    protected virtual void Update()
    {
        playerTransform = GameManager.player.transform;
        modifiedMoveSpeed = GameManager.gameManager.GetDifficultyModifier() * moveSpeed;
        
        distanceFromPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceFromPlayer > GameManager.gameManager.GetEnemyDispawnDistance() && !dontKillIfTooFar) Destroy(gameObject);
        //Destroy all non-unique enemies upon player death
        if (GameManager.gameManager.PlayerIsDead() && !dontKillIfTooFar) Destroy(gameObject);
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        { 
            PushBack(ref collision);
            movement.StopCharge();
            Stun();
        }
        if(isCharging && collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<EnemyTriggerRadius>().GetEnemy();
            if(enemy)if(enemy.GetSenderID() != senderID)
            {
                enemy.InflictDamage(attackDamage, GetComponentInChildren<Collider2D>());
            }
        }
    }

    //Used by MovementDirective
    public float GetPlayerDetectionRadius() { return PlayerDetectionRadius; }
    public float GetModifiedMoveSpeed() { return modifiedMoveSpeed; }
    public int GetSenderID() { return senderID; }
    public Animator GetAnimator() { return animator; }

    

    //Enacts the movement behavior of the attached MovementDirective
    protected virtual void Move()
    {
        if (!isStunned && GameManager.gameManager.EntitiesCanMove()){ movement.Move();}
    }

    //Calls GameManger to check to see if enemy's death completes a quest
    protected override void Die()
    {
        QuestManager.questManager.CompleteQuest(this);
        QuestManager.questManager.GetQuestFromQuestGiver(this);
        if (split) GetComponent<SplitOnDeath>().Split(attackerCollider);
        StartCoroutine(DestroyAfterSeconds(0.1f)); 
    }

    //Used to ensure all operations with references to enemy complete 
    IEnumerator DestroyAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        DropHeart();
        Destroy(gameObject);
    }

    protected virtual void DropHeart()
    {
        int dropChance = Random.Range(0, 100);
        if (dropChance < GameManager.gameManager.GetHeartDropRate()) 
            Instantiate(heartReplenisher, transform.position, new Quaternion());
    }


    public override void InflictDamage(int damage, Collider2D colliderOfAttacker)
    {
        attackerCollider = colliderOfAttacker;
        string aName = colliderOfAttacker.name;
        bool vulnerable = !invulnerabilityMod.isInvulerable;
        GameManager.gameManager.IncreaseDifficulty();
        Enemy enemy = null;
        if (colliderOfAttacker.CompareTag("Enemy")) { enemy = colliderOfAttacker.GetComponent<EnemyTriggerRadius>().GetEnemy(); }
        if (enemy)
        {
            if (enemy.isCharging) { base.InflictDamage(damage, colliderOfAttacker, true); }
            else { base.InflictDamage(damage, colliderOfAttacker); }
        }
        else { base.InflictDamage(damage, colliderOfAttacker); }
    }

    public override void DamageEnemy(ref Collider2D collision)
    {
        Player enemy = collision.gameObject.GetComponent<Player>();
        if (enemy != null)
        {
            enemy.InflictDamage(attackDamage, GetComponentInChildren<Collider2D>());
        }
    }

    //IQuestCompleter/IQuestGiver methods
    public string GetQuestCompleterName() { return gameObject.name; }
    public Quest GiveQuest() { return questGiver.GiveQuest(); }
    public int GetQuestNumber() { return questGiver.GetQuestNumber(); }
    public int GetQuestPrereqNumber() { return questGiver.GetQuestPrereqNumber(); }
    public bool HasQuest() { return questGiver.HasQuest(); }
    public void IncrementQuestIndex() { questGiver.IncrementQuestIndex(); }
    public string GetName(){return name;}
}
