namespace TowerDefense
{
    
    
    public enum TP_Directory{ProjectileAmmo, SprayAmmo, ProjectileWeapon, SprayerWeapon, MeleeWeapon, LaserWeapon, TargetingSystem, TowerState, Barricade }
    public enum Priority { First, Last, Closest, Strongest }
    public enum WeaknessPriority { Physical, Fire, Ice, Lightning, Poison, Stealth, Piercing, Ground, Flying, None }
    public enum WeaknessTargetingLevel { Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
    public enum TurretAngle { A0, A45, A90, A135, A180, A225, A270, A315, A360 }
    public enum PartSize { Small, Medium, Large }
    public enum InventoryContentType { ProjectileAmmo, SprayAmmo, ProjectileWeapon, Sprayer, Melee, AdvancedTargetingSystem, Tower }
    public enum Accuracy { Level10, Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
    public enum Recoil { Level10, Level9, Level8, Level7, Level6, Level5, Level4, Level3, Level2, Level1 }
    public enum TowerPartType { Base, Mount, MountStyle, MountSlot, Weapon, Ammo, TowerState, Barricade }
    public enum TargetPriority { First, Last, Strong, Close }
    public enum TowerSize { Small, Medium, Large }

    public enum Transition
    {
        TowerInventory, 
        WeaponInventory, 
        AmmoInventory, 
        BarricadeInventory,
        ProjectileWeapon, 
        SprayWeapon,
        MeleeWeapon,
        TargetingSystem,
        LaserWeapon,
        ProjectileAmmo,
        SprayAmmo,
        ObjectVisibility
    }

    public enum SelectedInventory{ Tower, Weapon, Ammo, Barricade}
}
