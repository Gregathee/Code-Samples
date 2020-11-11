/*
 * (Greg Brandt)
 * (Assignment 3)
 * Detects collision with Dogs, adds to score then destroys self
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionsX : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dog"))
        {
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<DisplayScore>().score++;
            Destroy(gameObject);
        }
    }
}
