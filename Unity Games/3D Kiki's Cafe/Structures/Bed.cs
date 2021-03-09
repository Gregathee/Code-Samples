using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] GameObject bedPrompt = null;
    public string GetName() { return name; }
    public void Interact(Player player)
    {
        bedPrompt.SetActive(true);
        bedPrompt.GetComponent<SelectableGrid>().Enable();
    }
}
