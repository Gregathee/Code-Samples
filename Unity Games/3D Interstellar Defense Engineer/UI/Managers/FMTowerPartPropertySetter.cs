using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Text;
using TMPro;
using System.Runtime.CompilerServices;

public class FMTowerPartPropertySetter : MonoBehaviour
{
    public static void SetTowerPartProperties(string path, string partDirectory, bool confirm)
    {
        StreamReaderPro partReader = new StreamReaderPro(path);
        GameObject loadedTowerPart = null;
        if (partDirectory != FileManager.towerStateDir)
        {
            loadedTowerPart = Instantiate(Resources.Load(partReader.ReadLine()) as GameObject);
            ReadPartColors(ref partReader, ref loadedTowerPart);
        }
        else loadedTowerPart = new GameObject();
        loadedTowerPart.name = partReader.ReadLine();

        if (partDirectory == FileManager.projectileAmmoDir) { SetProjectileAmmoProperties(ref partReader, ref loadedTowerPart); }
        else if (partDirectory == FileManager.sprayAmmoDir) { SetSprayAmmoProperties(ref partReader, ref loadedTowerPart); }
        else if (loadedTowerPart.GetComponent<Weapon>()) SetWeaponProperties(ref partReader, ref loadedTowerPart);
        else if (partDirectory == FileManager.towerStateDir) { SetTowerStateProperties(ref partReader, ref loadedTowerPart,  confirm); }
        if (loadedTowerPart.GetComponent<TowerPart>())
        {
            loadedTowerPart.GetComponent<TowerPart>().SetIsPreview(false);
            loadedTowerPart.GetComponent<TowerPart>().SetHide(false);
            loadedTowerPart.GetComponent<TowerPart>().SetShrink(false);
        }
        if(partDirectory != FileManager.towerStateDir) InventoryManager.inventoryManager.AddPart(false, loadedTowerPart.GetComponent<TowerPart>());
        Destroy(loadedTowerPart);
    }

    public static void ReadPartColors(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
	{
        Vector3 c1 = EnumsAndStaticFunctions.StringToVector3(partReader.ReadLine());
        Vector3 c2 = EnumsAndStaticFunctions.StringToVector3(partReader.ReadLine());
        Vector3 c3 = EnumsAndStaticFunctions.StringToVector3(partReader.ReadLine());
        Material m1 = new Material(loadedTowerPart.GetComponent<TowerPart>().mat1);
        Material m2 = new Material(loadedTowerPart.GetComponent<TowerPart>().mat2);
        Material m3 = new Material(loadedTowerPart.GetComponent<TowerPart>().mat3);
        if (loadedTowerPart.GetComponent<Spray>())
        {
            m1.color = new Color(c1.x, c1.y, c1.z, 0.5f);
            m2.color = new Color(c2.x, c2.y, c2.z, 0.5f);
            m3.color = new Color(c3.x, c3.y, c3.z, 0.5f);
        }
        else
        {
            m1.color = new Color(c1.x, c1.y, c1.z, 1);
            m2.color = new Color(c2.x, c2.y, c2.z, 1);
            m3.color = new Color(c3.x, c3.y, c3.z, 1);
        }

        loadedTowerPart.GetComponent<TowerPart>().SetMaterials(m1, m2, m3);
    }

    public static void SetProjectileAmmoProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        loadedTowerPart.GetComponent<Projectile>().Damage = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().Dot = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().DotTime = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().DotTicsPerSec = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().Penatration = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().Speed = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().TravelTime = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().AOE = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().Homing = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Projectile>().DamageType = (WeaknessPriority)int.Parse(partReader.ReadLine());
    }

    public static void SetSprayAmmoProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        loadedTowerPart.GetComponent<Spray>().Damage = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Spray>().Dot = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Spray>().DotTime = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Spray>().DotTicsPerSec = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Spray>().Penatration = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<Spray>().DamageType = (WeaknessPriority)int.Parse(partReader.ReadLine());
    }

    public static void SetWeaponProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        if (loadedTowerPart.GetComponent<WeaponProjectile>()) SetProjectileWeaponProperties(ref partReader, ref loadedTowerPart);
        else if (loadedTowerPart.GetComponent<WeaponSprayer>()) SetSprayWeaponProperties(ref partReader, ref loadedTowerPart);
        else if (loadedTowerPart.GetComponent<WeaponMelee>())
        {
            loadedTowerPart.GetComponent<WeaponMelee>().Damage = int.Parse(partReader.ReadLine());
            loadedTowerPart.GetComponent<WeaponMelee>().DamageType = (WeaknessPriority)int.Parse(partReader.ReadLine());
        }
        else if (loadedTowerPart.GetComponent<AdvancedTargetingSystem>()) SetTargetingSystemProperties(ref partReader, ref loadedTowerPart);
        loadedTowerPart.GetComponent<Weapon>().SetSize((PartSize)int.Parse(partReader.ReadLine()));
    }

    public static void SetProjectileWeaponProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        string ammoPath = partReader.ReadLine();
        StreamReaderPro ammoReader = new StreamReaderPro(ammoPath);
        GameObject projectile = Instantiate(Resources.Load(ammoReader.ReadLine()) as GameObject);
        ReadPartColors(ref ammoReader, ref projectile);
        projectile.name = ammoReader.ReadLine();
        projectile.GetComponent<Projectile>().customePartFilePath = FileManager.rootDir + FileManager.projectileAmmoDir + projectile.name + ".txt";
        Projectile ammoReference = null;
        foreach(Projectile ammo in InventoryManager.projectileAmmunition) { if(ammo.name == projectile.name) { ammoReference = ammo; } }
        loadedTowerPart.GetComponent<WeaponProjectile>().SetAmmo(ammoReference);
        Destroy(projectile.gameObject);

        loadedTowerPart.GetComponent<WeaponProjectile>().FireRate = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<WeaponProjectile>().RotationSpeed = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<WeaponProjectile>().Recoil = (Recoil)int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<WeaponProjectile>().Accuracy = (Accuracy)int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<WeaponProjectile>().CanShootUp = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
        loadedTowerPart.GetComponent<WeaponProjectile>().CanShootDown = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
    }

    public static void SetSprayWeaponProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        string ammoPath = partReader.ReadLine();
        StreamReaderPro ammoReader = new StreamReaderPro(ammoPath);
        GameObject spray = Instantiate(Resources.Load(ammoReader.ReadLine()) as GameObject);
        ReadPartColors(ref ammoReader, ref spray);
        spray.name = ammoReader.ReadLine();
        spray.GetComponent<Spray>().customePartFilePath = FileManager.rootDir + FileManager.sprayAmmoDir + spray.name + ".txt";
        Spray ammoReference = null;
        foreach (Spray ammo in InventoryManager.projectileAmmunition) { if (ammo.name == spray.name) { ammoReference = ammo; } }
        loadedTowerPart.GetComponent<WeaponSprayer>().SetAmmo(ammoReference);
        Destroy(spray.gameObject);

        loadedTowerPart.GetComponent<WeaponSprayer>().FireRate = int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<WeaponSprayer>().CanShootUp = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
        loadedTowerPart.GetComponent<WeaponSprayer>().CanShootDown = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
    }

    public static void SetTargetingSystemProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart)
    {
        loadedTowerPart.GetComponent<AdvancedTargetingSystem>().WeaknessTargetingLevel = (WeaknessTargetingLevel)int.Parse(partReader.ReadLine());
        loadedTowerPart.GetComponent<AdvancedTargetingSystem>().MovementTypePriorityTargeting = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
        loadedTowerPart.GetComponent<AdvancedTargetingSystem>().AdvancedPositionPriorityTarget = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
        loadedTowerPart.GetComponent<AdvancedTargetingSystem>().CanDetectStealth = EnumsAndStaticFunctions.intToBool(int.Parse(partReader.ReadLine()));
    }

    public static void SetTowerStateProperties(ref StreamReaderPro partReader, ref GameObject loadedTowerPart, bool confirm)
    {
        UIElements.ConstructionTimeIP.text = partReader.ReadLine();
        GameObject towerBase = Instantiate(Resources.Load(partReader.ReadLine()) as GameObject);
        ReadPartColors(ref partReader, ref towerBase);
        towerBase.GetComponent<TowerBase>().SendPartToAssembler();
        PartSize size = (PartSize)int.Parse(partReader.ReadLine());

        GameObject mount = Instantiate(Resources.Load(partReader.ReadLine()) as GameObject);
        ReadPartColors(ref partReader, ref mount);
        (Resources.Load(partReader.ReadLine()) as GameObject).GetComponent<WeaponMountStyle>().SendPartToAssembler();
        SetMountProperties(ref partReader, ref mount);

        TowerAssembler.towerAssembler.ChangeBaseSize(size);

        List<KeyValuePair<TowerState, string>> path1 = new List<KeyValuePair<TowerState, string>>();
        List<KeyValuePair<TowerState, string>> path2 = new List<KeyValuePair<TowerState, string>>();
        List<KeyValuePair<TowerState, string>> path3 = new List<KeyValuePair<TowerState, string>>();
        SetTowerPaths(ref partReader, path1, path2, path3);
        TowerAssembler.towerAssembler.path1 = path1;
        TowerAssembler.towerAssembler.path2 = path2;
        TowerAssembler.towerAssembler.path3 = path3;
        List<string> weaponPaths = new List<string>();
        List<string> weaponRotations = new List<string>();
        while (!partReader.EndOfStream()) { weaponPaths.Add(partReader.ReadLine()); weaponRotations.Add(partReader.ReadLine()); }
        SetTowerWeaponProperties(weaponPaths, weaponRotations);

        //Load tower parts to edit into assembler 
        if(confirm)TowerAssembler.towerAssembler.ConfirmTower(false, loadedTowerPart.name, false);
        Destroy(towerBase);
        Destroy(mount);
    }

    static void SetMountProperties(ref StreamReaderPro partReader, ref GameObject mount)
	{
        mount.GetComponent<WeaponMount>().SendPartToAssembler();
        mount.GetComponent<WeaponMount>().RotationalSpeed = int.Parse(partReader.ReadLine());
        mount.GetComponent<WeaponMount>().Range = int.Parse(partReader.ReadLine());
        mount.GetComponent<WeaponMount>().TurretAngle = (TurretAngle)int.Parse(partReader.ReadLine());
    }

    static void SetTowerPaths
        (
        ref StreamReaderPro partReader, 
        List<KeyValuePair<TowerState, string>> path1, 
        List<KeyValuePair<TowerState, string>> path2, 
        List<KeyValuePair<TowerState, string>> path3
        )
	{
        if (partReader.PeakLine() == "Start Path1")
        {
            partReader.ReadLine();
            while (partReader.PeakLine() != "End Path1") path1.Add(new KeyValuePair<TowerState, string>(null, partReader.ReadLine()));
            partReader.ReadLine();
        }
        if (partReader.PeakLine() == "Start Path2")
        {
            partReader.ReadLine();
            while (partReader.PeakLine() != "End Path2") path2.Add(new KeyValuePair<TowerState, string>(null, partReader.ReadLine()));
            partReader.ReadLine();
        }
        if (partReader.PeakLine() == "Start Path3")
        {
            partReader.ReadLine();
            while (partReader.PeakLine() != "End Path3") path3.Add(new KeyValuePair<TowerState, string>(null, partReader.ReadLine()));
            partReader.ReadLine();
        }
    }

    public static void SetTowerWeaponProperties(List<string> weaponPaths, List<string> weaponRotations)
    {
        for (int i = 0; i < weaponPaths.Count; i++)
        {
            StreamReaderPro weaponReader = new StreamReaderPro(weaponPaths[i]);
            GameObject weaponObject = Instantiate(Resources.Load(weaponReader.ReadLine()) as GameObject);
            weaponObject.GetComponent<Weapon>().savedLocalRotation = EnumsAndStaticFunctions.StringToQuaternion(weaponRotations[i]);
            ReadPartColors(ref weaponReader, ref weaponObject);
            weaponObject.name = weaponReader.ReadLine();
            weaponObject.GetComponent<Weapon>().customePartFilePath = weaponPaths[i];
            SetWeaponProperties(ref weaponReader, ref weaponObject);
            TowerAssembler.towerAssembler.AttachedWeapon(weaponObject.GetComponent<Weapon>(), i, 22.6f, false);
            Destroy(weaponObject);
        }
        while (TowerAssembler.towerAssembler.WeaponsTouching()) TowerAssembler.towerAssembler.ChangeBaseSize((PartSize)((int)TowerAssembler.towerAssembler.GetPartSize() + 1));
    }
    
    public static void SaveWeaponProperties(ref StreamWriter writer, Weapon weapon)
    {
        WeaponProjectile weaponBarrel = weapon.GetComponent<WeaponProjectile>();
        WeaponSprayer weaponSprayer = weapon.GetComponent<WeaponSprayer>();
        WeaponMelee melee = weapon.GetComponent<WeaponMelee>();
        AdvancedTargetingSystem targetingSystem = weapon.GetComponent<AdvancedTargetingSystem>();
        if (weaponBarrel) { Debug.Log(weaponBarrel.GetAmmo().customePartFilePath); SaveProjectileWeaponProperties(ref writer, weaponBarrel); }
        else if (weaponSprayer) { SaveSprayerProperties(ref writer, weaponSprayer); }
        else if (melee) 
        {
            writer.WriteLine(melee.Damage);
            writer.WriteLine((int)melee.DamageType);
        }
        else if (targetingSystem) { SaveTargetingProperties(ref writer, targetingSystem); }
    }

    public static void SaveAmmoProperties(ref StreamWriter writer, Ammo ammo)
    {
        Projectile projectile = ammo.GetComponent<Projectile>();
        Spray spray = ammo.GetComponent<Spray>();
        if (projectile) SaveProjectileProperties(ref writer, projectile);
        else if (spray) SaveSprayProperties(ref writer, spray);
    }

    public static void SaveProjectileWeaponProperties(ref StreamWriter writer, WeaponProjectile barrel)
    {
        Debug.Log(barrel.GetAmmo().customePartFilePath);
        writer.WriteLine(barrel.GetAmmo().customePartFilePath);
        writer.WriteLine(barrel.FireRate);
        writer.WriteLine(barrel.RotationSpeed);
        writer.WriteLine((int)barrel.Recoil);
        writer.WriteLine((int)barrel.Accuracy);
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(barrel.CanShootUp));
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(barrel.CanShootDown));
    }

    public static void SaveSprayerProperties(ref StreamWriter writer, WeaponSprayer sprayer)
    {
        writer.WriteLine(sprayer.GetAmmo().customePartFilePath);
        writer.WriteLine(sprayer.FireRate);
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(sprayer.CanShootUp));
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(sprayer.CanShootDown));
    }

    public static void SaveTargetingProperties(ref StreamWriter writer, AdvancedTargetingSystem targetingSystem)
    {
        writer.WriteLine((int)targetingSystem.WeaknessTargetingLevel);
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(targetingSystem.MovementTypePriorityTargeting));
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(targetingSystem.AdvancedPositionPriorityTarget));
        writer.WriteLine(EnumsAndStaticFunctions.boolToInt(targetingSystem.CanDetectStealth));
    }

    public static void SaveProjectileProperties(ref StreamWriter writer, Projectile ammo)
    {
        writer.WriteLine(ammo.Damage);
        writer.WriteLine(ammo.Dot);
        writer.WriteLine(ammo.DotTime);
        writer.WriteLine(ammo.DotTicsPerSec);
        writer.WriteLine(ammo.Penatration);
        writer.WriteLine(ammo.Speed);
        writer.WriteLine(ammo.TravelTime);
        writer.WriteLine(ammo.AOE);
        writer.WriteLine(ammo.Homing);
        writer.WriteLine((int)ammo.DamageType);
    }

    public static void SaveSprayProperties(ref StreamWriter writer, Spray ammo)
    {
        writer.WriteLine(ammo.Damage);
        writer.WriteLine(ammo.Dot);
        writer.WriteLine(ammo.DotTime);
        writer.WriteLine(ammo.DotTicsPerSec);
        writer.WriteLine(ammo.Penatration);
        writer.WriteLine((int)ammo.DamageType);
    }

    public static void SaveMountProperties(ref StreamWriter writer, WeaponMount mount)
    {
        writer.WriteLine(mount.RotationalSpeed);
        writer.WriteLine(mount.Range);
        writer.WriteLine((int)mount.TurretAngle);
    }

    public static void SaveTowerPaths(ref StreamWriter writer, TowerState state)
    {
        if(state.path1.Count > 0)
		{
            int i = 0;
            writer.WriteLine("Start Path1");
            foreach (KeyValuePair<TowerState, string> pair in state.path1) 
            {
                if (pair.Key) writer.WriteLine(pair.Key.customePartFilePath);
                else Debug.Log(i.ToString() + state);
                i++;
            }
            writer.WriteLine("End Path1");
        }
        if(state.path2.Count > 0)
        {
            int i = 0;
            writer.WriteLine("Start Path2");
            foreach (KeyValuePair<TowerState, string> pair in state.path2)
            {
                if (pair.Key) writer.WriteLine(pair.Key.customePartFilePath);
                else Debug.Log(i.ToString() + state);
                i++;
            }
            writer.WriteLine("End Path2");
        }
        if (state.path3.Count > 0)
        {
            int i = 0;
            writer.WriteLine("Start Path3");
            foreach (KeyValuePair<TowerState, string> pair in state.path3)
            {
                if (pair.Key) writer.WriteLine(pair.Key.customePartFilePath);
                else Debug.Log(i.ToString() + state);
                i++;
            }
            writer.WriteLine("End Path3");
        }
    }
    //tower state file formate
    //  name
    // construction cost
    //  basePrefabFilePath
    // base color 1
    // base color 2
    // base color 3
    //  towerBaseSize
    //  mountPrefabFilePath
    // mount color 1
    // mount color 2
    // mount color 3
    //  stylePrefabFilePath
    //  rotationSpeed
    //  range
    //  turretAngle
    // start path1 list
    // paths
    // end path1 List
    //start path2 list
    // paths
    // end path2 List
    //start path3 list
    // paths
    // end path3 List
    //  weaponPrefabFile
    //  weaponRotation
}
