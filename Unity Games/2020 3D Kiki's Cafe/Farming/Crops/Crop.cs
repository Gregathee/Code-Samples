using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] int harvestYieldMin = 1;
    [SerializeField] int harvestYieldMax = 1;
    [SerializeField] int unwateredDaysTillDeath = 3;
    [SerializeField] int daysTillSprout = 1;
    [SerializeField] int daysTillMiddling = 3;
    [SerializeField] int daysTillHarvestable = 5;
    [SerializeField] Item harvestedItem = null;
    [SerializeField] bool requiresSickle = false;
    CropStage cropStage = CropStage.Seedling;
    int yield = -1;
    int daysPlanted = 0;
    int daysUnwattered = 0;
    bool watered = false;
    CropPlot plot;

    public CropStage GetStage() { return cropStage;}

    public bool RequiresSickle() { return requiresSickle;}

    public void Plant(CropPlot plot)
    {
        this.plot = plot;
        CropManager.RegisterCrop(this);
    }

    public void WaterPlant() { watered = true;}

    public Item Harvest() { return harvestedItem;}

    public int GetHarvestYield()
    {
        if (yield == -1) { yield = Random.Range(harvestYieldMin, harvestYieldMax + 1); }
        return yield;
    }

    public void ResetHarvestYield() { yield = -1;}
    
    public void AdvanceDay()
    {
        ++daysPlanted;
        if (watered) { ++daysPlanted; }
        else { ++daysUnwattered; }
        if (daysUnwattered >= unwateredDaysTillDeath && cropStage != CropStage.Harvestable)
        {
            CropManager.UnregisterCrop(this);
            plot.ClearCrop();
            daysPlanted = daysUnwattered = 0;
        }
        DetermineCropStage();
        watered = false;
        plot.HideWaterIndicator();
    }

    void DetermineCropStage()
    {
        if (daysPlanted >= daysTillHarvestable) { cropStage = CropStage.Harvestable; }
        else if (daysPlanted >= daysTillMiddling) { cropStage = CropStage.Middling; }
        else if (daysPlanted >= daysTillSprout) { cropStage = CropStage.Sprout; }
        plot.UpdateName();
    }
}

public enum CropStage{Seedling, Sprout, Middling, Harvestable}
