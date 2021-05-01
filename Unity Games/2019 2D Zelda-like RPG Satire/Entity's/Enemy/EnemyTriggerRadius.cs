using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used to make enemy colliders solid and triggers. I later realized I didn't need to do this.
public class EnemyTriggerRadius : MonoBehaviour
{
    Enemy enemy;
    void Start(){enemy = GetComponentInParent<Enemy>(); }
    public Enemy GetEnemy() { return enemy; }
}
