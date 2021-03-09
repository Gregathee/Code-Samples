using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeSlot : UI_Slot
{
    Recipe recipe = null;
    [SerializeReference] Transform ingredientParent = null;
    [SerializeField] TMP_Text recipeName = null;
    [SerializeField] TMP_Text ingredientTextTemplate = null;
    List<TMP_Text> ingredientTexts = new List<TMP_Text>();
    Inventory inventory = null;
    RecipeBook recipeBook = null;
    int index = 0;
    
    public void Initialize(Inventory inventoryIn, RecipeBook recipeBookIn, int indexIn) 
    {
        inventory = inventoryIn;
        recipeBook = recipeBookIn;
        index = indexIn;
    }
    
    public void SetRecipe(Recipe recipeIn)
    {
        recipe = recipeIn;
        recipe.Initialize();
        elementImage.sprite = recipeIn.GetSprite();
        RequirementsMet();
    }
    
    public Recipe GetRecipe(){return recipe; }

    /// <summary>
    /// Returns true if player's inventory contains all necessary ingredients for recipe
    /// </summary>
    /// <returns></returns>
    public bool RequirementsMet()
    {
        bool requirementsMet = true;
        List<RequiredIngredient> requiredIngredients = recipe.GetRequiredIngredientsAsStructs();
        for (int i = 0; i < requiredIngredients.Count; ++i)
        {
            Item item = requiredIngredients[i].itemIngredient;
            int requiredAmount = requiredIngredients[i].requiredAmount;
            KeyValuePair<bool, int> results = inventory.SearchForItem(item, requiredAmount);
            
            if (!results.Key) { requirementsMet = false; }

            elementImage.color = requirementsMet ? Color.white : Color.gray;
        }
        return requirementsMet;
    }

    public override void Select()
    {
        recipeBook.ClearSelects();
        base.Select();
        //clear existing ingredient texts
        ingredientTexts.Clear();
        TMP_Text[] texts = ingredientParent.transform.GetComponentsInChildren<TMP_Text>();
        foreach(TMP_Text text in texts){Destroy(text.gameObject);}
        recipeName.text = "";
        if (!recipe) return;
        _displayRecipeInfo();

        bool requirementsMet = true;
        List<RequiredIngredient> requiredIngredients = recipe.GetRequiredIngredientsAsStructs();
        for (int i = 0; i < requiredIngredients.Count; ++i)
        {
            Item searchTarget = requiredIngredients[i].itemIngredient;
            int requiredAmount = requiredIngredients[i].requiredAmount;
            KeyValuePair<bool, int> results = inventory.SearchForItem(searchTarget, requiredAmount);
            
            if (results.Key) { ingredientTexts[i].color = Color.green; }
            else 
            {
                ingredientTexts[i].color = Color.red;
                requirementsMet = false;
            }
            //display to player information about requirements met status of the recipe
            elementImage.color = requirementsMet ? Color.white : Color.gray;
            ingredientTexts[i].text = results.Value + "/" + ingredientTexts[i].text;
        }
    }

    /// <summary>
    /// Displays recipe info to the recipe book UI
    /// </summary>
    void _displayRecipeInfo()
    {
        recipeName.text = recipe.name;
        foreach (string ingredient in recipe.GetRequiredIngredientsAsStrings())
        {
            TMP_Text text = Instantiate(ingredientTextTemplate, ingredientParent);
            text.text = ingredient;
            ingredientTexts.Add(text);
        }
    }

    public override void Invoke() { recipeBook.SelectThisSlot(this); }
    public override string GetDisplayName()
    {
        return "";
    }
    public override void ClearElement()
    {
        recipe = null;
        elementImage.sprite = defaultImage;
        Deselect();
    }
}
