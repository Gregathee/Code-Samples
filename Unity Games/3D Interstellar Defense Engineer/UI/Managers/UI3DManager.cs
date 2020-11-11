using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3DManager : MonoBehaviour
{
    public static UI3DManager instance;
    List<GameObject> UIElements = new List<GameObject>();

    private void Start(){ instance = this; }

    public void AddUIElement(GameObject element) 
    {
        element.GetComponent<TowerPart>().ActivateCamera(true);
        element.transform.localScale = new Vector3(1, 1, 1);
        UIElements.Add(element);
        PlaceElements();
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hi");
            foreach (GameObject g in UIElements) Debug.Log(g);
            Debug.Log("");
            foreach (TowerState state in InventoryManager.inventoryManager.GetTowerStates()) Debug.Log(state);
        }
	}
	void PlaceElements()
    {
        for(int i = 0; i < UIElements.Count;)
		{
            if (!UIElements[i]) { UIElements.RemoveAt(i); }
            else i++;
		}
        for(int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].transform.position = new Vector3(i * 30, i * -30, i * -30);
            UIElements[i].GetComponent<TowerPart>().GetView().depth = i;
        }
    }
}
