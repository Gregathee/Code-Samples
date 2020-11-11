using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For enemies that don't move
public class DontMove : MovementDirective
{
    public override void Move() { }

    public override void StopCharge() { }
}
