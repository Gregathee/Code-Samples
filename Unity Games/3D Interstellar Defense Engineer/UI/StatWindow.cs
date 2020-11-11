using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatWindow : MonoBehaviour
{
	public static StatWindow statWindow;
    [SerializeField] TMP_Text statText = null;
	bool clearText;

	private void Start() { statWindow = this; }

	private void Update()
	{
		if (clearText) statText.text = "";
	}

	public void ClearStats() { clearText = true; ; }

	public void DisplayTowerStats(TowerState state)
	{
		statText.text = "";
		clearText = false;
		if (state.IsPathed()) statText.text += "-Pathed Tower" + "\n\n";
		if (state.isRoot) statText.text += "-Root Tower" + "\n\n";
		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateTowerCost(state) + "\n\n"; 
		statText.text += "-Construction Time " + state.startingConstructionTime + "\n\n";
		statText.text += "-Size: " + state.GetComponentInChildren<TowerBase>().GetSize() + "\n\n";
		statText.text += "-Rotatates " + state.GetComponentInChildren<WeaponMount>().RotationalSpeed.ToString() + " Degrees Per Second.\n\n";
		statText.text += "-Targets Units Up To  " + state.GetComponentInChildren<WeaponMount>().Range.ToString() + " Meters.\n\n";
		statText.text += "-Can Rotate " + ((int)state.GetComponentInChildren<WeaponMount>().TurretAngle * 45).ToString() + " Degrees.\n\n";
		statText.text += "Equiped Weapons: \n";
		foreach (WeaponMountSlot slot in state.GetComponentInChildren<WeaponMountStyle>().GetSlots()) { statText.text += ( "    " + slot.GetWeapon().name + "\n").Replace("(Clone)", "");  }
	}

	public void DisplayProjectileWeaponStats(WeaponProjectile barrel)
	{
		statText.text = "";
		clearText = false;
		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateWeaponCost(barrel) + "\n\n";
		statText.text += "-Size: " + barrel.GetSize() + "\n\n";
		statText.text += "-Fires " + barrel.FireRate.ToString() + " Shots Per Second." + "\n\n"; 
		statText.text += "-Rotates Vertically  " + barrel.RotationSpeed.ToString() + " Degrees Per Second.\n\n";
		statText.text += "-Recoils " + ((float)barrel.Recoil * 2.5f)+ " Degrees Per Shot.\n\n";
		statText.text += "-Projectile Can Deviate Trajectory Up To " + ((float)barrel.Accuracy * 2.5f/2)+ " Degrees.\n\n";
		if(barrel.CanShootUp) statText.text += "-Can Target Air Units." + "\n\n";
		else statText.text += "-Cannot Target Air Units." + "\n\n";
		if (barrel.CanShootDown) statText.text += "-Can Target Ground Units." + "\n\n";
		else statText.text += "-Cannot Target Ground Units." + "\n\n";
		statText.text += "Ammunition: " + barrel.GetAmmo().name;
	}

	public void DisplaySprayerWeaponStats(WeaponSprayer sprayer)
	{
		statText.text = "";
		clearText = false;

		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateWeaponCost(sprayer) + "\n\n";
		statText.text += "-Size: " + sprayer.GetSize() + "\n\n";
		statText.text += "-Emits " + sprayer.FireRate.ToString() + "-Times Per Second.\n\n";
		if (sprayer.CanShootUp) statText.text += "-Can Target Air Units." + "\n\n";
		else statText.text += "-Cannot Target Air Units." + "\n\n";
		if (sprayer.CanShootDown) statText.text += "-Can Target Ground Units." + "\n\n";
		else statText.text += "-Cannot Target Ground Units." + "\n\n";
		statText.text += "Ammunition: " + sprayer.GetAmmo().name;
	}

	public void DisplayMeleeWeaponStats(WeaponMelee melee)
	{
		statText.text = "";
		clearText = false;

		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateWeaponCost(melee) + "\n\n";
		statText.text += "-Size: " + melee.GetSize() + "\n\n";
		statText.text += "-Deals " + melee.Damage.ToString() + " Points Of " + melee.DamageType + " Damage.\n\n";
	}

	public void DisplayTargetingSystemStats(AdvancedTargetingSystem targetingSystem)
	{
		statText.text = "";
		clearText = false;

		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateWeaponCost(targetingSystem) + "\n\n";
		statText.text += "-Size: " + targetingSystem.GetSize() + "\n\n";
		if (targetingSystem.MovementTypePriorityTargeting) statText.text += "-Can Prioritize Targets Based on Position and Strength." + "\n\n";
		if (targetingSystem.AdvancedPositionPriorityTarget) { statText.text += "-Can Prioritize " + ((int)targetingSystem.WeaknessTargetingLevel).ToString() + " Types Of Weaknesses.\n\n"; }
		else statText.text += "-Cannot Prioritize Target Weaknesses.\n\n";
		if (targetingSystem.CanDetectStealth) statText.text += "-Can Detect Stealthed Units." + "\n\n";
		else statText.text += "-Cannot Detect Stealthed Units." + "\n\n";
	}

	public void DisplayProjectileStats(Projectile projectile)
	{
		statText.text = "";
		clearText = false;
		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateAmmoCost(projectile) + "\n\n";
		statText.text += "-Deals " + projectile.Damage.ToString() + " Points of " + projectile.DamageType + " Damage On Impact.\n\n";
		statText.text += "-Deals Additional Damage Over " + projectile.DotTime.ToString() + " Seconds." + "\n\n";
		statText.text += "-Deals " + projectile.Dot.ToString() + " Damage Per Tic.\n\n";
		statText.text += "-Damage Is Applied " + projectile.DotTicsPerSec.ToString() + " Times Per Second.\n\n";
		statText.text += "-Penatrates " + projectile.Penatration.ToString() + " Targets.\n\n";
		statText.text += "-Travels " + projectile.Speed.ToString() + " Meters Per Second.\n\n";
		statText.text += "-Travels for " + projectile.TravelTime.ToString() + " Seconds.\n\n";
		statText.text += "-Has A Blast Radius Of " + projectile.AOE.ToString() + " Meters.\n\n";
		statText.text += "-Can Change Trajectory " + projectile.Homing.ToString() + " Degrees Per Second Towards Initial Target.\n\n";
	}

	public void DisplaySprayStats(Spray spray)
	{
		statText.text = "";
		clearText = false;

		statText.text += "-Construction Cost " + ConstructionCostCalculator.CalculateAmmoCost(spray) + "\n\n";
		statText.text += "-Deals " + spray.Damage.ToString() + " Points Of " + spray.DamageType + " Damage.\n\n";
		statText.text += "-Deals Additional Damage Over " + spray.DotTime.ToString() + " Seconds." + "\n\n";
		statText.text += "-Deals " + spray.Dot.ToString() + " Damage Per Tic.\n\n";
		statText.text += "-Damage is applied " + spray.DotTicsPerSec.ToString() + " Times Per Second.\n\n";
		statText.text += "-Penatrates " + spray.Penatration.ToString() + " Targets.\n\n";
	}
}
