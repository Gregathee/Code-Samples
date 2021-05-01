/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Dispenses ingredients
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispensor : FoodHolder
{
	[SerializeField] GameObject ingredientPrefab = null;
	public override void ReleaseFood(FoodHolder foodTaker)
	{
		//Can hold an ingredient or dispense a new one
		if (ingredient) { if (foodTaker.HoldFood(ingredient)) { ingredient = null; } }
		else
		{
			if (GameManager.Instance.isTutorial)
			{
				if (ingredientPrefab.GetComponent<Plate>()) { Tutorial.Instance.PlacePlate(); }
				if (ingredientPrefab.GetComponent<Bun>()) { Tutorial.Instance.PlaceBun(); }
				if (ingredientPrefab.GetComponent<Burger>()) { Tutorial.Instance.PlaceRaw(); }
			}
			ingredient = Instantiate(ingredientPrefab);
			if (foodTaker.HoldFood(ingredient)) { ingredient = null;  }
			//if player already has ingredient, destroy this one
			else { Destroy(ingredient);  }
		}
	}
}
