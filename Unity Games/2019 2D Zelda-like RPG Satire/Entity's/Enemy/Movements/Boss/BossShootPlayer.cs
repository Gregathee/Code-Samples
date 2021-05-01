using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Not implemented yet
public class BossShootPlayer : MovementDirective
{
    [SerializeField] Projectile projectile = null;
    [SerializeField] Transform[] teleportPoints;
    [SerializeField] GameObject firePoint;
    bool teleport = true;
    bool checking = false;
    int lastTeleportPoint;

    private void Update()
    {
        if (phaseComplete) { teleport = true; animator.SetBool("IsShooting", false); }
    }

    public override void Move()
    {
        base.Move();
        animator.speed = 1f;
        if (teleport && playerWithInDetectRange && !phaseComplete && !checking)
        {
            
            animator.SetBool("Teleport", false);
            StartCoroutine(ShootPlayer());
        }
    }

    IEnumerator ShootPlayer()
    {
        if (!PhaseComplete())
        {
            int teleportPoint = Random.Range(0, teleportPoints.Length - 1);
            while (GameManager.gameManager.IsPaused())
            {
                yield return null;
            }
            bool notLastPoint = false;
            bool pointNotColliding = false;
            int maxLoops = 10;
            checking = true;
            while (!notLastPoint && !pointNotColliding && maxLoops > 0)
            {

                if (teleportPoint != lastTeleportPoint)
                {
                    notLastPoint = true;
                }
                else
                {
                    notLastPoint = false;
                }
                if (notLastPoint)
                {
                    Collider2D collider = teleportPoints[teleportPoint].GetComponent<Collider2D>();
                    List<Collider2D> colliders = new List<Collider2D>();
                    ContactFilter2D filter = new ContactFilter2D();
                    collider.OverlapCollider(filter.NoFilter(), colliders);

                    if (colliders.Count > 0)
                    {
                        pointNotColliding = false;
                    }
                    else
                    {
                        pointNotColliding = true;
                    }
                }
                maxLoops--;
            }
            checking = false;
            if (notLastPoint && pointNotColliding)
            {
                enemy.transform.position = teleportPoints[teleportPoint].position;
                lastTeleportPoint = teleportPoint;
                animator.SetBool("IsShooting", true);
                teleport = false;
            }
        }
        else { Debug.Log("Phase Complete"); }
        yield return null;
    }

    

    public void Teleport()
    {
        if (!GameManager.gameManager.IsPaused())
        {
            teleport = true;
            animator.SetBool("Teleport", true);
            Projectile tempProjectile;
            tempProjectile = Instantiate(projectile, firePoint.transform.position, new Quaternion());
            tempProjectile.ConnectEnemySender(enemy);
        }
    }

    public override void StopCharge()
    {
    }
}
