using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingStation : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] List<Recipe> recipes = new List<Recipe>();    
    
    public virtual string GetName() { return name; }
    public void Interact(Player player) { player.GetRecipeBook().Open(recipes); }

    public List<Recipe> GetRecipes() { return recipes; }
}
