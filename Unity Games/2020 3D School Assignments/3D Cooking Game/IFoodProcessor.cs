/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Interfaces Food Processors
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodProcessorType { Stove, CuttingBoard, OrderupCounter}
public interface IFoodProcessor 
{
    FoodProcessorType FoodProcessorType();
    void ProcessFood();
    GameObject GameObject();
}
