using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to quest items
public class Glasses : MonoBehaviour
{
    [SerializeField] GameObject questIndicator;
    float questIndicatorDistance = 1.2f;

    private void Start()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Vector4(0,1,1,1);
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponentInChildren<SpriteRenderer>().color = new Vector4(1, 1, 1, 1);
        }
    }

    private void Update()
    {
        Vector2 playerPosition = GameManager.player.transform.position;
        float distanceFromPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceFromPlayer < questIndicatorDistance && GameManager.gameManager.QuestPointerVersion())
        {
            questIndicator.SetActive(true);
        }
        else questIndicator.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") Destroy(gameObject);
    }
}
