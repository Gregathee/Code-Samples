using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIntersectingWall : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 20);
        if (hit.collider)
        {
            HideableWall wall = hit.collider.GetComponent<HideableWall>();
            if (wall)
            {
                wall.IsVisiable = false;
            }
        }
    }
}
