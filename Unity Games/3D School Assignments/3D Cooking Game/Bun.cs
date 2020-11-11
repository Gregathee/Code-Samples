/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Holds a burger
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bun : FoodHolder, IIngredient
{
	public bool Complete() { return ingredient; }

	public void Destroy()
	{
		if (ingredient != null) { Destroy(ingredient); }
		Destroy(gameObject);
	}

	public GameObject GameObject() { return gameObject; }

	public override bool HoldFood(GameObject ingredient)
	{
		Burger burger = ingredient.GetComponent<Burger>();
		if (burger) { if (burger.State == BurgerState.Cooked) 
			{
				if (GameManager.Instance.isTutorial) { Tutorial.Instance.GrabMeal(); }
				return base.HoldFood(ingredient); } }
		return false;
	}

	public void Process(IFoodProcessor foodProcessor) {  /*Intentially blank*/}
}
