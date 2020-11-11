using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object once collided with by the player, restores player health
public class HeartReplenisher : MonoBehaviour
{
    [SerializeField] int replenishAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!GameManager.player.IsFullHealth())
            {
                GameManager.player.ReplenishHeart(ref replenishAmount);
                Destroy(gameObject);
            }
        }
    }
}
