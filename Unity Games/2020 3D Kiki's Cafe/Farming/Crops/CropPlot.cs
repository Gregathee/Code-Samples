using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPlot : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] GameObject waterIndicator = null;
    [SerializeField] GameObject tilledIndicator = null;
    Crop crop = null;
    ItemCropSeed seed = null;
    bool tilled = false;
    public string GetName() { return name; }
    public void Interact(Player player)
    {
        if (player.GetSelectedHotBarSlot().GetItem())
        {
            ItemTool tool = player.GetSelectedHotBarSlot().GetItem().GetComponent<ItemTool>();
            if (tool) { InteractWithTool(ref player, ref tool); }
        }
        if (seed) { if (crop.GetStage() == CropStage.Harvestable && !crop.RequiresSickle()) { Harvest(ref player); } }
        else if(tilled)
        {
            if (player.GetSelectedHotBarSlot().GetItem() != null) { seed = player.GetSelectedHotBarSlot().GetItem().GetComponent<ItemCropSeed>(); }
            if (seed)
            {
                crop = seed.GetCrop();
                PlantCrop(ref player);
            }
        }
    }

    public void HideWaterIndicator()
    {
        waterIndicator.SetActive(false);
    }

    void InteractWithTool(ref Player player, ref ItemTool tool)
    {
        switch (tool.GetToolType())
        {
            case ToolType.Hoe:
                tilled = true;
                tilledIndicator.SetActive(true);
                break;
            case ToolType.WateringCan:
                if (crop)
                {
                    crop.WaterPlant();
                    waterIndicator.SetActive(true);
                }

                break;
            case ToolType.Sickle:
                if(crop){if(crop.RequiresSickle() && crop.GetStage() == CropStage.Harvestable){Harvest(ref player);}}
                break;
        }
    }

    void Harvest(ref Player player)
    {
        if (player.AddItemToInventoryInFull(crop.Harvest(), crop.GetHarvestYield()))
        {
            crop.ResetHarvestYield();
            ClearCrop();
        }
    }

    public void ClearCrop()
    {
        if(crop){Destroy(crop.gameObject);}
        crop = null;
        seed = null;
        name = "Empty Crop Plot";
        tilledIndicator.SetActive(false);
        tilled = false;
    }

    public void UpdateName()
    {
        if (crop) { name = crop.name + " (" + crop.GetStage() + ")"; }
        else { name = "Empty Crop Plot"; }
    }
    
    void PlantCrop(ref Player player)
    {
        crop = Instantiate(crop, transform.position, new Quaternion());
        crop.name = crop.name.Replace("(Clone)", "");
        crop.Plant(this);
        UpdateName();
        player.GetSelectedHotBarSlot().PopItem();
    }
}
