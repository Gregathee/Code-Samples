using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : TowerPart
{
    [SerializeField] int damage = 1;
    [SerializeField] int dot = 0;
    [SerializeField] int dotTime;
    [SerializeField] int dotTicsPerSec;
    [SerializeField] int penatration = 1;



    public int Penatration { get => penatration; set => penatration = value; }
    public int Damage { get => damage; set => damage = value; }
    public int Dot { get => dot; set => dot = value; }
    public int DotTime { get => dotTime; set => dotTime = value; }
    public int DotTicsPerSec { get => dotTicsPerSec; set => dotTicsPerSec = value; }
    [SerializeField] WeaknessPriority damageType = WeaknessPriority.None;

	public WeaknessPriority DamageType { get => damageType; set => damageType = value; }
}
