using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCropSeed : Item
{
    [SerializeField] Crop cropPrefab = null;

    public Crop GetCrop() { return cropPrefab; }
}
