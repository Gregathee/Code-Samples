using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDispenser : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] Item item = null;
    [SerializeField] int maxStock = 1;
    [SerializeField] int minRestock = 1;
    [SerializeField] int maxRestock = 1;
    [SerializeField] bool stockDaily = true;
    [SerializeField] ToolType requiredToolType = ToolType.None;
    int stock = 0;

    void Start()
    {
        if(stockDaily){ClockSystem.Instance.RegisterDailyRestock(this);}
    }

    public void Restock()
    {
        stock += Random.Range(minRestock, maxRestock + 1);
        if (stock > maxStock) { stock = maxStock; }
    }
    
    public string GetName() { return item.name + "(" + stock + ")"; }
    public void Interact(Player player)
    {
        if (requiredToolType == ToolType.None)
        {
            if (stock > 0)
            {
                if (player.AddItemToInventoryInFull(item, stock)) { stock = 0; }
            }
        }
        else if (player.GetSelectedHotBarSlot().GetItem())
        {
            ItemTool tool = player.GetSelectedHotBarSlot().GetItem().GetComponent<ItemTool>();
            if (tool)
            {
                if (tool.GetToolType() == requiredToolType && stock > 0)
                {
                    if (player.AddItemToInventoryInFull(item, stock)) { stock = 0; }    
                }
            }
        }
    }
}
