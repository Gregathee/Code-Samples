/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Manages the state of a burger
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BurgerState { Raw, Uncooked, Cooked}
public class Burger : MonoBehaviour, IIngredient
{
    [SerializeField] GameObject rawMeat = null;
    [SerializeField] GameObject uncookedMeat = null;
    [SerializeField] GameObject cookedMeat = null;
    BurgerState state = BurgerState.Raw;

	public BurgerState State { get => state;  }

	private void Start()
	{
		rawMeat.gameObject.SetActive(true);
		uncookedMeat.SetActive(false);
		cookedMeat.SetActive(false);
	}

	public bool Complete() { return false; }

	public GameObject GameObject() { return gameObject; }

	public void Process(IFoodProcessor foodProcessor)
	{
		//Change state of meat based off of food processor and current state
		switch(foodProcessor.FoodProcessorType())
		{
			case FoodProcessorType.CuttingBoard:
				if(state == BurgerState.Raw)
				{
					rawMeat.SetActive(false);
					uncookedMeat.SetActive(true);
					state = BurgerState.Uncooked;
				}
				break;
			case FoodProcessorType.Stove:
				if(state == BurgerState.Uncooked)
				{
					uncookedMeat.SetActive(false);
					cookedMeat.SetActive(true);
					state = BurgerState.Cooked;
				}
				break;
		}
	}

	public void Destroy() { Destroy(gameObject); }
}
