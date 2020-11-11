using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitOnDeath : MonoBehaviour
{
    [SerializeField]Enemy enemy;
    public void Split(Collider2D collider)
    {
        Instantiate(enemy, transform.position, new Quaternion()).PushBack(ref collider);
        Instantiate(enemy, transform.position, new Quaternion()).PushBack(ref collider);
    }
}
