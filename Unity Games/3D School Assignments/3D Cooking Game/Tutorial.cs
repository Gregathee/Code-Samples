/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Manages the tutorial location indicator
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : Singleton<Tutorial>
{
	[SerializeField] GameObject tutorialRing;
	[SerializeField] Transform grabPlate = null;
	[SerializeField] Transform placePlate = null;
	[SerializeField] Transform grabBun = null;
	[SerializeField] Transform placeBun = null;
	[SerializeField] Transform grabRaw = null;
	[SerializeField] Transform placeRaw = null;
	[SerializeField] Transform grabUncooked = null;
	[SerializeField] Transform placeUncooked = null;
	[SerializeField] Transform grabCooked = null;
	[SerializeField] Transform placeCooked = null;
	[SerializeField] Transform grabMeal = null;
	[SerializeField] Transform placeMeal = null;

	private void Start() { GrabPlate(); }

	public void GrabPlate() { tutorialRing.transform.position = grabPlate.position; }
	public void PlacePlate() { tutorialRing.transform.position = placePlate.position; }
	public void GrabBun() { tutorialRing.transform.position = grabBun.position; }
	public void PlaceBun() { tutorialRing.transform.position = placeBun.position; }
	public void GrabRaw() { tutorialRing.transform.position = grabRaw.position; }
	public void PlaceRaw() { tutorialRing.transform.position = placeRaw.position; }
	public void GrabUncooked() { tutorialRing.transform.position = grabUncooked.position; }
	public void PlaceUncooked() { tutorialRing.transform.position = placeUncooked.position; }
	public void GrabCooked() { tutorialRing.transform.position = grabCooked.position; }
	public void PlaceCooked() { tutorialRing.transform.position = placeCooked.position; }
	public void GrabMeal() { tutorialRing.transform.position = grabMeal.position; }
	public void PlaceMeal() { tutorialRing.transform.position = placeMeal.position; }
}
