using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountContainer : MonoBehaviour
{
    [SerializeField] GameObject home = null;
    [SerializeField] GameObject rangeIndicator = null;
    [SerializeField] AngleIndicator angleIndicator = null;
    [SerializeField] TargetingBeam beam = null;
    public GameObject GetHome() { return home; }
    public GameObject GetRangeIndicator() { return rangeIndicator; }
    public AngleIndicator GetAngleIndicator() { return angleIndicator; }
    public TargetingBeam GetTargetingBeam() { return beam; }

    public void HideIndicators() 
    {
        rangeIndicator.SetActive(false);
        angleIndicator.gameObject.SetActive(false);
        beam.gameObject.SetActive(false);
    }
}
