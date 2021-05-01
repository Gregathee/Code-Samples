using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTool : Item
{
    [SerializeField] ToolType type;

    public ToolType GetToolType() { return type; }
}

public enum ToolType { None, WateringCan, Sickle, Hoe, MilkBucket}