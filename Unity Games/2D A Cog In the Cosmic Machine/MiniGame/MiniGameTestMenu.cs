/*
 * MiniGameTestMenu.cs
 * Author(s): Steven Drovie []
 * Created on: 10/21/2020 (en-US)
 * Description: Handles menu actions for the MiniGames Testing Menu scene
 */

using System;
using UnityEngine;

public class MiniGameTestMenu : MonoBehaviour
{
    private AdditiveSceneManager additiveSceneManager;

    [SerializeField] private GameObject buttonMenu;
    [SerializeField] private GameObject returnButton;
    
    private string currentMiniGame = null;


    private void Start()
    {
        additiveSceneManager = FindObjectOfType<AdditiveSceneManager>();
        returnButton.SetActive(false);
    }

    public void StartMiniGame(string miniGame)
    {
        if(currentMiniGame != null) return;
        buttonMenu.SetActive(false);
        returnButton.SetActive(true);
        currentMiniGame = miniGame;
        additiveSceneManager.LoadSceneSeperate(miniGame);
    }

    public void EndMiniGame()
    {
        if(currentMiniGame == null) return;
        buttonMenu.SetActive(true);
        returnButton.SetActive(false);
        additiveSceneManager.UnloadScene(currentMiniGame);
        currentMiniGame = null;
    }
}
