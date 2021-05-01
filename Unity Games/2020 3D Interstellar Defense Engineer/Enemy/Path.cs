using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints = null;
    float pathLength;

    private void Awake()
    {
        for(int i = wayPoints.Length-1; i > 0; i--)
        {
            pathLength += Vector3.Distance(wayPoints[i].position, wayPoints[i - 1].position);
        }
    }

    public Transform[] GetPath() { return wayPoints; }
    public float GetPathLength() { return pathLength; }
}
