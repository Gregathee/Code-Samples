using UnityEngine;

namespace TowerDefense.TowerParts.Ammo
{
	/// <summary>
	/// Base class for munitions
	/// </summary>
	public abstract class TP_Ammo : ColoredTowerPart
	{
		[SerializeField] protected WeaknessPriority damageType = WeaknessPriority.None;
		[SerializeField] protected int damage = 1;
		[SerializeField] protected int dot;
		[SerializeField] protected int dotTime;
		[SerializeField] protected int dotTicsPerSec;
		[SerializeField] protected int penetration = 1;

		public int Penetration { get => penetration; set => penetration = value; }
		public int Damage { get => damage; set => damage = value; }
		public int Dot { get => dot; set => dot = value; }
		public int DotTime { get => dotTime; set => dotTime = value; }
		public int DotTicsPerSec { get => dotTicsPerSec; set => dotTicsPerSec = value; }

		public WeaknessPriority DamageType { get => damageType; set => damageType = value; }
	}
}