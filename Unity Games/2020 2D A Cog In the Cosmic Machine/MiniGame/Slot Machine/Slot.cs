/*
 * Slot.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/12/2020 (en-US)
 * Description: Holds value for slot used in slot machine mini game.
 */

using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] int value;

    public int GetValue() { return value; }
}