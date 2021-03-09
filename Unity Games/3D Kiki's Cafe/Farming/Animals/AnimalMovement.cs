using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float minMoveTime = 0;
    [SerializeField] float maxMoveTime = 0;
    Animator animator = null;
    bool waiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RandomMovement());
        StartCoroutine(StartAnimation());
    }

    void Update()
    {
        if (GameManager.paused || ClockSystem.StopTime)
        {
            animator.speed = 0;
            return;
        }
        if (animator.speed == 0 && !waiting)
        {
            StartCoroutine(StartAnimation());
        }
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    IEnumerator RandomMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime));
            while (GameManager.paused || ClockSystem.StopTime) { yield return null;}
            transform.eulerAngles = new Vector3(0, Random.Range(0.0f, 360), 0);
        }
    }

    IEnumerator StartAnimation()
    {
        waiting = true;
        animator.speed = 0;
        yield return new WaitForSeconds(Random.Range(0.0f, 5f));
        animator.speed = 1;
        waiting = false;
    }
}
