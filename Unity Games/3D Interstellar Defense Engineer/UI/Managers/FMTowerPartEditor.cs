using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMTowerPartEditor : MonoBehaviour
{
    public static TowerState root = null;
    public static int pathNum = 0;
    public static int pathIndex = 0;
    public static bool editIsPathed = false;
    public static int pathedSibIndex = -1;
    public static int rootSibIndex = -1;

    public static void StorePathedTowerInfo()
    {
        pathedSibIndex = FileManager.editedSlotSibIndex;
        TowerState pathedTower = FileManager.partToBeDeleted.GetComponent<TowerState>();
        int tempIndex = 0;
        switch (pathNum)
        {
            case 1:
                foreach (KeyValuePair<TowerState, string> pair in root.path1)  if (pair.Key == pathedTower) pathIndex = tempIndex; else tempIndex++;
                root.path1.RemoveAt(pathIndex);
                break;
            case 2:
                foreach (KeyValuePair<TowerState, string> pair in root.path2) if (pair.Key == pathedTower) pathIndex = tempIndex; else tempIndex++;
                root.path2.RemoveAt(pathIndex);
                break;
            case 3:
                foreach (KeyValuePair<TowerState, string> pair in root.path3) if (pair.Key == pathedTower) pathIndex = tempIndex; else tempIndex++;
                root.path3.RemoveAt(pathIndex);
                break;
        }
    }

    public static void EditTower(ref TowerPart part)
    {
        editIsPathed = part.GetComponent<TowerState>().IsPathed();
        if (part.GetComponent<TowerState>().IsPathed())
        {
            root = part.GetComponent<TowerState>().GetRoot();
            pathNum = part.GetComponent<TowerState>().GetPathNumber();
        }
        UIElements.baseSelector.JumpToPart(part.GetComponentInChildren<TowerBase>());
        UIElements.mountSelector.JumpToPart(part.GetComponentInChildren<WeaponMount>());
        UIElements.styleSelector.JumpToPart(part.GetComponentInChildren<WeaponMountStyle>());

        ButtonEventManager.SetCreateTowerStage();
        FMTowerPartPropertySetter.SetTowerPartProperties(part.customePartFilePath, FileManager.towerStateDir, false);

        TowerAssembler.towerAssembler.path1 = part.GetComponent<TowerState>().path1;
        TowerAssembler.towerAssembler.path2 = part.GetComponent<TowerState>().path2;
        TowerAssembler.towerAssembler.path3 = part.GetComponent<TowerState>().path3;

        UIElements.baseSizeDD.value = (int)part.GetComponentInChildren<TowerBase>().GetSize();
        UIElements.baseRotSpeedIP.text = part.GetComponentInChildren<WeaponMount>().RotationalSpeed.ToString();
        UIElements.baseRangeIP.text = part.GetComponentInChildren<WeaponMount>().Range.ToString();
        UIElements.baseTurnRadDD.value = (int)part.GetComponentInChildren<WeaponMount>().TurretAngle;
    }

    public static void EditWeapon(Weapon weapon)
    {
        weapon.SendPartToAssembler();
        if (weapon.GetComponent<WeaponProjectile>()) { EditProjectileWeapon(weapon.GetComponent<WeaponProjectile>()); }
        else if (weapon.GetComponent<WeaponSprayer>()) { EditSprayerWeapon(weapon.GetComponent<WeaponSprayer>()); }
        else if (weapon.GetComponent<WeaponMelee>()) { EditMeleeWeapon(weapon.GetComponent<WeaponMelee>()); }
        else if (weapon.GetComponent<AdvancedTargetingSystem>()) { EditTargetingSystem(weapon.GetComponent<AdvancedTargetingSystem>()); }
    }

    public static void EditProjectileWeapon(WeaponProjectile barrel)
    {
        UIElements.projectileWeaponSelector.JumpToPart(barrel);
        ButtonEventManager.SetCreateWeaponStage();
        UIElements.projWepSizeDD.value = (int)barrel.GetSize();
        UIElements.projWepRateIP.text = barrel.FireRate.ToString();
        UIElements.projWepRotSpeedIP.text = barrel.RotationSpeed.ToString();
        UIElements.projWepRecoilDD.value = (int)barrel.Recoil;
        UIElements.projWepAccuracyDD.value = (int)barrel.Accuracy;
        UIElements.projWepTargetAirTog.isOn = barrel.CanShootUp;
        UIElements.projWepTargetGroundTog.isOn = barrel.CanShootDown;
        Ammo ammo = barrel.GetAmmo();
        TowerAssembler.towerAssembler.LoadAmmoSlot(ref ammo);
    }

    public static void EditSprayerWeapon(WeaponSprayer sprayer)
    {
        UIElements.sprayerWeaponSelector.JumpToPart(sprayer);
        ButtonEventManager.SetCreateSprayerWeaponStage();
        UIElements.sprayWepSizeDD.value = (int)sprayer.GetSize();
        UIElements.sprayWepRateIP.text = sprayer.FireRate.ToString();
        UIElements.sprayWepTargetAirTog.isOn = sprayer.CanShootUp;
        UIElements.sprayWepTargetGroundTog.isOn = sprayer.CanShootDown;
        Ammo ammo = sprayer.GetAmmo();
        TowerAssembler.towerAssembler.LoadAmmoSlot(ref ammo);
    }

    public static void EditMeleeWeapon(WeaponMelee melee)
    {
        UIElements.meleeSelector.JumpToPart(melee);
        ButtonEventManager.SetCreateMeleeWeaponStage();
        UIElements.meleeWepSizeDD.value = (int)melee.GetSize();
        UIElements.meleeWepDamageIP.text = melee.Damage.ToString();
        UIElements.meleeDamageTypeDD.value = (int)melee.DamageType;
    }

    public static void EditTargetingSystem(AdvancedTargetingSystem targetingSystem)
    {
        UIElements.targetingSelector.JumpToPart(targetingSystem);
        ButtonEventManager.SetCreateTargetingWeaponStage();
        UIElements.targetSizeDD.value = (int)targetingSystem.GetSize();
        UIElements.targetWeaknessLevelDD.value = (int)targetingSystem.WeaknessTargetingLevel;
        UIElements.targetBasicTog.isOn = targetingSystem.MovementTypePriorityTargeting;
        UIElements.targetAdvancedTog.isOn = targetingSystem.AdvancedPositionPriorityTarget;
        UIElements.targetDetectStealthTog.isOn = targetingSystem.CanDetectStealth;
    }

    public static void EditAmmo(Ammo ammo)
    {
        Projectile projectile = ammo.GetComponent<Projectile>();
        Spray spray = ammo.GetComponent<Spray>();
        if (projectile) { EditProjectile(projectile); }
        else if (spray) { EditSpray(spray); }
        ammo.SendPartToAssembler();
    }

    public static void EditProjectile(Projectile projectile)
    {
        UIElements.projectileSelector.JumpToPart(projectile);
        ButtonEventManager.SetCreateProjectileAmmoStage();
        UIElements.projAmmoDamageIP.text = projectile.Damage.ToString();
        UIElements.projAmmDOTIP.text = projectile.Dot.ToString();
        UIElements.projAmmDOTDurIP.text = projectile.DotTime.ToString();
        UIElements.projAmmDOTRateIP.text = projectile.DotTicsPerSec.ToString();
        UIElements.projAmmPenIP.text = projectile.Penatration.ToString();
        UIElements.projAmmSpeedIP.text = projectile.Speed.ToString();
        UIElements.projAmmLifeIP.text = projectile.TravelTime.ToString();
        UIElements.projRadIP.text = projectile.AOE.ToString();
        UIElements.projHomIP.text = projectile.Homing.ToString();
        UIElements.projDamTypeDD.value = (int)projectile.DamageType;
    }

    public static void EditSpray(Spray spray)
    {
        UIElements.spraySelector.JumpToPart(spray);
        ButtonEventManager.SetCreateSprayAmmoStage();
        UIElements.sprayAmmoDamageIP.text = spray.Damage.ToString();
        UIElements.sprayAmmDOTIP.text = spray.Dot.ToString();
        UIElements.sprayAmmDOTDurIP.text = spray.DotTime.ToString();
        UIElements.sprayAmmDOTRateIP.text = spray.DotTicsPerSec.ToString();
        UIElements.sprayAmmPenIP.text = spray.Penatration.ToString();
        UIElements.sprayDamTypeDD.value = (int)spray.DamageType;
    }
}
