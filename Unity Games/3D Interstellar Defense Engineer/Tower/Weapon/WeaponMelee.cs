using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    [SerializeField] int damage;
    [SerializeField] WeaknessPriority damageType;

    public int Damage { get => damage; set => damage = value; }
	public WeaknessPriority DamageType { get => damageType; set => damageType = value; }

	public override void Fire(Transform target)
    {
    }

}
