/*
* Greg Brandt
* Assignment 7
* Controls enemy behavior
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 3.0f;
    Rigidbody enenmyRb;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enenmyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enenmyRb.AddForce(lookDirection * speed);
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
