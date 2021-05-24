using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.Enemies;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.Factories.Weapon;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts.Weapon
{
    /// <summary>
    /// Tower Weapon Component that enables a tower to have advanced enemy prioritization. 
    /// </summary>
    public class TP_Wep_TargetingSystem : TP_Weapon
    {
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Wep_TargetingSystem Factory;
        
        public List<Enemy> FilteredEnemies = new List<Enemy>();
        
        [SerializeField] WeaknessTargetingLevel _weaknessTargetingLevel = WeaknessTargetingLevel.Level1;
        [SerializeField] WeaknessPriority _weaknessPriority1 = WeaknessPriority.Physical;
        [SerializeField] WeaknessPriority _weaknessPriority2 = WeaknessPriority.Fire;
        [SerializeField] WeaknessPriority _weaknessPriority3 = WeaknessPriority.Ice;
        [SerializeField] WeaknessPriority _weaknessPriority4 = WeaknessPriority.Lightning;
        [SerializeField] WeaknessPriority _weaknessPriority5 = WeaknessPriority.Poison;
        [SerializeField] WeaknessPriority _weaknessPriority6 = WeaknessPriority.Piercing;
        
        [SerializeField] Priority priority = Priority.First;
        
        [SerializeField] bool _primaryWeaknessTargeting;
        [SerializeField] bool _secondWeaknessTargeting;
        [SerializeField] bool _thirdWeaknessTargeting;
        [SerializeField] bool _forthWeaknessTargeting;
        [SerializeField] bool _fifthWeaknessTargeting;
        [SerializeField] bool _sixthWeaknessTargeting;
        [SerializeField] bool _movementTypePriorityTargeting;
        [SerializeField] bool _advancedPositionPriorityTarget;
        [SerializeField] bool _canDetectStealth;

        WeaknessPriority[] _weaknessPriorities;

        protected override void Start()
        {
            _weaknessPriorities = new[]
            {
                _weaknessPriority1, 
                _weaknessPriority2, 
                _weaknessPriority3, 
                _weaknessPriority4, 
                _weaknessPriority5, 
                _weaknessPriority6
            };
        }
        
        public override void Fire(Transform target)
        {
            throw new System.NotImplementedException();
        }
        public override void SaveToFile()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Prefab Path", _prefabFilePath},
                {"Mat 1 Color", Mat1.color.ToString()},
                {"Mat 2 Color", Mat2.color.ToString()},
                {"Mat 3 Color", Mat3.color.ToString()},
                {"Name", name},
                {"Size", ((int) size).ToString()},
                {"Slot Number", SlotNumber.ToString()},
                {"Weakness Targeting Level", ((int)_weaknessTargetingLevel).ToString()},
                {"Basic Priority",_movementTypePriorityTargeting.ToString()},
                {"Advanced Priority", _advancedPositionPriorityTarget.ToString()},
                {"Detect Stealth", _canDetectStealth.ToString()}
            };
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        
        public override void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            CustomPartFilePath = jsonDict["File Path"];
            SlotNumber = int.Parse(jsonDict["Slot Number"]);
            
            Mat1.color = HU_Functions.StringToColor(jsonDict["Mat 1 Color"]);
            Mat2.color = HU_Functions.StringToColor(jsonDict["Mat 2 Color"]);
            Mat3.color = HU_Functions.StringToColor(jsonDict["Mat 3 Color"]);
            SetMaterials(Mat1, Mat2, Mat3);
            
            _weaknessTargetingLevel = (WeaknessTargetingLevel)(int.Parse(jsonDict["Weakness Targeting Level"]));
            _movementTypePriorityTargeting = bool.Parse(jsonDict["Basic Priority"]);
            _advancedPositionPriorityTarget = bool.Parse(jsonDict["Advanced Priority"]);
            _canDetectStealth = bool.Parse(jsonDict["Detect Stealth"]);
            SetSize((PartSize)int.Parse(jsonDict["Size"]), true);
        }
        
        public override void RemoveFromInventory() { Inventory.RemovePartFromInventory(this); }
        
        public override TC_UI_TP_Inventory GetInventory() { return Inventory; }
        public override TC_Fac_TowerPartFactory GetFactory() { return Factory; }
        
        public override void GenerateFileName()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TARGETING_SYSTEM_DIR + name + ".json";
        }
        
        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Size: " + TD_Globals.PartNameColor + GetSize() + "\n";
            if (_movementTypePriorityTargeting) stats += TD_Globals.PartNameColor + "Has basic prioritization." + "\n";
            if (_advancedPositionPriorityTarget) stats += TD_Globals.PartNameColor + "Has advanced prioritization." + "\n";
            if (_advancedPositionPriorityTarget) stats += TD_Globals.StandardWordColor + "Targeting Priority level:" + TD_Globals.PartNameColor + _weaknessTargetingLevel + "\n";
            if (_canDetectStealth) stats += TD_Globals.PartNameColor + "Can detect stealth." + "\n";
            
            return stats;
        }
        
        public override int Cost()
        {
            int result = 0;

            //TODO

            return result;
        }

        public WeaknessTargetingLevel WeaknessTargetingLevel { get => _weaknessTargetingLevel; set => _weaknessTargetingLevel = value; }
        public bool CanDetectStealth { get => _canDetectStealth; set => _canDetectStealth = value; }
        public bool MovementTypePriorityTargeting { get => _movementTypePriorityTargeting; set => _movementTypePriorityTargeting = value; }
        public bool AdvancedPositionPriorityTarget { get => _advancedPositionPriorityTarget; set => _advancedPositionPriorityTarget = value; }

        /// <summary>
        /// Returns the highest priority Enemy based on targeting system settings.   
        /// </summary>
        /// <param name="tpWeaponMount"></param>
        /// <param name="enemies"></param>
        /// <returns></returns>
        public Enemy PrioritizeTarget(in TP_WeaponMount tpWeaponMount, ref List<Enemy> enemies)
        {
            FilterStealthEnemies(ref enemies);
            CheckPriorityCapabilities();
            Enemy target = null;
            if (enemies.Count <= 0){ return null;}

            FindTarget(in tpWeaponMount, ref enemies, ref target);
            return target;
        }
        
        static void FindLastEnemyInTurnRadius(out Enemy target, ref List<Enemy> enemyList, in TP_WeaponMount tpWeaponMount)
        {
            for (int i = enemyList.Count; i > 0; i--) { if (tpWeaponMount.EnemyBeyondTurnRadius(target = enemyList[i])) { return; } }
            target = null;
        }

        static void FindFirstEnemyInTurnRadius( out Enemy target, ref List<Enemy> enemyList, in TP_WeaponMount tpWeaponMount)
        {
            for (int i = 0; i < enemyList.Count - 1; i++) { if (tpWeaponMount.EnemyBeyondTurnRadius(target = enemyList[i])) { return; } }
            target = null;
        }

        /// <summary>
        /// Returns an enemy with the highest health.
        /// </summary>
        /// <param name="enemyList"></param>
        static void FilterStrong(ref List<Enemy> enemyList)
        {
            List<Enemy> tempList = new List<Enemy>();
            int strongestHealth = 0;
            foreach (Enemy enemy in enemyList) { if (strongestHealth < enemy.GetCurrentHealth()) { strongestHealth = enemy.GetCurrentHealth(); } }
            foreach (Enemy enemy in enemyList) { if (enemy.GetCurrentHealth() == strongestHealth) { tempList.Add(enemy); } }
            enemyList = tempList;
        }

        /// <summary>
        /// Returns an enemy that's closest to the weapon mount.
        /// </summary>
        /// <param name="enemyList"></param>
        /// <param name="tpWeaponMount"></param>
        static void FilterClose(ref List<Enemy> enemyList, in TP_WeaponMount tpWeaponMount)
        {
            if (enemyList.Count == 0) return;
            List<Enemy> tempList = new List<Enemy>();
            Enemy closestEnemy = enemyList[0];
            Vector3 closestPos = closestEnemy.transform.position;
            closestPos.y = 0;
            // ReSharper disable once TooWideLocalVariableScope (Declared outside to generate less garbage)
            Vector3 enemyPos;
            Vector3 weaponPos = tpWeaponMount.transform.position;
            weaponPos.y = 0;
            foreach (Enemy enemy in enemyList)
            {
                enemyPos = enemy.transform.position;
                enemyPos.y = 0;
                if (!(Vector3.Distance(weaponPos, closestPos) > Vector3.Distance(weaponPos, enemyPos))) continue;
                closestEnemy = enemy;
                closestPos = enemy.transform.position;
                closestPos.y = 0;
            }
            tempList.Add(closestEnemy);
            enemyList = tempList;
        }
        
        /// <summary>
        /// Nullifies priority settings based on capabilities available.
        /// </summary>
        void CheckPriorityCapabilities()
        {
            if (!AdvancedPositionPriorityTarget) { priority = Priority.First; }
            if (!_primaryWeaknessTargeting) { _weaknessPriority1 = WeaknessPriority.None; }
            else if (!_secondWeaknessTargeting) { _weaknessPriority2 = WeaknessPriority.None; }
            else if (!_thirdWeaknessTargeting) { _weaknessPriority3 = WeaknessPriority.None; }
            else if (!_forthWeaknessTargeting) { _weaknessPriority4 = WeaknessPriority.None; }
            else if (!_fifthWeaknessTargeting) { _weaknessPriority5 = WeaknessPriority.None; }
            else if (!_sixthWeaknessTargeting) { _weaknessPriority6 = WeaknessPriority.None; }
        }

        /// <summary>
        /// Nullifies priority settings based on capabilities available.
        /// </summary>
        /// <param name="tpWeaponMount"></param>
        /// <param name="enemies"></param>
        /// <param name="target"></param>
        void FindTarget(in TP_WeaponMount tpWeaponMount, ref List<Enemy> enemies, ref Enemy target)
        {
            if (enemies.Count == 1) { target = tpWeaponMount.EnemyBeyondTurnRadius(enemies[0])? null : enemies[0]; return; }
            if (enemies.Count > 0) { FindPriorityTarget(out target, ref enemies, in tpWeaponMount); } 
        }
        
        /// <summary>
        /// Returns the highest priority Enemy based on targeting system settings.  
        /// </summary>
        /// <param name="target"></param>
        /// <param name="enemies"></param>
        /// <param name="tpWeaponMount"></param>
        void FindPriorityTarget(out Enemy target, ref List<Enemy> enemies,in TP_WeaponMount tpWeaponMount)
        {
            FilteredEnemies.Clear();
            if (_weaknessPriority1 != WeaknessPriority.None)
            {
                SortEnemiesByPriorities();
                PrioritizeWithoutWeakness(out target, ref FilteredEnemies, in tpWeaponMount);
            }
            else { PrioritizeWithoutWeakness(out target, ref enemies, in tpWeaponMount); }

        }
        
        /// <summary>
        /// Removes all stealthed enemies from a list enemies.
        /// </summary>
        /// <param name="enemies"></param>

        void FilterStealthEnemies(ref List<Enemy> enemies)
        {
            if (CanDetectStealth) return;
            foreach (Enemy enemy in enemies) { if (enemy.IsStealthed()) { enemies.Remove(enemy); } }
        }

        /// <summary>
        /// Sorts enemies by each level of weaknessPriorities
        /// </summary>
        void SortEnemiesByPriorities()
        {
            //PrioritizeWithWeakness(_weaknessPriority1, ref enemies);
            for (int i = 1; i < _weaknessPriorities.Length; i++)
            {
                if (!SortEnemiesByPriorityLevel(_weaknessPriorities[i])) { break; }
            }
            // if (_weaknessPriority2 != WeaknessPriority.None && FilteredEnemies.Count > 1)
            // {
            //     PrioritizeWithWeakness(_weaknessPriority2, ref FilteredEnemies);
            //     if (_weaknessPriority3 != WeaknessPriority.None && FilteredEnemies.Count > 1)
            //     {
            //         PrioritizeWithWeakness(_weaknessPriority3, ref FilteredEnemies);
            //         if (_weaknessPriority4 != WeaknessPriority.None && FilteredEnemies.Count > 1)
            //         {
            //             PrioritizeWithWeakness(_weaknessPriority4, ref FilteredEnemies);
            //             if (_weaknessPriority5 != WeaknessPriority.None && FilteredEnemies.Count > 1)
            //             {
            //                 PrioritizeWithWeakness(_weaknessPriority5, ref FilteredEnemies);
            //                 if (_weaknessPriority6 != WeaknessPriority.None && FilteredEnemies.Count > 1)
            //                     PrioritizeWithWeakness(_weaknessPriority6, ref FilteredEnemies);
            //             }
            //         }
            //     }
            // }
            FilteredEnemies.Sort();
        }

        /// <summary>
        /// Filters out enemies so only enemies who are the weakest against a certain weakness type.
        /// Returns false if enemy count is 1 or the passed priority level is none.
        /// </summary>
        /// <param name="priorityLevel"></param>
        /// <returns></returns>
        bool SortEnemiesByPriorityLevel(WeaknessPriority priorityLevel)
        {
            if (priorityLevel == WeaknessPriority.None || FilteredEnemies.Count <= 1) return false;
            List<Enemy> tempList = new List<Enemy>();
            int smallestResistance = 100;
            foreach (Enemy enemy in FilteredEnemies) { if (smallestResistance > enemy.GetResistance(priorityLevel)) { smallestResistance = enemy.GetResistance(priorityLevel); } }
            foreach (Enemy enemy in FilteredEnemies) { if (enemy.GetResistance(priorityLevel) == smallestResistance) { tempList.Add(enemy); } }
            FilteredEnemies = tempList;
            return true; 
        }

        void PrioritizeWithoutWeakness(out Enemy target, ref List<Enemy> enemies, in TP_WeaponMount tpWeaponMount)
        {
            // Filter out weak or far enemies
            if (priority == Priority.Strongest) { FilterStrong(ref enemies); }
            else if (priority == Priority.Closest) { FilterClose(ref enemies, in tpWeaponMount); }

            // Strongest and Closest default back to first after filter
            if (priority == Priority.Last) { FindLastEnemyInTurnRadius(out target, ref enemies, in tpWeaponMount); }
            else{ FindFirstEnemyInTurnRadius(out target, ref enemies, in tpWeaponMount);}
        }
    }
}