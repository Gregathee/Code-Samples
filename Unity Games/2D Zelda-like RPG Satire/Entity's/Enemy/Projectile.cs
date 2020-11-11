using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 1;
    [SerializeField] protected int attackDamage = 1;
    protected Vector3 targetsLastPosition = Vector3.zero;
    protected Enemy enemy = null;
    bool returnToSender = false;
    protected bool isPaused = false;
    protected bool canResume = false;
    float torque = 10;
    protected Vector3 pushDirection;
    protected Vector2 force;
    protected Rigidbody2D rb;

    private void Start()
    {
        targetsLastPosition = GameManager.player.transform.position;
        StartCoroutine(StartDeathTimer());
        pushDirection = (targetsLastPosition - transform.position).normalized;
        rb = GetComponent<Rigidbody2D>();
        force = pushDirection * speed * GameManager.gameManager.GetDifficultyModifier();
        rb.AddForce(force, ForceMode2D.Impulse);
        rb.AddTorque(torque, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!GameManager.gameManager.EntitiesCanMove())
        {
            rb.velocity = Vector2.zero;
            isPaused = true;
            canResume = false;
        }
        else if (!returnToSender && !isPaused && canResume)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            isPaused = false;
            canResume = false;
        }
        else if(GameManager.gameManager.EntitiesCanMove() && isPaused)
        {
            canResume = true;
            isPaused = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameManager.player.InflictDamage(attackDamage, this.GetComponent<Collider2D>());
            Destroy(gameObject);
        }
        SwordTip sword = collision.GetComponent<SwordTip>();
        if(sword != null)
        {
            if (sword.canDamage)
            {
                returnToSender = true;
                pushDirection = (transform.position - collision.transform.position).normalized;
                rb.AddForce(pushDirection * speed * 2, ForceMode2D.Impulse);
            }
        }
        //Allows friendly fire
        if(collision.tag == "Enemy")
        {
            if(collision.GetComponent<EnemyTriggerRadius>() != null)
            { 
                Enemy enemyToHit = collision.GetComponent<EnemyTriggerRadius>().GetEnemy();
                if (enemyToHit.GetSenderID() == enemy.GetSenderID() && !returnToSender) { }
                else
                {
                    enemyToHit.InflictDamage(attackDamage, GetComponent<Collider2D>());
                    Destroy(gameObject);
                }
            }
        }
        //Destroy object upon colistion with enviornment. 
        bool props = collision.name == "Props";
        bool castle = collision.name.Contains("Castle");
        if (props || castle){ Destroy(gameObject); }
    }

    //Used to prevent sender from being damaged by upon instantiation
    public void ConnectEnemySender(Enemy enemy) { this.enemy = enemy; }

    IEnumerator StartDeathTimer()
    {
        yield return new WaitForSeconds(1);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(1);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(1);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(1);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(1);
        while (isPaused) yield return null;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
