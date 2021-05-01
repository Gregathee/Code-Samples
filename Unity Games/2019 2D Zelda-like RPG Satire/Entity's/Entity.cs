using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class for Player and Enemy
public abstract class Entity : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    [SerializeField] protected int hitPoints;
    [SerializeField] protected int maxHitPoints;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float invulnerabilityTimeAfterDmg = 1f;
    [SerializeField] protected bool pushable = true;
    [SerializeField] protected bool isStunnable = true;
    [SerializeField] protected int stunTime = 1;
    protected Animator animator;
    protected InvulnerabilityMod invulnerabilityMod;
    protected Rigidbody2D rigbody;
    [SerializeField]protected float pushBackForce = 5;
    protected bool isDead;
    protected bool isStunned;
    protected bool isSwingingSword;
    protected bool hasFinishedSwinging;

    protected void Start()
    {
        invulnerabilityMod = GetComponent<InvulnerabilityMod>();
        animator = GetComponentInChildren<Animator>();
        rigbody = GetComponent<Rigidbody2D>();
    }

    public int GetHitPoints() { return hitPoints; }
    public int GetMaxHitPoints() { return maxHitPoints; }
    public int GetAttackDamage() { return attackDamage; }
    public Animator GetAnimator() { return animator; }
    public bool IsStunned() { return isStunned; }
    public abstract void DamageEnemy(ref Collider2D collision);
    public void Heal(int healAmount) 
    {
        hitPoints += healAmount; 
        if(hitPoints > maxHitPoints) { hitPoints = maxHitPoints; }
    }

    public bool IsFullHealth()
    {
        if (hitPoints > maxHitPoints) hitPoints = maxHitPoints;
        if (hitPoints == maxHitPoints) return true;
        else return false;
    }

    //used by animation event after the player or boss finishes sword swinging animation
    public void StopSwimingSword()
    {
        isSwingingSword = false;
        animator.SetBool("IsSwingingSword", false);
    }

    //Pushes entity in the opposite direction of the collider of the object that called it
    public virtual void PushBack(ref Collider2D collision)
    {
        if (pushable)
        {
            var pushDirection = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);
        }
    }

    public virtual void PushBack(ref Collider2D collision, bool wasCharged)
    {
        if (pushable)
        {
            var pushDirection = (transform.position - collision.transform.position).normalized;
            if(wasCharged)GetComponent<Rigidbody2D>().AddForce(pushDirection * pushBackForce * 10 * GameManager.gameManager.GetDifficultyModifier(), ForceMode2D.Impulse);
            else GetComponent<Rigidbody2D>().AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);
        }
    }

    public virtual void InflictDamage(int damage, Collider2D colliderOfAttacker)
    {
        if (invulnerabilityMod != null)
        {
            if (!invulnerabilityMod.isInvulerable)
            {
                hitPoints -= damage;
                PushBack(ref colliderOfAttacker);
                Stun();
                invulnerabilityMod.MakeInvulnerable(ref invulnerabilityTimeAfterDmg);
            }
        }
        if (hitPoints < 1){ Die();}
    }

    public virtual void InflictDamage(int damage, Collider2D colliderOfAttacker, bool wasCharged)
    {
        if (invulnerabilityMod != null)
        {
            if (!invulnerabilityMod.isInvulerable)
            {
                hitPoints -= damage;
                PushBack(ref colliderOfAttacker, wasCharged);
                Stun();
                invulnerabilityMod.MakeInvulnerable(ref invulnerabilityTimeAfterDmg);
            }
        }
        if (hitPoints < 1) { Die(); }
    }

    protected abstract void Die();

    protected void Stun() { if(isStunnable)StartCoroutine(StunTimer()); }

    

    int stunIndex;
    protected virtual IEnumerator StunTimer()
    {
        if (!isStunned)
        {
            stunIndex = stunTime;
            isStunned = true;
            while (stunIndex > 0)
            {
                yield return new WaitForSeconds(1);
                stunIndex--;
            }
            isStunned = false;
        }
        else { stunIndex = stunTime; }
    }

    public void SwingSword()
    {
        isSwingingSword = true;
        animator.SetBool("IsSwingingSword", true);
    }




}
