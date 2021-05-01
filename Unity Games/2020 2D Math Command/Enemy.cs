using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 1, distanceToExplode = 0.5f;
    [SerializeField]
    private int pointValue = 20;
    private Base target;
    private Vector3 targetPos = new Vector3(0, -3, 0);
    [SerializeField]
    private GameObject explosion;

    bool moving = true;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip loop = null;

    [SerializeField]
    private bool shieldEnemy = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = Random.Range(0.95f, 1.05f);

        if(shieldEnemy)
        {
            targetPos = new Vector3(-transform.position.x, transform.position.y);
            target = null;
        }
        else
        {
            target = BaseManager.Inst.GetRandomActiveBase();
            if(target)
                targetPos = target.GetPosition();

            var rot = Quaternion.Euler(0, 0, -180) * (targetPos - transform.position);
            transform.Find("Graphic").rotation = Quaternion.LookRotation(Vector3.forward, rot);
        }
    }

    private void Update()
    {
        if(moving)
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    
        if(Vector2.Distance(transform.position, targetPos) < distanceToExplode)
        {
            var variable = GetComponent<Problem>().Variable;
            EnemyManager.Inst.RemoveProblem(variable);
            EnemyManager.Inst.ForceDeselect(variable);
            target?.BlowUp();
            Explode();
        }

        if(!audioSource.isPlaying)
        {
            audioSource.clip = loop;
            audioSource.Play();
        }
    }

    private void ShipDestroyed()
    {
        if(shieldEnemy)
            BaseManager.Inst.ShieldBases();

        moving = false;
        ScoreManager.Inst.AddScore(pointValue);
    }

    public void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        MasterExploder.Inst.aaaAAAAAAAAA();
        Destroy(gameObject);
    }
}
