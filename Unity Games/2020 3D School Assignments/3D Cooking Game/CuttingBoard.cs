/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Turns raw meat into uncooked meat
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : FoodHolder, IFoodProcessor
{
	public FoodProcessorType FoodProcessorType() { return global::FoodProcessorType.CuttingBoard; }

	public GameObject GameObject() { return gameObject; }

	public void ProcessFood() { if(ingredient != null) { ingredient.GetComponent<IIngredient>().Process(this); } }
}
