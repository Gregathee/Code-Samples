using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object for an animation event to refer to after the player of boss finishes a sword swing
public class StopPlayerSwingingSword : MonoBehaviour
{
    [SerializeField] Entity entity = null;
    [SerializeField] BossShootPlayer bossShootPlayer;

    public void StopSwingingSword() { entity.StopSwimingSword(); }

    public void StopSpining()
    {
        entity.StopSwimingSword();
        King king = entity.GetComponent<King>();
        if(king != null)
        {
            king.StopSpining();
        }
    }

    public void Shoot()
    {
        bossShootPlayer.Teleport();
    }
}
