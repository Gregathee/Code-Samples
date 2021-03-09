using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text interactableText = null;
    [SerializeField] TMP_Text moneyText = null;
    [SerializeField] Transform interactRaycastOrigin = null;
    [SerializeField] PlayerMovement movement = null;
    [SerializeField] List<Recipe> knownRecipes = new List<Recipe>();
    RecipeBook recipeBook = null;
    Inventory inventory = null;
    HotBar hotBar = null;
    IPlayerInteractable interactable = null;
    int money = 0;

    void Start()
    {
        recipeBook = GetComponent<RecipeBook>();
        inventory = GetComponent<Inventory>();
        hotBar = GetComponent<HotBar>();
        movement = GetComponent<PlayerMovement>();
        moneyText.text = "$" + money;
    }

    void Update()
    {
        if (GamePaused())
        {
            if(!hotBar.Frozen()){hotBar.FreezeHotkeys();} 
            ClockSystem.StopTime = true;
            return;
        }
        ClockSystem.StopTime = false;
        hotBar.UnfreezeHotkeys();
        movement.Move();
        GetInteractable();
        Interact();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = "$" + money;
    }

    public InventorySlot GetSelectedHotBarSlot() { return hotBar.GetSelectedSlot();}

    /// <summary>
    /// Adds an item and its amount to inventory and returns true if item amount fit in inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AddItemToInventoryInFull(Item item, int count = 1) { return inventory.AddItemToInventoryInFull(item, count); }

    public Inventory GetInventory() { return inventory;}

    public void OpenRecipeBook() { recipeBook.Open(); }
    public List<Recipe> GetKnownRecipes() { return knownRecipes;}

    public RecipeBook GetRecipeBook() { return recipeBook;}

    bool GamePaused() { return GameManager.paused || inventory.IsOpen() || recipeBook.IsOpen() || ClockSystem.StopTime; }

    void GetInteractable()
    {
        //Shoot raycast to get potential interactables
        RaycastHit hit;
        Physics.Raycast(interactRaycastOrigin.position, interactRaycastOrigin.forward, out hit, 1.5f);
        if (hit.collider)
        {
            Component[] components = hit.collider.GetComponents(typeof(IPlayerInteractable));

            if (components.Length == 0)
            {
                interactable = null;
                interactableText.text = "";
            }
            else
            {
                interactable = components[0] as IPlayerInteractable;
                interactableText.text = interactable.GetName();
            }
        }
        else
        {
            interactable = null;
            interactableText.text = "";
        }
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1) ) {interactable?.Interact(this); }
    }
}
