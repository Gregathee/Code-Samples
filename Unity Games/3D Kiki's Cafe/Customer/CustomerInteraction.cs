using UnityEngine;

//Objects such as tables or cash register use this connect the player to a customer and their request
public class CustomerInteraction : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] Transform customerTarget = null;
    Customer customer = null;
    bool hasCustomer = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            Customer tempCustomer = other.GetComponent<Customer>();
            if (tempCustomer)
            {
                if (!tempCustomer.IsServed()) { tempCustomer.ShowRequest(true); }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Customer"))
        {
            customer = other.GetComponent<Customer>();
            hasCustomer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Customer")) { EjectCustomer(); }
    }

    public void EjectCustomer()
    {
        customer = null;
        hasCustomer = false;
    }

    public Transform GetCustomerTarget() { return customerTarget;}

    public bool HasCustomer() { return hasCustomer;}
    public string GetName() { return name; }
    public void Interact(Player player)
    {
        if (customer)
        {
            if (player.GetInventory().RemoveItemFromInventory(customer.GetRequestedItem()))
            {
                player.AddMoney(customer.GetRequestedItem().GetPrice());
                customer.GoHome();
            }
        }
    }
}
