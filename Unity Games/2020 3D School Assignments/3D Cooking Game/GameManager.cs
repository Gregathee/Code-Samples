/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Manages scene, scores and overall game state
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[SerializeField] GameObject congrats;
	[SerializeField] GameObject fail;
	[SerializeField] int requiredScore = 3;
    int score = 0;
	string curentLevelName = string.Empty;
	public bool isTutorial = false;

	public GameObject pauseMenu;

	private void Update() { if (Input.GetKeyDown(KeyCode.P)) { Pause(); } }

	public void SetTutorial(bool set) { isTutorial = set; }

	public void LoadLevel(string levelName)
	{
		AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		if (ao == null) { Debug.LogError("[GameManager] Unable to load level "); }
		curentLevelName = levelName;
	}

	public void UnloadLevel(string levelName)
	{
		AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
		if (ao == null) { Debug.LogError("[GameManager] Unable to unload level "); }
	}

	public void UnloadCurrentLevel()
	{
		AsyncOperation ao = SceneManager.UnloadSceneAsync(curentLevelName);
		if (ao == null) { Debug.LogError("[GameManager] Unable to unload level "); }
		score = 0;
		if (Tutorial.ISInitialized) { Tutorial.Instance.Destroy(); }
	}

	public void Pause()
	{
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
	}

	public void unpause()
	{
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
	}

	public virtual void Score() 
	{
		score++; 
		if(score == requiredScore || isTutorial) { congrats.SetActive(true); }
	}

	public void GameOver() { fail.SetActive(true); }
}
