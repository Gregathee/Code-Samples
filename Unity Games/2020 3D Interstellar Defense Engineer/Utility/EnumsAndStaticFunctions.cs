using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsAndStaticFunctions : MonoBehaviour
{
    public static string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789.-'";

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');
        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static Quaternion StringToQuaternion(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Quaternion result = new Quaternion(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]),
            float.Parse(sArray[3]));

        return result;
    }

    public static bool intToBool(int i)
    {
        if (i == 0) return false;
        else return true;
    }

    public static int boolToInt(bool i)
    {
        if (i) return 1;
        else return 0;
    }

    public static bool ValidInput(string input)
	{
        if (input.Contains(".txt")) return false;
        foreach(char c in input)
		{
            if (!validCharacters.Contains(c.ToString())) return false;
		}
        return true;
	}
}
public enum Priority { First, Last, Closest, Strongest }
public enum WeaknessPriority { Physical, Fire, Ice, Lightning, Poison, Stealth, Piercing, Ground, Flying, None }
public enum WeaknessTargetingLevel { Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
public enum TurretAngle { A0, A45, A90, A135, A180, A225, A270, A315, A360 }
public enum PartSize { Small, Medium, Large }
public enum InventoryContentType { ProjectileAmmo, SprayAmmo, ProjectileWeapon, Sprayer, Melee, AdvancedTargetingSystem, Tower }
public enum Accuracy { Level10, Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
public enum Recoil { Level10, Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
public enum TowerPartType { Base, Mount, MountStyle, MountSlot, Weapon, Ammo, TowerState }
public enum TargetPriority { First, Last, Strong, Close }
public enum TowerSize { Small, Medium, Large }

