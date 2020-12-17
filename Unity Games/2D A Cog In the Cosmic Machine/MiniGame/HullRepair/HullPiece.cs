/*
 * HullPiece.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/17/2020 (en-US)
 * Description: Implements hull piece for hull repair mini game
 */

using UnityEngine;

public class HullPiece : MonoBehaviour
{
    public string[] PickupSFX;
    public string[] PutdownSFX;
    public string[] BumpSFX;

    Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (this == HullRepairMiniGame.selectedHullPiece)
        {
            collider.isTrigger = true;
            //Follow cursor
            Vector3 mousePosition;
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = -0.1f;
            transform.position = mousePosition;
        }
		else { collider.isTrigger = false; }
    }

	private void OnMouseDown()
    {
        Collider2D[] colliders = new Collider2D[1];
        if (this != HullRepairMiniGame.selectedHullPiece) { HullRepairMiniGame.selectedHullPiece = this; AudioManager.instance.PlaySFX(PickupSFX[Random.Range(0, PickupSFX.Length)]);}
        else
        {
            collider.OverlapCollider(new ContactFilter2D(), colliders);
            bool collidedWithHullPiece = false;
            foreach(Collider2D collider in colliders)
            {
				if (collider) { if (collider.CompareTag("Hull Piece")) { collidedWithHullPiece = true; AudioManager.instance.PlaySFX(BumpSFX[Random.Range(0, BumpSFX.Length)]); } }
            }
			if (!collidedWithHullPiece) { HullRepairMiniGame.selectedHullPiece = null; AudioManager.instance.PlaySFX(PutdownSFX[Random.Range(0, PutdownSFX.Length)]); }
        }
    }
}