/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Cookes uncooked meat
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : FoodHolder, IFoodProcessor
{
	public FoodProcessorType FoodProcessorType() { return global::FoodProcessorType.Stove; }

	public GameObject GameObject() { return gameObject; }

	public void ProcessFood() { if (ingredient != null) { ingredient.GetComponent<IIngredient>().Process(this); } }
}
