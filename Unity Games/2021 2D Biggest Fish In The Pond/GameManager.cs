using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the state of the game for pausing
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    bool _paused;    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { _paused = !_paused; }
        _pauseMenu.SetActive(_paused);
        Time.timeScale = _paused ? 0 : 1;
    }

    public void Unpause()
    {
        _paused = false;
    }
}
