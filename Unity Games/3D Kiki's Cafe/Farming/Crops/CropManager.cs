using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropManager : MonoBehaviour
{
    static List<Crop> crops = new List<Crop>();
    static List<Crop> cropsToRemove = new List<Crop>();
    
    public static void RegisterCrop(Crop crop){crops.Add(crop);}
    public static void UnregisterCrop(Crop crop){if(crops.Contains(crop)){ cropsToRemove.Add(crop); }}

    public static void AdvanceDay()
    {
        foreach(Crop crop in crops){crop.AdvanceDay();}

        if (cropsToRemove.Count > 0)
        {
            foreach (Crop crop in cropsToRemove) { crops.Remove(crop); }
            cropsToRemove.Clear();
        }
    }
}
