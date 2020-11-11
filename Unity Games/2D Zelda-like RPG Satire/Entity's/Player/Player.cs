using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] AttackAOE upAtkAOE = null;
    [SerializeField] AttackAOE downAtkAOE = null;
    [SerializeField] AttackAOE rightAtkAOE = null;
    [SerializeField] AttackAOE leftAtkAOE = null;
    [SerializeField] LifeBar lifeBar = null;
    [SerializeField] GameObject startObject;
    [SerializeField] float playerStunTime = 0.25f;
    [SerializeField] float swordInvulnerablityTime = 0.1f;
    [SerializeField] float frenzySpeed = 2;
    float verticalInput = 0;
    float horizontalInput = 0;
    public bool dashCooledDown = true;
    AttackAOE currentAtkAOE = null;
    RespawnPoint respawnPoint;
    PlayerFacing facing = PlayerFacing.Down;
    float originalSpeed;
    
    private new void Start()
    {
        base.Start();
        GameManager.gameManager.animator = animator;
        originalSpeed = moveSpeed;
    }


    private void Update() 
    {
        if (!isSwingingSword && GameManager.gameManager.EntitiesCanMove() && !isStunned &&
            !startObject.activeInHierarchy) 
        { Move(); CheckNPC(); Attack();  }
        //Prevent animator from walking while stunned
        if (isStunned) 
        {
            animator.SetFloat("HorizontalDirection", 0);
            animator.SetFloat("VerticalDirection", 0);
        }
        if (!isSwingingSword)
        {
            pushable = true;
        }
        if(isSwingingSword && pushable) 
        { 
            invulnerabilityMod.MakeInvulnerable(ref swordInvulnerablityTime, false);
            pushable = false;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) { CheckEnemyOnCollision(collider); }
    public PlayerFacing GetPlayerFacing() { return facing; }
    public void SetRespawnPoint(RespawnPoint respawn) { respawnPoint = respawn; }

    protected override IEnumerator StunTimer()
    {
        isStunned = true;
        yield return new WaitForSeconds(playerStunTime);
        isStunned = false;
    }

    public void UpdateLifeBar()
    {
        int numberOfHearts = lifeBar.GetNumberOfHearts();
        int numberOfFullHearts = lifeBar.GetNumberOfFullHearts();
        if(numberOfFullHearts != hitPoints)
        {
            if(numberOfFullHearts < hitPoints){ lifeBar.Heal(hitPoints - numberOfFullHearts); }
            else { lifeBar.TakeDamage(numberOfFullHearts - hitPoints); }
        }
        if(numberOfHearts != maxHitPoints) 
        {
            if (numberOfHearts < maxHitPoints) { lifeBar.AddHeart(maxHitPoints - numberOfHearts);}
            else { lifeBar.AddHeart(numberOfHearts - maxHitPoints); }
        }
    }

    public void ReplenishHeart(ref int replenishAmount)
    {
        Heal(replenishAmount);
        UpdateLifeBar();
    }

    

    void Move()
    {
        GameManager game = GameManager.gameManager;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        animator.SetFloat("HorizontalDirection", horizontalInput);
        animator.SetFloat("VerticalDirection", verticalInput);

        //normalize diagnonal move speed
        if ((verticalInput != 0) && horizontalInput != 0)
        {
            verticalInput *= 0.7f;
            horizontalInput *= 0.7f;
        }
        float horizontalVelocity = Mathf.Lerp(0, horizontalInput * moveSpeed, 0.8f);
        float verticalVelocity = Mathf.Lerp(0, verticalInput * moveSpeed, 0.8f);
        rigbody.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        ChangePlayerFacing(ref verticalInput, ref horizontalInput);
    }

    void CheckNPC()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.gameManager.IsPaused()) { currentAtkAOE.ActivateNPC(); }
        else if (GameManager.gameManager.MobileSwing && !GameManager.gameManager.IsPaused()) currentAtkAOE.ActivateNPC();
    }

    void Attack()
    {
        bool canSwingSword = GameManager.gameManager.CanSwingSword();
        if( !GameManager.gameManager.NPCIsTalking() && canSwingSword && !GameManager.gameManager.IsPaused())
        {
            if (Input.GetKeyDown(KeyCode.Space) || GameManager.gameManager.MobileSwing)SwingSword();
            if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || GameManager.gameManager.MobileDash) && dashCooledDown)
            {
                Dash();
            }
        }
    }

    void Dash()
    {
        float dashTime = 0.5f;
        invulnerabilityMod.MakeInvulnerable(ref dashTime);
        GetComponent<Rigidbody2D>().AddForce(rigbody.velocity * pushBackForce * 2, ForceMode2D.Impulse);
        SwingSword();
        StartCoroutine(DashCoolDown());
    }

    bool attackFrenzy = false;
    [SerializeField] int frenzyTime = 3;
    int frenzyIndex;
    public IEnumerator AttackFrenzy()
    {
        if (!attackFrenzy)
        {
            frenzyIndex = frenzyTime;
            attackFrenzy = true;
            moveSpeed = frenzySpeed;
            while (frenzyIndex > 0)
            {
                yield return new WaitForSeconds(1);
                frenzyIndex--;
            }
            attackFrenzy = false;
            moveSpeed = originalSpeed;
        }
        else if(!invulnerabilityMod.isInvulerable) { frenzyIndex += frenzyTime; }  
        
    }
    

    IEnumerator DashCoolDown()
    {
        dashCooledDown = false;
        for(int i = 1; i < 30; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GetComponentInChildren<SpriteRenderer>().color = new Vector4(((float)i  /60f) +0.5f, ((float)i /60f) + 0.5f, 1, 1);
        }
        dashCooledDown = true;
    }

    public override void DamageEnemy(ref Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<EnemyTriggerRadius>().GetEnemy();
        if (enemy != null)
        {
            enemy.InflictDamage(attackDamage, GetComponent<Collider2D>());
            StartCoroutine(AttackFrenzy());
        }
    }

    void CheckEnemyOnCollision(Collider2D collider)
    {
        bool vulnerable = !invulnerabilityMod.isInvulerable;
        if (collider.tag == "Enemy" && vulnerable && collider.name != "KingTrigger")
        {
            Enemy entity = collider.GetComponent<EnemyTriggerRadius>().GetEnemy();
            InflictDamage(entity.GetAttackDamage(), collider);
        }
    }

    public override void InflictDamage(int damage, Collider2D colliderOfAttacker)
    {
        Enemy enemy = null;
        EnemyTriggerRadius trigger = colliderOfAttacker.GetComponent<EnemyTriggerRadius>();
        if (trigger) enemy = trigger.GetEnemy();
        if (enemy)
        {
            GameManager.gameManager.DecreaseDifficulty();
            if (enemy.isCharging){base.InflictDamage(damage, colliderOfAttacker, true);}
            else { base.InflictDamage(damage, colliderOfAttacker); }
        }
        else { base.InflictDamage(damage, colliderOfAttacker); }
        
        UpdateLifeBar();
    }
    

    protected override void Die()
    {
        StartCoroutine(GameManager.gameManager.ShowDeathScreen());
        GameManager.gameManager.TeleportPlayer(respawnPoint.transform);
        StartCoroutine(KeepPosition());
        Heal(maxHitPoints);
        DataMiner.dataMiner.AddDeathToLog();
    }

    public void ChangePlayerFacing(PlayerFacing playerFacing) 
    {
        facing = playerFacing;
        switch(playerFacing)
        {
            case PlayerFacing.Up:
                SetActiveAOE(true, false, false, false);
                break;
            case PlayerFacing.Down:
                SetActiveAOE(false, true, false, false);
                break;
            case PlayerFacing.Left:
                SetActiveAOE(false, false, true, false);
                break;
            case PlayerFacing.Right:
                SetActiveAOE(false, false, false, true);
                break;
            default:
                Debug.Log("Case not covered");
                break;
        }
    }

    void ChangePlayerFacing(ref float verticalInput, ref float horizontalInput)
    {
        if (horizontalInput > 0) { SetActiveAOE(false, false, false, true); }
        else if (horizontalInput < 0) { SetActiveAOE(false, false, true, false); }
        else if (verticalInput > 0) { SetActiveAOE(true, false, false, false); }
        else if (verticalInput < 0) { SetActiveAOE(false, true, false, false); }
    }

    public void SetActiveAOE(bool up, bool down, bool left, bool right)
    {
        if (!GameManager.gameManager.NPCIsTalking())
        {
            bool tripTrue = false;
            bool[] boolArray = new bool[] { up, down, left, right };
            int falseCount = 0;
            for (int i = 0; i < boolArray.Length; i++)
            {
                if (tripTrue && boolArray[i])
                {
                    Debug.Log("Cannot assign multiple facings");
                    return;
                }
                if (boolArray[i]) tripTrue = true;
                else falseCount++;
            }
            if (falseCount != 3)
            {
                Debug.Log("Must assign facing");
                return;
            }
            animator = GetComponentInChildren<Animator>();
            StartCoroutine(ActivateAOE( up,  down,  left,  right));
            if (up) { currentAtkAOE = upAtkAOE; facing = PlayerFacing.Up; }
            else if (down) { currentAtkAOE = downAtkAOE; facing = PlayerFacing.Down; }
            else if (left) { currentAtkAOE = leftAtkAOE; facing = PlayerFacing.Left; }
            else if (right) { currentAtkAOE = rightAtkAOE; facing = PlayerFacing.Right; }
        }
    }

    IEnumerator ActivateAOE(bool up, bool down, bool left, bool right)
    {
        yield return new WaitForSeconds(0.01f);
        upAtkAOE.gameObject.SetActive(up);
        animator.SetBool("IsFacingUp", up);
        downAtkAOE.gameObject.SetActive(down);
        animator.SetBool("IsFacingDown", down);
        leftAtkAOE.gameObject.SetActive(left);
        animator.SetBool("IsFacingLeft", left);
        rightAtkAOE.gameObject.SetActive(right);
        animator.SetBool("IsFacingRight", right);
    }

    //Keep player on respawn point 
    IEnumerator KeepPosition()
    {
        yield return new WaitForSeconds(0.3f);
        GameManager.gameManager.TeleportPlayer(respawnPoint.transform);
    }
}

public enum PlayerFacing { Up, Down, Left, Right}