using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class King : Enemy
{
    [SerializeField] Slider healthBar;
    [SerializeField] float alterEnemySpawnRange;
    Vector3 originalPos;

    protected new void Start()
    {
        base.Start();
        originalPos = transform.position;
    }
    protected override void Move()
    {
        bool isSpining = animator.GetBool("IsSpining");
        bool canMove = GameManager.gameManager.EntitiesCanMove();
        if (!isStunned && canMove && !isSwingingSword || isSpining)
        {
            movement.Move();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (distanceFromPlayer < GetPlayerDetectionRadius())
        {
            healthBar.gameObject.SetActive(true);
        }
        else healthBar.gameObject.SetActive(false);
        healthBar.value = (float)hitPoints/(float)maxHitPoints;
    }
    private void OnDestroy()
    {
        if(healthBar != null) healthBar.gameObject.SetActive(false);
    }

    public void StopSpining() { animator.SetBool("IsSpining", false); }

    protected override void DropHeart()
    {
        Instantiate(heartReplenisher, transform.position, new Quaternion());
        Instantiate(heartReplenisher, transform.position + new Vector3(0, 0.1f, 0), new Quaternion());
        Instantiate(heartReplenisher, transform.position + new Vector3(0, -0.1f, 0), new Quaternion());
        Instantiate(heartReplenisher, transform.position + new Vector3(0.1f, 0, 0), new Quaternion());
        Instantiate(heartReplenisher, transform.position + new Vector3(-0.1f, 0, 0), new Quaternion());
    }

    public override void DamageEnemy(ref Collider2D collision)
    {
        Entity enemy;
        enemy = collision.gameObject.GetComponent<Player>();
        if (enemy == null)
        {
            enemy = collision.gameObject.GetComponent<EnemyTriggerRadius>().GetEnemy();
        }
        if (enemy != null && enemy != this)
        {
            enemy.InflictDamage(attackDamage, GetComponentInChildren<Collider2D>());
        }
    }

    public void ResetHealth()
    {
        hitPoints = maxHitPoints;
        transform.position = originalPos;
        StopSpining();
        GetComponentInChildren<BossMoveToPlayer>().ChangeFaceing(false, true, false, false);
    }

}
