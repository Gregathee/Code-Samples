/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Holds an ingredient in specified position
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodHolder : MonoBehaviour
{
	[SerializeField] protected Transform holdPosition = null;
	[SerializeField] protected GameObject ingredient = null;
	[SerializeField] bool tutorial = false;

	//Check if holding food, if not, hold food, return whether food was held
	public virtual bool HoldFood(GameObject ingredient)
	{
		FoodHolder foodHolder = null;
		if (this.ingredient) { foodHolder = this.ingredient.GetComponent<FoodHolder>(); }
		if (!this.ingredient)
		{
			if(!GetComponent<PlayerController>())UpdateTutorial(ingredient);
			this.ingredient = ingredient;
			ingredient.transform.position = holdPosition.position;
			ingredient.transform.SetParent(transform);
			return true;
		}
		else if (foodHolder) { return foodHolder.HoldFood(ingredient); }
		else { return false;}
	}

	public virtual void ReleaseFood(FoodHolder foodTaker) {  if (ingredient != null) { if (foodTaker.HoldFood(ingredient)) { ingredient = null; } } }

	private void UpdateTutorial(GameObject ingredient)
	{
		if (GameManager.Instance.isTutorial)
		{
			CuttingBoard cuttingBoard = GetComponent<CuttingBoard>();
			Stove stove = GetComponent<Stove>();
			Burger burger = ingredient.GetComponent<Burger>();
			Plate plate = ingredient.GetComponent<Plate>();
			if (burger)
			{
				if (burger.State == BurgerState.Raw && cuttingBoard) { Tutorial.Instance.GrabUncooked(); }
				if (burger.State == BurgerState.Uncooked && stove) { Tutorial.Instance.GrabCooked(); }
			}
			if (plate && tutorial) { Tutorial.Instance.GrabBun(); }
		}
	}

}
