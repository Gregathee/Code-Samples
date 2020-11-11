using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] TowerState[] towerStates;
    [SerializeField] TowerModel towerModel;
    int price;
    int towerStateIndex;
    bool hasBeenPlaced;
    bool prioritizeAir;
    TowerSize size;
    TargetPriority priority;

    public void Upgrade() { }
    public void DownGrade() { }
    public void Place() { }

}
