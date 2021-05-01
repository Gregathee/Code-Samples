using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Adjusts image of hearts based on player health
public class LifeBar : MonoBehaviour
{
    [SerializeField]Image[] hearts = null;
    int numberOfHearts = 0;
    int numberOfFullHearts = 0;
    Color fullHeart;
    Color emptyHeart;

    private void Start()
    {
        fullHeart = hearts[0].color;
        emptyHeart = fullHeart;
        emptyHeart.a = 0.5f;
        UpdatePlayerInfo();
    }

    public int GetNumberOfHearts() { return numberOfHearts; }
    public int GetNumberOfFullHearts() { return numberOfFullHearts; }
    public void AddHeart(int heartsToAdd) { numberOfHearts += heartsToAdd; DisplayHearts(); }

    public void UpdatePlayerInfo()
    {
        numberOfHearts = GameManager.player.GetMaxHitPoints();
        numberOfFullHearts = GameManager.player.GetHitPoints();
        DisplayHearts();
    }

    public void TakeDamage(int damage)
    {
        numberOfFullHearts -= damage;
        int i = 0;
        while( i<numberOfFullHearts) { hearts[i].color = fullHeart; i++;}
        while (i < numberOfHearts){ hearts[i].color = emptyHeart;i++;}
        DisplayHearts();
    }

    public void Heal(int healAmount)
    {
        numberOfFullHearts += healAmount;
        int i = 0;
        while (i < numberOfFullHearts) { hearts[i].color = fullHeart; i++; }
        if(i != numberOfFullHearts) while (i < numberOfHearts) { hearts[i].color = emptyHeart; i++; }
        DisplayHearts();
    }

    void DisplayHearts()
    {
        int i = 0;
        while (i < numberOfHearts){ hearts[i].gameObject.SetActive(true);i++;}
        while (i < hearts.Length){ hearts[i].gameObject.SetActive(false); i++;}
    }
}
