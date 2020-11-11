using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMissile : MonoBehaviour
{
    [SerializeField]
    private float explodeDistance = 1f;

    private Transform graphic;

    private Enemy target;
    private Transform targetTransform;

    public void SetTarget(Enemy enemy)
    {
        target = enemy;
        targetTransform = enemy.GetComponent<Transform>();
        graphic = transform.Find("Graphic");
    }

    private void Update()
    {
        if(!target)
            return;

        HandleTransform();

        if(Vector2.Distance(transform.position, targetTransform.position) < explodeDistance)
            Explode();
    }

    private void HandleTransform()
    {
        Vector3 nextPos = Vector2.up * 2 * Time.deltaTime
            + Vector2.MoveTowards(transform.position, targetTransform.position, 3 * Time.deltaTime);

        var rot = nextPos - transform.position;
        graphic.rotation = Quaternion.LookRotation(Vector3.forward, rot);

        transform.position = nextPos;
    }

    private void Explode()
    {
        target.Explode();

        // Do explosion stuff on here too
        // Explosions are cool

        Destroy(gameObject);
    }
}
