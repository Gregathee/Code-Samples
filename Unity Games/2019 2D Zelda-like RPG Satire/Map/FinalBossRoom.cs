using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossRoom : MonoBehaviour
{
    [SerializeField]GameObject obsticle;
    bool preformedOnce = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !preformedOnce)
        {
            preformedOnce = true;
            GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().HalfEnemyCount();
            obsticle.SetActive(true);
        }
    }
}
