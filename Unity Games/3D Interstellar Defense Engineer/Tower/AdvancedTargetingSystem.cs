using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedTargetingSystem : Weapon
{
    [SerializeField] WeaknessTargetingLevel weaknessTargetingLevel = WeaknessTargetingLevel.Level1;
    [SerializeField] WeaknessPriority weaknessPriority1 = WeaknessPriority.Physical;
    [SerializeField] WeaknessPriority weaknessPriority2 = WeaknessPriority.Fire;
    [SerializeField] WeaknessPriority weaknessPriority3 = WeaknessPriority.Ice;
    [SerializeField] WeaknessPriority weaknessPriority4 = WeaknessPriority.Lightning;
    [SerializeField] WeaknessPriority weaknessPriority5 = WeaknessPriority.Poison;
    [SerializeField] WeaknessPriority weaknessPriority6 = WeaknessPriority.Piercing;
    [SerializeField] Priority priority = Priority.First;
    [SerializeField] bool primaryWeaknessTargeting = false;
    [SerializeField] bool secondWeaknessTargeting = false;
    [SerializeField] bool thirdWeaknessTargeting = false;
    [SerializeField] bool forthWeaknessTargeting = false;
    [SerializeField] bool fifthWeaknessTargeting = false;
    [SerializeField] bool sixthWeaknessTargeting = false;
    [SerializeField] bool movementTypePriorityTargeting = false;
    [SerializeField] bool advancedPositionPriorityTarget = false;
    [SerializeField] bool canDetectStealth = false;

    public List<Enemy> filteredEnemies = new List<Enemy>();

    public WeaknessTargetingLevel WeaknessTargetingLevel { get => weaknessTargetingLevel; set => weaknessTargetingLevel = value; }
    public bool CanDetectStealth { get => canDetectStealth; set => canDetectStealth = value; }
    public bool MovementTypePriorityTargeting { get => movementTypePriorityTargeting; set => movementTypePriorityTargeting = value; }
    public bool AdvancedPositionPriorityTarget { get => advancedPositionPriorityTarget; set => advancedPositionPriorityTarget = value; }

    void CheckPriorityCapabilities()
    {
        if (!AdvancedPositionPriorityTarget) { priority = Priority.First; }
        if (!primaryWeaknessTargeting) { weaknessPriority1 = WeaknessPriority.None; }
        else if (!secondWeaknessTargeting) { weaknessPriority2 = WeaknessPriority.None; }
        else if (!thirdWeaknessTargeting) { weaknessPriority3 = WeaknessPriority.None; }
        else if (!forthWeaknessTargeting) { weaknessPriority4 = WeaknessPriority.None; }
        else if (!fifthWeaknessTargeting) { weaknessPriority5 = WeaknessPriority.None; }
        else if (!sixthWeaknessTargeting) { weaknessPriority6 = WeaknessPriority.None; }
    }

    public Enemy PrioritizeTarget(in WeaponMount weaponMount, ref List<Enemy> enemies)
    {
        if (!CanDetectStealth) { foreach (Enemy enemy in enemies) { if (enemy.IsStealthed()) { enemies.Remove(enemy); } } }
        CheckPriorityCapabilities();
        Enemy target = null;
        if (enemies.Count > 0)
        {
            int priorityIndex = 0;
            
            if (priority == Priority.First || priority == Priority.Strongest || priority == Priority.Closest) priorityIndex = 0;
            else if (priority == Priority.Last) priorityIndex = enemies.Count - 1;

            if ((enemies.Count == 1) && !weaponMount.EnemyBeyondTurnRadius(enemies[0])) { target = enemies[priorityIndex]; }
            else if (enemies.Count == 1 && weaponMount.EnemyBeyondTurnRadius(enemies[0])) { target = null; }
            else if (enemies.Count > 0) { target = FindPriorityTarget(ref target, ref enemies, ref priorityIndex, in weaponMount); }
        }
        return target;
    }

    Enemy FindPriorityTarget(ref Enemy target, ref List<Enemy> enemies, ref int priorityIndex, in WeaponMount weaponMount)
    {
        filteredEnemies.Clear();
        if (weaknessPriority1 != WeaknessPriority.None)
        {
            PrioritizeWithWeakness(weaknessPriority1, ref enemies);
            if (weaknessPriority2 != WeaknessPriority.None && filteredEnemies.Count > 1)
            {
                PrioritizeWithWeakness(weaknessPriority2, ref filteredEnemies);
                if (weaknessPriority3 != WeaknessPriority.None && filteredEnemies.Count > 1)
                {
                    PrioritizeWithWeakness(weaknessPriority3, ref filteredEnemies);
                    if (weaknessPriority4 != WeaknessPriority.None && filteredEnemies.Count > 1)
                    {
                        PrioritizeWithWeakness(weaknessPriority4, ref filteredEnemies);
                        if (weaknessPriority5 != WeaknessPriority.None && filteredEnemies.Count > 1)
                        {
                            PrioritizeWithWeakness(weaknessPriority5, ref filteredEnemies);
                            if (weaknessPriority6 != WeaknessPriority.None && filteredEnemies.Count > 1)
                                PrioritizeWithWeakness(weaknessPriority6, ref filteredEnemies);
                        }
                    }
                }
            }
            filteredEnemies.Sort();
            target = PrioritizeWithoutWeakness(ref priorityIndex, ref target, ref filteredEnemies, in weaponMount);
        }
        else
        {
            target = PrioritizeWithoutWeakness(ref priorityIndex, ref target, ref enemies, in weaponMount);
        }
        return target;

}

    void PrioritizeWithWeakness(WeaknessPriority weaknessPriority, ref List<Enemy> enemyList)
    {
        List<Enemy> tempList = new List<Enemy>();
        float smallestResistance = 100;
        foreach (Enemy enemy in enemyList)
        {
            if (smallestResistance > enemy.GetResistance(weaknessPriority))
            {
                smallestResistance = enemy.GetResistance(weaknessPriority);
            }
        }

        foreach (Enemy enemy in enemyList)
        {
            if (enemy.GetResistance(weaknessPriority) == smallestResistance)
            {
                tempList.Add(enemy);
            }
        }
        filteredEnemies = tempList;
    }

    Enemy PrioritizeWithoutWeakness(ref int priorityIndex,ref Enemy target, ref List<Enemy> enemyList, in WeaponMount weaponMount)
    {
        if (!weaponMount.EnemyBeyondTurnRadius(enemyList[priorityIndex]) &&  (priority == Priority.First || priority == Priority.Last))
        {
            Debug.Log("No Priority so shoot first");
            target = enemyList[priorityIndex];
            return target;
        }
        
        target = enemyList[priorityIndex];
        target = PrioritizeWithoutTypePriority(priorityIndex, ref target, ref enemyList, in weaponMount);
        return target;
    }

    Enemy PrioritizeWithoutTypePriority(int priorityIndex, ref Enemy target, ref List<Enemy> enemyList, in WeaponMount weaponMount)
    {
        int i = priorityIndex;
        if(priority == Priority.Strongest)
        {
            FilterStrong(ref enemyList);
        }
        else if(priority == Priority.Closest)
        {
            FilterClose(ref enemyList, in weaponMount);
        }
        if (priority == Priority.First || priority == Priority.Strongest || priority == Priority.Closest)
        {
            while (weaponMount.EnemyBeyondTurnRadius(target) && i < enemyList.Count - 1)
            {
                target = enemyList[i];
                if (!weaponMount.EnemyBeyondTurnRadius(target) || i == enemyList.Count -1) { break; }
                i++;
            }
        }
        else if (priority == Priority.Last)
        {
            while (weaponMount.EnemyBeyondTurnRadius(target) && i > 0)
            {
                target = enemyList[i];
                if (!weaponMount.EnemyBeyondTurnRadius(target) || i == 0) { break; }
                i--;
            }
        }
        if (!weaponMount.EnemyBeyondTurnRadius(target)) { target = enemyList[i]; }
        else target = null;
        return target;
    }

    void FilterStrong(ref List<Enemy> enemyList)
    {
        List<Enemy> tempList = new List<Enemy>();
        int strongestHealth = 0;
        foreach (Enemy enemy in enemyList)
        {
            if (strongestHealth < enemy.GetCurrentHealth())
            {
                strongestHealth = enemy.GetCurrentHealth();
            }
        }

        foreach (Enemy enemy in enemyList)
        {
            if (enemy.GetCurrentHealth() == strongestHealth)
            {
                tempList.Add(enemy);
            }
        }
        enemyList = tempList;
    }

    void FilterClose(ref List<Enemy> enemyList, in WeaponMount weaponMount)
    {
        List<Enemy> tempList = new List<Enemy>();
        float closetDistance = 10000;
        Vector3 enemyPos;
        Vector3 weaponPos = weaponMount.transform.position;
        weaponPos.y = 0;
        foreach (Enemy enemy in enemyList)
        {
            enemyPos = enemy.transform.position;
            enemyPos.y = 0;
            if (closetDistance > Vector3.Distance(weaponPos, enemyPos))
            {
                closetDistance = Vector3.Distance(weaponPos, enemyPos);
            }
        }

        foreach (Enemy enemy in enemyList)
        {
            enemyPos = enemy.transform.position;
            enemyPos.y = 0;
            if (closetDistance == Vector3.Distance(weaponPos, enemyPos))
            {
                tempList.Add(enemy);
            }
        }
        enemyList = tempList;
    }

    public override void Fire(Transform target)
    {
        throw new System.NotImplementedException();
    }
}
