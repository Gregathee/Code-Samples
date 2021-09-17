using UnityEngine;

/// <summary>
/// Starts game
/// </summary>
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        FishSpawner.Instance.StartSpawn();
        AudioManager.instance.PlaySFX("LongBubbles_1");
    }
}