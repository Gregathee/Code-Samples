using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made for the purpose of having a serializable 2D array of strings for dialog purposes
[System.Serializable]
public class StringArray 
{
    [SerializeField] public string[] elements;

    public string[] GetArray() { return elements; }
}
