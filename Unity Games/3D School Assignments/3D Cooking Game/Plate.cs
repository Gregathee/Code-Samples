/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Holds buns and determines if meal is complete
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : FoodHolder, IIngredient
{
	public bool Complete()
	{
		//check for bun and check if bun has burger
		if (ingredient != null) { return ingredient.GetComponent<IIngredient>().Complete(); }
		else { return false; }
	}

	public void Destroy()
	{
		//destroy held ingredients then self
		if(ingredient != null) { Destroy(ingredient); }
		Destroy(gameObject);
	}

	public GameObject GameObject() { return gameObject; }

	public void Process(IFoodProcessor foodProcessor)
	{
		if(foodProcessor.FoodProcessorType() == FoodProcessorType.OrderupCounter && Complete())
		{
			GameManager.Instance.Score();
			this.Destroy();
		}
	}

	public override bool HoldFood(GameObject ingredient)
	{
		Bun bun = ingredient.GetComponent<Bun>();
		Burger burger = ingredient.GetComponent<Burger>();
		if (bun) 
		{
			if (bun && GameManager.Instance.isTutorial) { Tutorial.Instance.GrabRaw(); }
			return base.HoldFood(ingredient); 
		} 
		//if plate has a bun, pass burger to bun
		else if (burger && this.ingredient.GetComponent<Bun>()) 
		{ return this.ingredient.GetComponent<Bun>().HoldFood(ingredient); }
		return false;
	}
}
