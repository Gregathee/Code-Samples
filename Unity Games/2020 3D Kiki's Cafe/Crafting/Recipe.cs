using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct RequiredIngredient
{
    public int requiredAmount;
    public ItemIngredient itemIngredient;
}

public class Recipe : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] Item craftedItem = null;
    [SerializeField] List<RequiredIngredient> ingredients = new List<RequiredIngredient>();
    List<string> requiredIngredients = new List<string>();

    
    //change to initialize
    public void Initialize()
    {
        requiredIngredients.Clear();
        foreach (RequiredIngredient ingredient in ingredients)
        {
            requiredIngredients.Add(ingredient.requiredAmount + " " + ingredient.itemIngredient.name);
        }
    }

    public Item GetCraftedItem() { return craftedItem;}

    public Sprite GetSprite() { return sprite;}

    public List<string> GetRequiredIngredientsAsStrings() { return requiredIngredients; }

    public List<RequiredIngredient> GetRequiredIngredientsAsStructs() { return ingredients; }
}
