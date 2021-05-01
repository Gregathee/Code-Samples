/*
 * HullGridSquare.cs
 * Author(s): #Greg Brandt#
 * Created on: 12/1/2020 (en-US)
 * Description: Used to to indicate whether a hull piece is overlaping
 */

using UnityEngine;
using UnityEngine.UI;

public class HullGridSquare : MonoBehaviour
{
    Collider2D collider;
    Image image;
    bool isCovered = false;
    HullRepairMiniGame miniGameManager;

	private void Start()
	{
        collider = GetComponent<Collider2D>();
        image = GetComponent<Image>();
        miniGameManager = FindObjectOfType<HullRepairMiniGame>();
	}
	private void Update()
	{
        Collider2D[] colliders = new Collider2D[1];
        collider.OverlapCollider(new ContactFilter2D(), colliders);
        bool collidedWithHullPiece = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider) { if (collider.CompareTag("Hull Piece")) { collidedWithHullPiece = true;} }
        }
        if (collidedWithHullPiece) { image.color = miniGameManager.CoveredColor(); isCovered = true; }
        else { image.color = miniGameManager.UncoveredColor(); isCovered = false; }
    }

    public bool IsCovered() { return isCovered; }
}