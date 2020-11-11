/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Controls the player with input. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : FoodHolder
{
	[SerializeField] float moveSpeed = 0;
	new Rigidbody rigidbody = null;

	private void Start() { rigidbody = GetComponent<Rigidbody>(); }
	private void Update() { if (Input.GetKeyDown(KeyCode.Space)) { ActivateFoodHolder(); } }
	private void FixedUpdate()
	{
		Move();
		Rotate();
	}

	void Rotate()
	{
		if (Input.GetKey(KeyCode.W)) { transform.eulerAngles = new Vector3(0, 0, 0); }
		if (Input.GetKey(KeyCode.S)) { transform.eulerAngles = new Vector3(0, 180, 0); }
		if (Input.GetKey(KeyCode.D)) { transform.eulerAngles = new Vector3(0, 90, 0); }
		if (Input.GetKey(KeyCode.A)) { transform.eulerAngles = new Vector3(0, -90, 0); }
	}

	private void Move()
	{
		float horizontalMovement = Input.GetAxis("Horizontal");
		float verticalMovement = Input.GetAxis("Vertical");
		rigidbody.MovePosition(transform.position + (new Vector3(horizontalMovement, 0, verticalMovement).normalized * moveSpeed * Time.fixedDeltaTime));
    }

	void ActivateFoodHolder()
	{
		RaycastHit hit;
		Vector3 position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		//Attempt to find a food holder
		Physics.Raycast(position,transform.forward, out hit, 1);
		Debug.DrawRay(position, transform.forward.normalized, Color.red, 1);
		if (hit.collider)
		{
			FoodHolder foodHolder = hit.collider.GetComponent<FoodHolder>();
			IFoodProcessor foodProcessor = hit.collider.GetComponent<IFoodProcessor>();
			bool heldFood = false;

			//Attempt to give food to food holder
			if (foodHolder && ingredient) { heldFood = foodHolder.HoldFood(ingredient); }

			//If if food holder didn't take our ingredient because we didn't have one, attempt to take ingredient from food holder
			if(!heldFood && !ingredient){ foodHolder.ReleaseFood(this); UpdateTutorial(); }

			//If food holder took ingredient, clear reference
			else { ingredient = null; }

			//if food holder was a food processor, proccess food
			if (foodProcessor != null) { foodProcessor.ProcessFood(); }
		}
	}

	void UpdateTutorial()
	{
		if (GameManager.Instance.isTutorial)
		{
			Burger burger = ingredient.GetComponent<Burger>();
			Plate plate = ingredient.GetComponent<Plate>();
			if (burger)
			{
				if (burger.State == BurgerState.Uncooked) { Tutorial.Instance.PlaceUncooked(); }
				if (burger.State == BurgerState.Cooked) { Tutorial.Instance.PlaceCooked(); }
			}
			if (plate) { if (plate.Complete()) { Tutorial.Instance.PlaceMeal(); } }
		}
	}
}
