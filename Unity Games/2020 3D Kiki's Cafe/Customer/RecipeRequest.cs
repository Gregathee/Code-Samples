using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Represents what a customer wants to order

public class RecipeRequest : MonoBehaviour
{
    Transform target = null;
    [SerializeField] Image recipeImage = null;
    [SerializeField] TMP_Text recipeText = null;
    Recipe recipe = null;

    void Start()
    {
        target = Camera.main.transform;
    }

    public void Initialize(Recipe newRecipe)
    {
        recipe = newRecipe;
        recipeText.text = recipe.name;
        recipeImage.sprite = recipe.GetSprite();
    }

    void Update()
    {
        transform.rotation = target.rotation;
    }

    public Item RequestedItem() { return recipe.GetCraftedItem(); }
}
