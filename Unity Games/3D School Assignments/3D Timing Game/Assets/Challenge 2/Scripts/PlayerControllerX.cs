/*
 * (Greg Brandt)
 * (Assignment 3)
 * Spawns prefabs from player with space
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    float coolDownTime = 0.5f;
    bool cooledDown = true;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && cooledDown)
        {
            cooledDown = false;
            StartCoroutine(StartCoolDown());
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
        }
    }

    IEnumerator StartCoolDown()
	{
        yield return new WaitForSeconds(coolDownTime);
        cooledDown = true;
	}
}
