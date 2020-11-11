/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Detects compete meal, increments score, removes complete meal
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderupCounter : FoodHolder, IFoodProcessor
{
	public FoodProcessorType FoodProcessorType() { return global::FoodProcessorType.OrderupCounter; }

	public GameObject GameObject() { return gameObject; }

	public void ProcessFood() { if (ingredient != null) { ingredient.GetComponent<IIngredient>().Process(this); } }
}
