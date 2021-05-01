using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour, IEscapeKeySubscriber
{
    [SerializeField] SelectableGrid recipeSlots = null;
    //[SerializeField] List<RecipeSlot> slots = new List<RecipeSlot>();
    [SerializeField] GameObject bookObject = null;
    [SerializeField] GridLayoutGroup layoutGroup = null;
    [SerializeField] Button craftButton = null;
    
    public List<Recipe> allRecipes = new List<Recipe>();
    public List<Recipe> filteredRecipes = new List<Recipe>();
    Inventory inventory;
    bool open = false;
    bool filtered = false;
    bool escapePressed = false;

    void Start()
    {
        GameManager.RegisterEscapeSubscriber(this, 0);
        for (int i = 0; i < recipeSlots.GetSelectables().Count; ++i)
        {
            ((RecipeSlot) recipeSlots.GetSelectables()[i]).Initialize(GetComponent<Inventory>(), this, i);
        }

        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        if (open)
        {
            craftButton.Select();
            if(Input.GetKeyDown(KeyCode.Joystick1Button0)){craftButton.onClick.Invoke();}
            if(escapePressed)
            {
                escapePressed = false;
                Close();
            }
        }
    }

    public void CraftItem()
    {
        if (GetSelectedSlot().RequirementsMet())
        {
            foreach (RequiredIngredient ingredient in GetSelectedSlot().GetRecipe().GetRequiredIngredientsAsStructs())
            {
                if (!inventory.RemoveItemFromInventory(ingredient.itemIngredient, ingredient.requiredAmount))
                {
                    Debug.Log("[RecipeBook] Item failed to remove required materials");
                }
            }
            inventory.AddItemToInventoryInFull(GetSelectedSlot().GetRecipe().GetCraftedItem());
        }
        GetSelectedSlot().Select();
    }

    public void ClearSelects()
    {
        foreach (IUI_Selectable slot in recipeSlots.GetSelectables())
        {
            ((RecipeSlot)slot).Deselect();
        }
    }

    public void Open()
    {
        bookObject.transform.localScale = Vector3.one;
        open = true;
    }

    public void Open(List<Recipe> recipes)
    {
        recipeSlots.Enable();
        bookObject.transform.localScale = Vector3.one;
        open = true;
        foreach (Recipe recipe in recipes)
        {
            foreach (Recipe playerRecipe in GetComponent<Player>().GetKnownRecipes())
            {
                if(recipe == playerRecipe){allRecipes.Add(recipe);}
            }
        }
        filtered = false;
        LoadRecipes();
    }

    public void Close()
    {
        recipeSlots.Disable();
        GameManager.ignorePause = false;
        bookObject.transform.localScale = Vector3.zero;
        open = false;
    }

    public bool IsOpen() { return open;}

    public void ToggleFilter()
    {
        filtered = !filtered;
        LoadRecipes();
    }

    public RecipeSlot GetSelectedSlot() { return (RecipeSlot) recipeSlots.GetSelectedSelectable();}
    
    public void SelectThisSlot(RecipeSlot slot){recipeSlots.SelectSelectable(slot);}

    void LoadRecipes()
    {
        foreach(IUI_Selectable slot in recipeSlots.GetSelectables()){((RecipeSlot)slot).ClearElement();}
        if (filtered)
        {
            int page = 0; 
            // if(recipeIndex > 0){ page = recipeIndex / filteredRecipes.Count;}
            for (int i = 0; i < recipeSlots.GetSelectables().Count && i + (page * recipeSlots.GetSelectables().Count) < filteredRecipes.Count; ++i)
            {
                ((RecipeSlot) recipeSlots.GetSelectables()[i]).SetRecipe(filteredRecipes[i + (page * recipeSlots.GetSelectables().Count)]);
            }
        }
        else
        {
            int page = 0; 
            //if(recipeIndex > 0){ page = recipeIndex / allRecipes.Count;}
            for (int i = 0; i < recipeSlots.GetSelectables().Count && i + (page * recipeSlots.GetSelectables().Count) < allRecipes.Count; ++i)
            {
                ((RecipeSlot) recipeSlots.GetSelectables()[i]).SetRecipe(allRecipes[i + (page * recipeSlots.GetSelectables().Count)]);
            }
        }
        GetSelectedSlot().Select();
    }
    public void EscapeKeyPressed()
    {
        escapePressed = true;
    }
}
