using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [SerializeField] NavMeshAgent ai = null;
    [SerializeField] RecipeRequest request;
    [SerializeField] float speed = 1;
    Transform home;
    bool served = false;
    void Start()
    {
        request.gameObject.SetActive(false);
    }


    void Update()
    {
        if (GameManager.paused || ClockSystem.StopTime) { ai.speed = 0; }
        else { ai.speed = speed; }
    }
    public void SetDestination(Transform target, Transform newHome)
    {
        home = newHome;
        ai.SetDestination(target.position);
    }

    public RecipeRequest GetRecipeRequest() { return request; }

    public void GoHome()
    {
        served = true;
        ShowRequest(false);
        ai.SetDestination(home.position);
    }

    public bool IsServed() { return served; }

    public Item GetRequestedItem() { return request.RequestedItem(); }

    public void ShowRequest(bool show) { request.gameObject.SetActive(show); }
}
