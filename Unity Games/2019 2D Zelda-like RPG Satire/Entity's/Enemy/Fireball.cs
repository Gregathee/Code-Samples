using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Projectile
{
    Animator animator;

    private void Start()
    {
        targetsLastPosition = GameManager.player.transform.position;
        pushDirection = (targetsLastPosition - transform.position).normalized;
        rb = GetComponent<Rigidbody2D>();
        force = pushDirection * speed * GameManager.gameManager.GetDifficultyModifier();
        rb.AddForce(force, ForceMode2D.Impulse);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!GameManager.gameManager.EntitiesCanMove())
        {
            rb.velocity = Vector2.zero;
            isPaused = true;
            canResume = false;
        }
        else if (!isPaused && canResume)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            isPaused = false;
            canResume = false;
        }
        else if (GameManager.gameManager.EntitiesCanMove() && isPaused)
        {
            canResume = true;
            isPaused = false;
        }
        if(Vector2.Distance(transform.position, targetsLastPosition) > -0.1f && 
            Vector2.Distance(transform.position, targetsLastPosition) < 0.1f)
        {
            Explode();
        }
        transform.up = new Vector3(transform.position.x - targetsLastPosition.x, transform.position.y - targetsLastPosition.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hitPlayer = false;
        bool hitEnemy = false;
        if (collision.tag == "Player")
        {
            GameManager.player.InflictDamage(attackDamage, this.GetComponent<Collider2D>());
            hitPlayer = true;
        }
        //Allows friendly fire
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyTriggerRadius>() != null)
            {
                Enemy enemyToHit = collision.GetComponent<EnemyTriggerRadius>().GetEnemy();
                if (enemyToHit.GetSenderID() == enemy.GetSenderID()) { }
                else
                {
                    enemyToHit.InflictDamage(attackDamage, GetComponent<Collider2D>());
                    hitEnemy = true;
                }
            }
        }
        //Destroy object upon colistion with enviornment. 
        bool props = collision.name == "Props";
        bool castle = collision.name.Contains("Castle");
        if (props || castle || hitPlayer || hitEnemy) 
        {
            Explode();
        }
    }

    void Explode()
    {
        animator.SetBool("ReachedDestination", true);
        rb.velocity = Vector2.zero;
        canResume = false;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
