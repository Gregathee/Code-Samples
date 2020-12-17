/*
 * EnergyLevelStabalizeMiniGame.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/1/2020 (en-US)
 * Description: Manages engery stabilization mini game
 */


using Ink.Parsed;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyLevelStabalizeMiniGame : MiniGame
{
	[SerializeField] Slider[] sliders = null;
	[SerializeField] MiniGameButton[] buttonSwitches = null;
	[SerializeField] TMP_Text optimizationText = null;
	[Tooltip("Segments of power level bar that get added to or removed from vertical layout group to indicate current power level.")]
	[SerializeField] GameObject[] powerBarIndicators = null;
	public List<float> valueLevels = new List<float>();
	float[] sliderTargets = null;
	int[] buttonSwitchTargets = null;


	private void Start()
	{
		InitializeGame();
		int goal = 50;
		//Ensure starting game state doesn't start within 10 of goal
		while (Mathf.Abs(CalculatePowerLevel() - goal) < 10) { InitializeGame(); }
	}
	public List<float> optimizationLevels;
	
	private void Update()
	{
		if (!gameOver)
		{
			int total = CalculatePowerLevel();

			//Make indicator number the percent of indicators active based on total
			int indicatorNumber = (total / (100 / powerBarIndicators.Length));

			for (int i = 0; i < powerBarIndicators.Length; i++) { powerBarIndicators[i].SetActive(i < indicatorNumber); }

			total += 50;
			optimizationText.text = (total + "%");
			if (total == 100) { EndMiniGameSuccess(); }
		}
	}

	void InitializeGame()
	{
		float total = 100;
		//Assign random values of buttons, switches and sliders and subtract them from 100
		for (int i = 0; i < sliders.Length + buttonSwitches.Length; i++)
		{
			float randomValue = Random.Range(0f, sliders.Length + buttonSwitches.Length + 1);
			valueLevels.Add((float)randomValue);
			total -= (float)randomValue;
		}
		//turn (total - assigned values) into a percentage and distribute to all the values so they add up to 100
		total = total / (float)((float)sliders.Length + (float)buttonSwitches.Length);
		for (int i = 0; i < valueLevels.Count; i++) { valueLevels[i] += total; }

		sliderTargets = new float[sliders.Length];
		buttonSwitchTargets = new int[buttonSwitches.Length];

		//Assign target values for buttons, switches, and sliders.
		RandomizeTargets();

		//Randomize starting values for buttons, switches, and sliders.
		foreach (Slider slider in sliders) { slider.value = Random.Range(0f, 1f); }
		foreach (MiniGameButton button in buttonSwitches)
		{
			int rand = Random.Range(0, 2);
			if (rand == 0) { button.ChangeValue(); }
		}
	}

	int CalculatePowerLevel()
	{
		int index = 0;
		optimizationLevels = new List<float>();
		for (int i = 0; i < sliders.Length; i++)
		{
			//Adjust slider values to only have 1 of 5 values [0.2, 0.4, 0.6, 0.8, 1] to decrease required precision 
			float num = 1 - (Mathf.Abs(sliderTargets[i] - sliders[i].value));
			optimizationLevels.Add(num * valueLevels[index]);
			index++;
		}
		for (int i = 0; i < buttonSwitches.Length; i++)
		{
			float value = (1 - (Mathf.Abs(buttonSwitchTargets[i] - buttonSwitches[i].value))) * (valueLevels[index]);
			index++;
			optimizationLevels.Add(value);
		}
		int total = 0;
		//add the rounded 
		foreach (float optimizationLevel in optimizationLevels)
		{
			total += Mathf.RoundToInt((optimizationLevel));
		}
		return total;
	}

	void RandomizeTargets()
	{
		for (int i = 0; i < sliders.Length; i++) { sliderTargets[i] = Random.Range(0f, 1f); }
		for (int i = 0; i < buttonSwitches.Length; i++) { buttonSwitchTargets[i] = Random.Range(0, 2); }
	}
}