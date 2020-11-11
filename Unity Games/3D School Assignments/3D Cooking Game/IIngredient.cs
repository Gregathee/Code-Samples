/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Interfaces Ingredients
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIngredient 
{
    bool Complete();
    GameObject GameObject();
    void Process(IFoodProcessor foodProcessor);
    void Destroy();
}
