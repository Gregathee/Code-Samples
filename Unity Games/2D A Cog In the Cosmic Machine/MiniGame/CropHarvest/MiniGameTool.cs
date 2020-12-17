/*
 * MiniGameTool.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/15/2020 (en-US)
 * Description: Makes tool follow cursor when clicked and changes the states of crops
 */

using UnityEngine;
using UnityEngine.UI;

public class MiniGameTool : MonoBehaviour
{
    public MiniGameToolType toolType = MiniGameToolType.Clippers;
    Vector3 originalPosition = Vector3.zero;
    bool isBeingDraged = false;
    int originalLayer = 0;
    Camera cam;

    void Start()
    {
        originalLayer = gameObject.layer;
        originalPosition = transform.position;
        cam = Camera.main;
    }

    void Update()
    {
        if (isBeingDraged)
        {
            //Play water particle effect if watering can
            if(toolType == MiniGameToolType.WateringCan) 
            {
				if (!GetComponentInChildren<ParticleSystem>().isPlaying) { GetComponentInChildren<ParticleSystem>().Play(); }
            }
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = cam.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
            transform.SetSiblingIndex(transform.parent.childCount -1);
        }
		else
		{
            //Stop playing water particle effect if watering can
            if (toolType == MiniGameToolType.WateringCan)
            {
                if (GetComponentInChildren<ParticleSystem>().isPlaying) { GetComponentInChildren<ParticleSystem>().Stop(); }
            }
        }
        //If tool was dropped, stop following cursor.
        if(CropHarvestMiniGame.selectedTool != this) 
        {
            isBeingDraged = false;
            transform.position = originalPosition;
            gameObject.layer = originalLayer;
        }
        //Drop tool
        if (Input.GetMouseButtonDown(1))
        {
            CropHarvestMiniGame.selectedTool = null;
        }
    }

    private void OnMouseDown() 
    {
        //pick up tool
            isBeingDraged = true;
            CropHarvestMiniGame.selectedTool = this;
            //change layer to avoid interacting with background
            gameObject.layer = 2;
    }
}

public enum MiniGameToolType { WateringCan, Fertilizer, Clippers, Seed }