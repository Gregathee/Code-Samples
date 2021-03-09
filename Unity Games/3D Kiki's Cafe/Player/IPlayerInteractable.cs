using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    string GetName();
    void Interact(Player player);
}
