using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] Sprite itemImage = null;
    [SerializeField] int stackSize = 99;
    [SerializeField] int price = 1;

    public Sprite GetSprite() { return itemImage; }
    public int GetStackSize() { return stackSize; }

    public int GetPrice() { return price; }
}
