using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    float flashTime = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<InvulnerabilityMod>().MakeInvulnerable(ref flashTime, true);
            GameManager.player.SetRespawnPoint(this);
        }
    }
}
