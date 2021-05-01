using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideableRoof : MonoBehaviour
{
    [SerializeField] GameObject hideableObject = null;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))hideableObject.SetActive(false);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))hideableObject.SetActive(true);
    }
}
