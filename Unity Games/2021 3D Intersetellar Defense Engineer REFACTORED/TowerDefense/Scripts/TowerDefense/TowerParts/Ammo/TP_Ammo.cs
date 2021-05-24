using System.IO;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Ammo
{
	/// <summary>
	/// Base class for munitions
	/// </summary>
	public abstract class TP_Ammo : ColoredTowerPart
	{
		[SerializeField] protected WeaknessPriority damageType = WeaknessPriority.None;
		[SerializeField] protected int _damage = 1;
		[SerializeField] protected int _dot;
		[SerializeField] protected int _dotTime;
		[SerializeField] protected int _dotTicsPerSec;
		[SerializeField] protected int _penetration = 1;

		public int Penetration { get => _penetration; set => _penetration = value; }
		public int Damage { get => _damage; set => _damage = value; }
		public int Dot { get => _dot; set => _dot = value; }
		public int DotTime { get => _dotTime; set => _dotTime = value; }
		public int DotTicsPerSec { get => _dotTicsPerSec; set => _dotTicsPerSec = value; }

		public WeaknessPriority DamageType { get => damageType; set => damageType = value; }

	}
}