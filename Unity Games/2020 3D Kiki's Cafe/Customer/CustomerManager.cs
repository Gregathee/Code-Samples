using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance = null;
    [SerializeField] List<Customer> customerPrefabs = new List<Customer>();
    [SerializeField] List<Customer> spawnedCustomers = new List<Customer>();
    [SerializeField] List<CustomerInteraction> interactables = new List<CustomerInteraction>();
    [SerializeField] List<Recipe> recipes = null;
    [SerializeField] float minSpawnTime = 1;
    [SerializeField] float maxSpawnTIme = 10;
    [SerializeField] int openTime = 9;
    [SerializeField] int closeTime = 17;

    void Awake()
    {
        if (!Instance) { Instance = this; }
        else {Destroy(this.gameObject);}
    }
    
    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            Customer customer = other.GetComponent<Customer>();
            if (customer.IsServed())
            {
                spawnedCustomers.Remove(other.GetComponent<Customer>());
                Destroy(other.gameObject);
            }
        }
    }

    public void SendAllCustomersHome()
    {
        foreach (Customer customer in spawnedCustomers) { customer.GoHome(); }
    }

    public void DestroyAllCustomers()
    {
        foreach (Customer customer in spawnedCustomers) { Destroy(customer.gameObject); }
        spawnedCustomers.Clear();
        foreach(CustomerInteraction interaction in interactables){interaction.EjectCustomer();}
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            while (ClockSystem.Instance.GetHour() < openTime || ClockSystem.Instance.GetHour() >= closeTime)
            {
                if(spawnedCustomers.Count > 0){SendAllCustomersHome();}
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTIme));
            List<CustomerInteraction> randomInteractables = new List<CustomerInteraction>();
            foreach (CustomerInteraction interactable in interactables)
            {
                if(!interactable.HasCustomer()){randomInteractables.Add(interactable);}
            }
            if (randomInteractables.Count > 0)
            {
                Customer newCustomer = Instantiate(customerPrefabs[Random.Range(0, customerPrefabs.Count)], transform.position, transform.rotation);
                spawnedCustomers.Add(newCustomer);
                newCustomer.GetRecipeRequest().Initialize(recipes[Random.Range(0, recipes.Count)]);
                newCustomer.SetDestination(randomInteractables[Random.Range(0, randomInteractables.Count)].GetCustomerTarget(), transform);
            }
            
        }
    }
}
