// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using System.IO;
//
// using TPath = System.Collections.Generic.KeyValuePair<TowerState, string>;
// using FM = TowerPartFileManager;
// using FMEdit = FMTowerPartEditor;
// using TPSlot = TowerPartInventorySlot;
//
// public class InventoryManager : MonoBehaviour
// {
//     public static InventoryManager inventoryManager = null;
//     [SerializeField] private float ammoDistanceFromCamera = 10;
//     [SerializeField] private float weaponDistanceFromCamera = 10;
//     [SerializeField] private float towerDistanceFromCamera = 10;
//     [SerializeField] public SlotDetector slotDetector = null;
//     [SerializeField] List<GameObject> projectileAmmoContent = null;
//     [SerializeField] List<GameObject> sprayAmmoContent = null;
//     [SerializeField] List<GameObject> projectileWeaponContent = null;
//     [SerializeField] List<GameObject> sprayerContent = null;
//     [SerializeField] List<GameObject> meleeContent = null;
//     [SerializeField] List<GameObject> advancedTargetingContent = null;
//     [SerializeField] List<GameObject> lazerContent = null;
//     [SerializeField] List<GameObject> barricadeContent = null;
//     [SerializeField] List<GameObject> towerContent = null;
//     [SerializeField] Color normalColor = new Color();
//     [SerializeField] Color selectedColor = new Color();
//     [SerializeField] TMP_InputField inputField = null;
//     public static List<TowerPart> projectileAmmunition = new List<TowerPart>();
//     public static List<TowerPart> sprayAmmunition = new List<TowerPart>();
//     List<TowerPart> projectileWeapons = new List<TowerPart>();
//     List<TowerPart> sprayers = new List<TowerPart>();
//     List<TowerPart> advancedTargetingSystems = new List<TowerPart>();
//     List<TowerPart> meleeWeapons = new List<TowerPart>();
//     List<TowerPart> lazerWeapons = new List<TowerPart>();
//     List<TowerPart> walls = new List<TowerPart>();
//     List<TowerState> towerStates = new List<TowerState>();
//
//     public static TPSlot towerPartInventorySlot = null;
//     TPSlot pathedInventorySlot = null;
//     GameObject inventorySlot = null;
//     GameObject slotImage = null;
//     int projectileAmmunitionBuiltCount = 0;
//     int sprayAmmunitionBuiltCount = 0;
//     int projectileWeaponBuiltCount = 0;
//     int sprayerBuiltCount = 0;
//     int meleeBuiltCount = 0;
//     int advancedTargetingBuiltCount = 0;
//     int lazerBuiltCount = 0;
//     int wallBuiltCount = 0;
//     int towerStatesBuiltCount = 0;
//     public bool clearToAdd = false;
//
//     private void Awake() { inventoryManager = this; }
//
//     public List<TowerState> GetTowerStates() { return towerStates; }
//
//     public void AddPart(ref TowerPart part)
//     {
//         clearToAdd = false;
//         clearToAdd = true;
//         part.transform.position = new Vector3(0, 100, 0);
//         AddPartType(ref part); //}
//     }
//     
// #region Add part methods
//     void AddPartType(ref TowerPart partObject)
//     {
//         partObject.GetComponent<TowerPart>().inventorySlots.Clear();
//         if (partObject.GetComponent<Projectile>()) { AddProjectileAmmo(ref partObject); }
//         else if (partObject.GetComponent<Spray>()) { AddSprayAmmo(ref partObject); }
//         else if (partObject.GetComponent<WeaponProjectile>()) { AddProjectileWeapon(ref partObject); }
//         else if (partObject.GetComponent<WeaponSprayer>()) { AddSprayWeapon(ref partObject); }
//         else if (partObject.GetComponent<WeaponMelee>()) { AddMeleeWeapon(ref partObject); }
//         else if (partObject.GetComponent<AdvancedTargetingSystem>()) { AddTargetingSystem( ref partObject); }
//         else if (partObject.GetComponent<TowerState>()) { AddTowerState( ref partObject); }
//         UI3DManager.instance.AddUIElement(ref partObject);
//
//         if (FMEdit.editIsPathed)
// 		{
// 			if (FMEdit.pathedSibIndex < towerPartInventorySlot.transform.parent.childCount - 3)
// 			{
//                 pathedInventorySlot.transform.parent.GetChild(FMEdit.pathedSibIndex).transform.SetParent(null);
//                 TPSlot.slotFollowingCursor = pathedInventorySlot;
//                 TPSlot.SwapDirectoryOrder(pathedInventorySlot.transform.parent.GetChild(FMEdit.pathedSibIndex).GetComponent<TPSlot>());
// 			}
// 			FMTowerPartEditor.editIsPathed = false;
// 		}
//     }
//
//     void AddProjectileAmmo(ref TowerPart projectile)
//     {
//         projectile.customePartFilePath = (FM.rootDir + FM.projectileAmmoDir + projectile.name + ".txt");
//         projectileAmmunition.Add(projectile);
//         foreach (GameObject content in projectileAmmoContent)
//         {
//             for (int i = projectileAmmunitionBuiltCount; i < projectileAmmunition.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(projectileAmmunition[i], slotDetector, ammoDistanceFromCamera);
//             }
//         }
//         projectileAmmunitionBuiltCount++;
//     }
//
//     void AddSprayAmmo(ref TowerPart spray)
//     {
//         spray.customePartFilePath = (FM.rootDir + FM.sprayAmmoDir + spray.name + ".txt");
//         sprayAmmunition.Add(spray);
//         foreach (GameObject content in sprayAmmoContent)
//         {
//             for (int i = sprayAmmunitionBuiltCount; i < sprayAmmunition.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(sprayAmmunition[i], slotDetector, ammoDistanceFromCamera);
//             }
//         }
//         sprayAmmunitionBuiltCount++;
//     }
//
//     void AddProjectileWeapon( ref TowerPart barrel)
//     {
//         barrel.customePartFilePath = (FM.rootDir + FM.projectileWeaponDir + barrel.name + ".txt");
//         projectileWeapons.Add(barrel);
//         foreach (GameObject content in projectileWeaponContent)
//         {
//             for (int i = projectileWeaponBuiltCount; i < projectileWeapons.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(projectileWeapons[i], slotDetector, weaponDistanceFromCamera);
//             }
//         }
//         projectileWeaponBuiltCount++;
//     }
//
//     void AddSprayWeapon(ref TowerPart sprayer)
//     {
//         sprayer.customePartFilePath = (FM.rootDir + FM.sprayWeaponDir + sprayer.name + ".txt");
//         sprayers.Add(sprayer);
//         foreach (GameObject content in sprayerContent)
//         {
//             for (int i = sprayerBuiltCount; i < sprayers.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(sprayers[i], slotDetector, weaponDistanceFromCamera);
//             }
//         }
//         sprayerBuiltCount++;
//     }
//
//     void AddTargetingSystem( ref TowerPart targetingSystem)
//     {
//         targetingSystem.customePartFilePath = (FM.rootDir + FM.targetingSystemDir + targetingSystem.name + ".txt");
//         advancedTargetingSystems.Add(targetingSystem);
//         foreach (GameObject content in advancedTargetingContent)
//         {
//             for (int i = advancedTargetingBuiltCount; i < advancedTargetingSystems.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(advancedTargetingSystems[i], slotDetector, weaponDistanceFromCamera);
//             }
//         }
//         advancedTargetingBuiltCount++;
//     }
//
//     void AddMeleeWeapon( ref TowerPart meleeWeapon)
//     {
//         meleeWeapon.customePartFilePath = (FM.rootDir + FM.meleeWeaponDir + meleeWeapon.name + ".txt");
//         meleeWeapons.Add(meleeWeapon);
//         foreach (GameObject content in meleeContent)
//         {
//             for (int i = meleeBuiltCount; i < meleeWeapons.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(meleeWeapons[i], slotDetector, weaponDistanceFromCamera);
//             }
//         }
//         meleeBuiltCount++;
//     }
//
//     void AddTowerState( ref TowerPart part)
//     {
//         TowerState state = part.GetComponent<TowerState>();
//         AddState(ref state);
//         AddPaths(ref state);
//         towerStatesBuiltCount++;
//         if(FMEdit.editIsPathed) { EditPathedTower(ref state); }
//         if (state.isRoot) state.SetRootsAfterTime();
//     }
//     
//     void AddState( ref TowerState state)
//     {
//         state.customePartFilePath = (FM.rootDir + FM.towerStateDir + state.name + ".txt");
//         towerStates.Add(state);
//         foreach (GameObject content in towerContent)
//         {
//             for (int i = towerStatesBuiltCount; i < towerStates.Count; i++)
//             {
//                 ConstructTowerPartUI(content);
//                 towerPartInventorySlot.Initialize(towerStates[i], slotDetector, towerDistanceFromCamera);
//             }
//         }
//     }
// #endregion
//
// public string GetFileName(GameObject partObject)
//     {
//         if (partObject.GetComponent<Projectile>()) { return FM.rootDir + FM.projectileAmmoDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<Spray>()) { return FM.rootDir + FM.sprayAmmoDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<WeaponProjectile>()) { return FM.rootDir + FM.projectileWeaponDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<WeaponSprayer>()) { return FM.rootDir + FM.sprayWeaponDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<WeaponMelee>()) { return FM.rootDir + FM.meleeWeaponDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<AdvancedTargetingSystem>()) { return FM.rootDir + FM.targetingSystemDir + inputField.text + ".txt"; }
//         else if (partObject.GetComponent<TowerState>()) { return FM.rootDir + FM.towerStateDir + inputField.text + ".txt"; }
//         else return "";
//     }
//
//     void DestroyPartsAndSlots(GameObject partObject)
//     {
//         foreach (TowerPart part in towerStates) { part.DestroySlotsThenSelf(); }
//         if (partObject.GetComponent<Projectile>()) { foreach (TowerPart part in projectileWeapons) { part.DestroySlotsThenSelf(); } projectileAmmunitionBuiltCount = 0; projectileWeapons.Clear(); } 
//         if(partObject.GetComponent<Spray>()){foreach (TowerPart part in sprayers) { part.DestroySlotsThenSelf();} sprayAmmunitionBuiltCount = 0; sprayers.Clear(); }
//         towerStatesBuiltCount = 0;
//         towerStates.Clear();
//     }
//
//     string GetPartToUpdateData(GameObject partObject, int i)
//     {
//         StreamReader reader = null;
//         if (partObject.GetComponent<Projectile>()) reader = File.OpenText(projectileWeapons[i].customePartFilePath);
//         else if (partObject.GetComponent<Spray>()) reader = File.OpenText(sprayers[i].customePartFilePath);
//         else if (partObject.GetComponent<Weapon>()) reader = File.OpenText(towerStates[i].customePartFilePath);
//         string partData = reader.ReadToEnd();
//         reader.Dispose();
//         return partData;
//     }
//
//     void ReloadParts()
//     {
//         if (projectileWeapons.Count == 0) { projectileWeaponBuiltCount = 0;  FM.LoadTowerPart(FM.projectileWeaponDir); }
//         if (sprayers.Count == 0) { sprayerBuiltCount = 0; FM.LoadTowerPart(FM.sprayWeaponDir); }
//         if (towerStates.Count == 0) { towerStatesBuiltCount = 0; FM.LoadTowerPart(FM.towerStateDir); }
//     }
//
//     void AddPaths(ref TowerState state)
// 	{
//         state.path1 = TowerPartFactory.path1; state.path2 = TowerPartFactory.path2; state.path3 = TowerPartFactory.path3;
//         if (state.path1.Count > 0 || state.path2.Count > 0 || state.path3.Count > 0) { state.isRoot = true; }
//         else state.isRoot = false;
//         TowerPartFactory.path1 = new List<TPath>();
//         TowerPartFactory.path2 = new List<TPath>();
//         TowerPartFactory.path3 = new List<TPath>();
//     }
//
//     void EditPathedTower(ref TowerState state)
// 	{
//         switch (FMTowerPartEditor.pathNum)
//         {
//             case 1:  FMEdit.root.path1.Insert(FMEdit.pathIndex, new TPath(state, state.customePartFilePath)); break;
//             case 2:  FMEdit.root.path2.Insert(FMEdit.pathIndex, new TPath(state, state.customePartFilePath)); break;
//             case 3:  FMEdit.root.path3.Insert(FMEdit.pathIndex, new TPath(state, state.customePartFilePath)); break;
//         }
//         pathedInventorySlot = towerPartInventorySlot;
//         FMEdit.editIsPathed = false;
//         FMEdit.pathNum = 0;
//         TowerConstructionInterface.SelectSlot(FMEdit.root.inventorySlots[0]);
//         FMEdit.root = null;
//         TowerPartSelector.forceHide = true;
//         TowerConstructionInterface.EditPart();
//         ButtonEventManager.ConfirmTower();
//
//         FMEdit.editIsPathed = true;
//         TowerPartSelector.forceHide = false;
//     }
//
//     public void ConstructTowerPartUI(GameObject inventory, bool addEvents = true)
//     {
//         BuildObjects(inventory);
//         AddButtonToObject();
//         AddEventToObject(addEvents);
//     }
//     
//     #region Construct tower part UI helpers
//     void BuildObjects(GameObject inventory)
//     {
//         inventorySlot = new GameObject();
//         inventorySlot.name = UIElements.fileNameInput.text + " inventory slot";
//         slotImage = new GameObject();
//         slotImage.name = "Slot image";
//         inventorySlot.transform.SetParent(inventory.transform);
//         slotImage.transform.SetParent(inventorySlot.transform);
//         towerPartInventorySlot = inventorySlot.AddComponent<TPSlot>();
//         inventorySlot.AddComponent<RectTransform>().pivot = new Vector2(0, 1);
//         slotImage.AddComponent<RawImage>();
//         //slotImage.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
//         inventorySlot.AddComponent<BoxCollider2D>();
//     }
//
//     void AddButtonToObject()
//     {
//         RectTransform rectTrans = slotImage.GetComponent<RectTransform>();
//         rectTrans.offsetMin = Vector2.zero;
//         rectTrans.offsetMax = Vector2.zero;
//         rectTrans.anchorMin = Vector2.zero; 
//         rectTrans.anchorMax = new Vector2(1, 1);
//         inventorySlot.AddComponent<Image>();
//         Button button = inventorySlot.AddComponent<Button>();
//         ColorBlock colorBlock = button.colors;
//         colorBlock.selectedColor = selectedColor;
//         colorBlock.normalColor = normalColor;
//         colorBlock.highlightedColor = selectedColor;
//         colorBlock.pressedColor = selectedColor;
//         button.colors = colorBlock;
//     }
//
//     void AddEventToObject(bool addEvents)
//     {
//         InventoryEventTrigger inventoryEventTrigger = inventorySlot.AddComponent<InventoryEventTrigger>();
//         inventoryEventTrigger.SetInventorySlot(towerPartInventorySlot);
//         if(addEvents) towerPartInventorySlot.AddEvents();
//     }
//     #endregion
//
//     public void RemoveFromInventory(TowerPart part)
//     {
//         if (part.GetComponent<TowerState>())
//         {
//             towerStatesBuiltCount--;
//             for (int i = 0; i < towerStates.Count;) { if (towerStates[i].name == part.name) { towerStates.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<WeaponProjectile>()) 
//         {
//             projectileWeaponBuiltCount--;
//             for (int i = 0; i < projectileWeapons.Count;) { if (projectileWeapons[i].name == part.name) { projectileWeapons.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<WeaponSprayer>()) 
//         {
//             sprayerBuiltCount--;
//             for (int i = 0; i < sprayers.Count;) { if (sprayers[i].name == part.name) { sprayers.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<WeaponMelee>())
//         {
//             meleeBuiltCount--;
//             for (int i = 0; i < meleeWeapons.Count;) { if (meleeWeapons[i].name == part.name) { meleeWeapons.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<AdvancedTargetingSystem>())
//         {
//             advancedTargetingBuiltCount--;
//             for (int i = 0; i < advancedTargetingSystems.Count;) { if (advancedTargetingSystems[i].name == part.name) { advancedTargetingSystems.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<Projectile>())
//         {
//             projectileAmmunitionBuiltCount--;
//             for (int i = 0; i < projectileAmmunition.Count;) { if (projectileAmmunition[i].name == part.name) { projectileAmmunition.RemoveAt(i); } else i++; }
//         }
//         else if (part.GetComponent<Spray>()) 
//         {
//             sprayAmmunitionBuiltCount--;
//             for (int i = 0; i < sprayAmmunition.Count;) { if (sprayAmmunition[i].name == part.name) { sprayAmmunition.RemoveAt(i); } else i++; }
//         }
//     }
//
//     public void WipeInventory()
//     {
//         ClearBuildCounts();
//         DestroyInventories();
//         ClearPartLists();
//     }
//
//     void ClearBuildCounts()
//     {
//         towerStatesBuiltCount = 0;
//         projectileWeaponBuiltCount = 0;
//         sprayerBuiltCount = 0;
//         meleeBuiltCount = 0;
//         advancedTargetingBuiltCount = 0;
//         lazerBuiltCount = 0;
//         wallBuiltCount = 0;
//         projectileAmmunitionBuiltCount = 0;
//         sprayAmmunitionBuiltCount = 0;
//     }
//
//     void DestroyInventories()
//     {
//         foreach (var part in towerStates) { Destroy(part.gameObject); }
//         foreach (var part in projectileWeapons) { Destroy(part.gameObject); }
//         foreach (var part in sprayers) { Destroy(part.gameObject); }
//         foreach (var part in meleeWeapons) { Destroy(part.gameObject); }
//         foreach (var part in advancedTargetingSystems) { Destroy(part.gameObject); }
//         foreach (var part in lazerWeapons) { Destroy(part.gameObject); }
//         foreach (var part in walls) { Destroy(part.gameObject); }
//         foreach (var part in projectileAmmunition) { Destroy(part.gameObject); }
//         foreach (var part in sprayAmmunition) { Destroy(part.gameObject); }
//     }
//
//     void ClearPartLists()
//     {
//         towerStates.Clear();
//         projectileWeapons.Clear();
//         sprayers.Clear();
//         meleeWeapons.Clear();
//         advancedTargetingSystems.Clear();
//         lazerWeapons.Clear();
//         walls.Clear();
//         projectileAmmunition.Clear();
//         sprayAmmunition.Clear(); 
//     }
// }