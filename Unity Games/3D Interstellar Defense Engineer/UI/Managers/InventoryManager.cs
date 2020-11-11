using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using UnityEngine.EventSystems;
using System.ComponentModel.Design.Serialization;
using System.Xml.XPath;
using System.Runtime.InteropServices;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager inventoryManager = null;
    [SerializeField] public SlotDetector slotDetector = null;
    [SerializeField] List<GameObject> projectileAmmoContent = null;
    [SerializeField] List<GameObject> sprayAmmoContent = null;
    [SerializeField] List<GameObject> projectileWeaponContent = null;
    [SerializeField] List<GameObject> sprayerContent = null;
    [SerializeField] List<GameObject> meleeContent = null;
    [SerializeField] List<GameObject> advancedTargetingContent = null;
    [SerializeField] List<GameObject> towerContent = null;
    [SerializeField] Color normalColor = new Color();
    [SerializeField] Color selectedColor = new Color();
    [SerializeField] TMP_InputField inputField = null;
    public static List<TowerPart> projectileAmmunition = new List<TowerPart>();
    public static List<TowerPart> sprayAmmunition = new List<TowerPart>();
    List<TowerPart> projectileWeapons = new List<TowerPart>();
    List<TowerPart> sprayers = new List<TowerPart>();
    List<TowerPart> advancedTargetingSystems = new List<TowerPart>();
    List<TowerPart> meleeWeapons = new List<TowerPart>();
    List<TowerState> towerStates = new List<TowerState>();

    public TowerPartInventorySlot towerPartInventorySlot = null;
    TowerPartInventorySlot pathedInventorySlot = null;
    GameObject inventorySlot = null;
    GameObject slotImage = null;
    int projectileAmmunitionBuiltCount = 0;
    int sprayAmmunitionBuiltCount = 0;
    int projectileWeaponBuiltCount = 0;
    int sprayerBuiltCount = 0;
    int meleeBuiltCount = 0;
    int advancedTargetingBuiltCount = 0;
    int towerStatesBuiltCount = 0;
    public bool clearToAdd = false;
    public bool updateAfterSubPartEdit = false;
    bool skipNameCheck = false;

    private void Awake() { inventoryManager = this; }

    public void SetEdit() { updateAfterSubPartEdit = true; }
    public void CancelEdit() { updateAfterSubPartEdit = false; }
    public List<TowerState> GetTowerStates() { return towerStates; }

    public void AddPart(bool isNew, TowerPart part)
    {
        clearToAdd = false;
        GameObject partObject = Instantiate(part.gameObject);
        if (!NameIsInUse(partObject, part))
        {
            partObject.GetComponent<TowerPart>().SetView(partObject.GetComponent<TowerPart>().GetView());
            clearToAdd = true;
            if (isNew) partObject.name = inputField.text;
            else partObject.name = part.name;
            partObject.transform.position = new Vector3(0, 100, 0);

            if (updateAfterSubPartEdit) { UpdatePart(partObject); }
            else { AddPartType(partObject, isNew); }
        }
        inputField.text = "";
    }

    bool NameIsInUse(GameObject partObject, TowerPart part)
    {
        if (File.Exists(FileManager.rootDir + FileManager.GetPartDirectory(part) + inputField.text + ".txt") && !updateAfterSubPartEdit && !skipNameCheck)
        {
            UIElements.rejectDeleteCanvas.SetActive(true);
            UIElements.rejectDeleteText.text = "This name is already in use.";
            Destroy(partObject);
            return true;
        }
        return false;
    }

    void UpdatePart(GameObject partObject)
    {
        TowerState state = partObject.GetComponent<TowerState>();
        TowerPart part = partObject.GetComponent<TowerPart>();
        if (FMTowerPartEditor.editIsPathed) { FMTowerPartEditor.StorePathedTowerInfo(); }
        FileManager.forceDelete = true;
        FileManager.DeleteTowerPart();
        if (!state) UpdateExistingParts(partObject);
        else AddPartType(partObject, true);
    }

    void UpdateExistingParts(GameObject partObject)
    {
        bool reload = UpdatePartFiles(partObject);
        AddPartType(partObject, true);
        if (reload) ReloadParts();
    }

    bool UpdatePartFiles(GameObject partObject)
    {
        bool reload = false;
        int count = 0;

        if (partObject.GetComponent<Weapon>()) count = towerStates.Count;
        else if (partObject.GetComponent<Projectile>()) count = projectileWeapons.Count;
        else if (partObject.GetComponent<Spray>()) count = sprayers.Count;
        for (int i = 0; i < count; i++)
        {
            string partData = GetPartToUpdateData(partObject, i);
            if (partData.Contains(FileManager.editedPartsPriorCustomFilePath)) { reload = true; UpdatePartFile(partObject, partData, i); }
        }
        if (reload) { DestroyPartsAndSlots(partObject); }
        return reload;
    }

    void UpdatePartFile(GameObject partObject, string partData, int i)
    {
        partData = partData.Replace(FileManager.editedPartsPriorCustomFilePath, GetFileName(partObject));
        StreamWriter writer = null;
        if (partObject.GetComponent<Projectile>()) writer = new StreamWriter(projectileWeapons[i].customePartFilePath);
        else if (partObject.GetComponent<Spray>()) writer = new StreamWriter(sprayers[i].customePartFilePath);
        else if (partObject.GetComponent<Weapon>()) writer = new StreamWriter(towerStates[i].customePartFilePath);
        writer.Write(partData);
        writer.Dispose();
    }

    public string GetFileName(GameObject partObject)
    {
        if (partObject.GetComponent<Projectile>()) { return FileManager.rootDir + FileManager.projectileAmmoDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<Spray>()) { return FileManager.rootDir + FileManager.sprayAmmoDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<WeaponProjectile>()) { return FileManager.rootDir + FileManager.projectileWeaponDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<WeaponSprayer>()) { return FileManager.rootDir + FileManager.sprayWeaponDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<WeaponMelee>()) { return FileManager.rootDir + FileManager.meleeWeaponDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<AdvancedTargetingSystem>()) { return FileManager.rootDir + FileManager.targetingSystemDir + inputField.text + ".txt"; }
        else if (partObject.GetComponent<TowerState>()) { return FileManager.rootDir + FileManager.towerStateDir + inputField.text + ".txt"; }
        else return "";
    }

    void DestroyPartsAndSlots(GameObject partObject)
    {
        foreach (TowerPart part in towerStates) { part.DestroySlotsThenSelf(); }
        if (partObject.GetComponent<Projectile>()) { foreach (TowerPart part in projectileWeapons) { part.DestroySlotsThenSelf(); } projectileAmmunitionBuiltCount = 0; projectileWeapons.Clear(); } 
        if(partObject.GetComponent<Spray>()){foreach (TowerPart part in sprayers) { part.DestroySlotsThenSelf();} sprayAmmunitionBuiltCount = 0; sprayers.Clear(); }
        towerStatesBuiltCount = 0;
        towerStates.Clear();
    }

    string GetPartToUpdateData(GameObject partObject, int i)
    {
        StreamReader reader = null;
        if (partObject.GetComponent<Projectile>()) reader = File.OpenText(projectileWeapons[i].customePartFilePath);
        else if (partObject.GetComponent<Spray>()) reader = File.OpenText(sprayers[i].customePartFilePath);
        else if (partObject.GetComponent<Weapon>()) reader = File.OpenText(towerStates[i].customePartFilePath);
        string partData = reader.ReadToEnd();
        reader.Dispose();
        return partData;
    }

    void ReloadParts()
    {
        skipNameCheck = true;
        if (projectileWeapons.Count == 0) { projectileWeaponBuiltCount = 0;  FileManager.LoadTowerPart(FileManager.projectileWeaponDir); }
        if (sprayers.Count == 0) { sprayerBuiltCount = 0; FileManager.LoadTowerPart(FileManager.sprayWeaponDir); }
        if (towerStates.Count == 0) { towerStatesBuiltCount = 0; FileManager.LoadTowerPart(FileManager.towerStateDir); }
        skipNameCheck = false;
    }

    void AddPartType(GameObject partObject, bool isNew)
    {
        partObject.GetComponent<TowerPart>().inventorySlots.Clear();
        if (partObject.GetComponent<Projectile>()) { AddProjectileAmmo(isNew, partObject.GetComponent<Projectile>()); }
        else if (partObject.GetComponent<Spray>()) { AddSprayAmmo(isNew, partObject.GetComponent<Spray>()); }
        else if (partObject.GetComponent<WeaponProjectile>()) { AddProjectileWeapon(isNew, partObject.GetComponent<WeaponProjectile>()); }
        else if (partObject.GetComponent<WeaponSprayer>()) { AddSprayWeapon(isNew, partObject.GetComponent<WeaponSprayer>()); }
        else if (partObject.GetComponent<WeaponMelee>()) { AddMeleeWeapon(isNew, partObject.GetComponent<WeaponMelee>()); }
        else if (partObject.GetComponent<AdvancedTargetingSystem>()) { AddTargetingSystem(isNew, partObject.GetComponent<AdvancedTargetingSystem>()); }
        else if (partObject.GetComponent<TowerState>()) { AddTowerState(isNew, partObject.GetComponent<TowerState>()); }
        UI3DManager.instance.AddUIElement(partObject);
		if (updateAfterSubPartEdit)
		{
			int end = towerPartInventorySlot.transform.parent.childCount - 3;
			if (FileManager.editedSlotSibIndex < end)
			{
                towerPartInventorySlot.transform.parent.GetChild(FileManager.editedSlotSibIndex).transform.SetParent(null);
				TowerPartInventorySlot.slotFollowingCursor = towerPartInventorySlot;
				TowerPartInventorySlot.SwapDirectoryOrder(towerPartInventorySlot.transform.parent.GetChild(FileManager.editedSlotSibIndex).GetComponent<TowerPartInventorySlot>());
			}
		}
		if (FMTowerPartEditor.editIsPathed && !updateAfterSubPartEdit)
		{
			if (FMTowerPartEditor.pathedSibIndex < towerPartInventorySlot.transform.parent.childCount - 3)
			{
                pathedInventorySlot.transform.parent.GetChild(FMTowerPartEditor.pathedSibIndex).transform.SetParent(null);
                TowerPartInventorySlot.slotFollowingCursor = pathedInventorySlot;
				TowerPartInventorySlot.SwapDirectoryOrder(pathedInventorySlot.transform.parent.GetChild(FMTowerPartEditor.pathedSibIndex).GetComponent<TowerPartInventorySlot>());
			}
			FMTowerPartEditor.editIsPathed = false;
		}
		updateAfterSubPartEdit = false;
    }

    void AddProjectileAmmo(bool isNew, Projectile projectile)
    {
        projectile.customePartFilePath = (FileManager.rootDir + FileManager.projectileAmmoDir + projectile.name + ".txt");
        projectileAmmunition.Add(projectile);
        foreach (GameObject content in projectileAmmoContent)
        {
            for (int i = projectileAmmunitionBuiltCount; i < projectileAmmunition.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(projectileAmmunition[i], slotDetector, 15);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(projectile); }
        projectileAmmunitionBuiltCount++;
    }

    void AddSprayAmmo(bool isNew, Spray spray)
    {
        spray.customePartFilePath = (FileManager.rootDir + FileManager.sprayAmmoDir + spray.name + ".txt");
        sprayAmmunition.Add(spray);
        foreach (GameObject content in sprayAmmoContent)
        {
            for (int i = sprayAmmunitionBuiltCount; i < sprayAmmunition.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(sprayAmmunition[i], slotDetector, 15);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(spray); }
        sprayAmmunitionBuiltCount++;
    }

    void AddProjectileWeapon(bool isNew, WeaponProjectile barrel)
    {
        barrel.customePartFilePath = (FileManager.rootDir + FileManager.projectileWeaponDir + barrel.name + ".txt");
        projectileWeapons.Add(barrel);
        foreach (GameObject content in projectileWeaponContent)
        {
            for (int i = projectileWeaponBuiltCount; i < projectileWeapons.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(projectileWeapons[i], slotDetector, 22.6f);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(barrel); }
        projectileWeaponBuiltCount++;
    }

    void AddSprayWeapon(bool isNew, WeaponSprayer sprayer)
    {
        sprayer.customePartFilePath = (FileManager.rootDir + FileManager.sprayWeaponDir + sprayer.name + ".txt");
        sprayers.Add(sprayer);
        foreach (GameObject content in sprayerContent)
        {
            for (int i = sprayerBuiltCount; i < sprayers.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(sprayers[i], slotDetector, 22.6f);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(sprayer); }
        sprayerBuiltCount++;
    }

    void AddTargetingSystem(bool isNew, AdvancedTargetingSystem targetingSystem)
    {
        targetingSystem.customePartFilePath = (FileManager.rootDir + FileManager.targetingSystemDir + targetingSystem.name + ".txt");
        advancedTargetingSystems.Add(targetingSystem);
        foreach (GameObject content in advancedTargetingContent)
        {
            for (int i = advancedTargetingBuiltCount; i < advancedTargetingSystems.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(advancedTargetingSystems[i], slotDetector, 22.6f);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(targetingSystem); }
        advancedTargetingBuiltCount++;
    }

    void AddMeleeWeapon(bool isNew, WeaponMelee meleeWeapon)
    {
        meleeWeapon.customePartFilePath = (FileManager.rootDir + FileManager.meleeWeaponDir + meleeWeapon.name + ".txt");
        meleeWeapons.Add(meleeWeapon);
        foreach (GameObject content in meleeContent)
        {
            for (int i = meleeBuiltCount; i < meleeWeapons.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(meleeWeapons[i], slotDetector, 22.6f);
            }
        }
        if (isNew) { FileManager.SaveTowerPart(meleeWeapon); }
        meleeBuiltCount++;
    }

    void AddTowerState(bool isNew, TowerState state)
    {
        AddState(isNew, ref state);
        AddPaths(ref state);
        if (isNew) { FileManager.SaveTowerPart(state); }
        towerStatesBuiltCount++;
        if(FMTowerPartEditor.editIsPathed) { EditPathedTower(ref state); }
        if (state.isRoot) state.SetRootsAfterTime();
    }

    void AddPaths(ref TowerState state)
	{
        state.path1 = TowerAssembler.towerAssembler.path1; state.path2 = TowerAssembler.towerAssembler.path2; state.path3 = TowerAssembler.towerAssembler.path3;
        if (state.path1.Count > 0 || state.path2.Count > 0 || state.path3.Count > 0) { state.isRoot = true; }
        else state.isRoot = false;
        TowerAssembler.towerAssembler.path1 = new List<KeyValuePair<TowerState, string>>();
        TowerAssembler.towerAssembler.path2 = new List<KeyValuePair<TowerState, string>>();
        TowerAssembler.towerAssembler.path3 = new List<KeyValuePair<TowerState, string>>();
    }

    void EditPathedTower(ref TowerState state)
	{
        switch (FMTowerPartEditor.pathNum)
        {
            case 1:  FMTowerPartEditor.root.path1.Insert(FMTowerPartEditor.pathIndex, new KeyValuePair<TowerState, string>(state, state.customePartFilePath)); break;
            case 2:  FMTowerPartEditor.root.path2.Insert(FMTowerPartEditor.pathIndex, new KeyValuePair<TowerState, string>(state, state.customePartFilePath)); break;
            case 3:  FMTowerPartEditor.root.path3.Insert(FMTowerPartEditor.pathIndex, new KeyValuePair<TowerState, string>(state, state.customePartFilePath)); break;
        }
        pathedInventorySlot = towerPartInventorySlot;
        FMTowerPartEditor.editIsPathed = false;
        FMTowerPartEditor.pathNum = 0;
        FileManager.SelectSlot(FMTowerPartEditor.root.inventorySlots[0]);
        FMTowerPartEditor.root = null;
        TowerPartSelector.forceHide = true;
        FileManager.EditPart();
        ButtonEventManager.ConfirmTower();

        FMTowerPartEditor.editIsPathed = true;
        TowerPartSelector.forceHide = false;
    }

    void AddState(bool isNew, ref TowerState state)
	{
        state.customePartFilePath = (FileManager.rootDir + FileManager.towerStateDir + state.name + ".txt");
        towerStates.Add(state);
        foreach (GameObject content in towerContent)
        {
            for (int i = towerStatesBuiltCount; i < towerStates.Count; i++)
            {
                BuildObjects(content);
                AddButtonToObject();
                AddEventToObject(true);
                towerPartInventorySlot.Initialize(towerStates[i], slotDetector, 30);
            }
        }
    }

    public void BuildObjects(GameObject inventory)
    {
        inventorySlot = new GameObject();
        slotImage = new GameObject();
        inventorySlot.transform.SetParent(inventory.transform);
        slotImage.transform.SetParent(inventorySlot.transform);
        towerPartInventorySlot = inventorySlot.AddComponent<TowerPartInventorySlot>();
        slotImage.AddComponent<RawImage>();
        slotImage.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        inventorySlot.AddComponent<BoxCollider2D>();
    }

    public void AddButtonToObject()
    {
        RectTransform rectTrans = slotImage.GetComponent<RectTransform>();
        rectTrans.offsetMin = Vector2.zero;
        rectTrans.offsetMax = Vector2.zero;
        rectTrans.anchorMin = Vector2.zero; rectTrans.anchorMax = new Vector2(1, 1); ;
        inventorySlot.AddComponent<Image>();
        Button button = inventorySlot.AddComponent<Button>();
        ColorBlock colorBlock = button.colors;
        colorBlock.selectedColor = selectedColor;
        colorBlock.normalColor = normalColor;
        colorBlock.highlightedColor = selectedColor;
        colorBlock.pressedColor = selectedColor;
        button.colors = colorBlock;
    }

    public void AddEventToObject(bool addMouseEvent)
    {
        InventoryEventTrigger inventoryEventTrigger = inventorySlot.AddComponent<InventoryEventTrigger>();
        inventoryEventTrigger.SetInventorySlot(towerPartInventorySlot);
        if(addMouseEvent) towerPartInventorySlot.AddEvents();
    }

    public void RemoveFromInventory(TowerPart part)
    {
        if (part.GetComponent<TowerState>())
        {
            towerStatesBuiltCount--;
            for (int i = 0; i < towerStates.Count;) { if (towerStates[i].name == part.name) { towerStates.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<WeaponProjectile>()) 
        {
            projectileWeaponBuiltCount--;
            for (int i = 0; i < projectileWeapons.Count;) { if (projectileWeapons[i].name == part.name) { projectileWeapons.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<WeaponSprayer>()) 
        {
            sprayerBuiltCount--;
            for (int i = 0; i < sprayers.Count;) { if (sprayers[i].name == part.name) { sprayers.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<WeaponMelee>())
        {
            meleeBuiltCount--;
            for (int i = 0; i < meleeWeapons.Count;) { if (meleeWeapons[i].name == part.name) { meleeWeapons.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<AdvancedTargetingSystem>())
        {
            advancedTargetingBuiltCount--;
            for (int i = 0; i < advancedTargetingSystems.Count;) { if (advancedTargetingSystems[i].name == part.name) { advancedTargetingSystems.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<Projectile>())
        {
            projectileAmmunitionBuiltCount--;
            for (int i = 0; i < projectileAmmunition.Count;) { if (projectileAmmunition[i].name == part.name) { projectileAmmunition.RemoveAt(i); } else i++; }
        }
        else if (part.GetComponent<Spray>()) 
        {
            sprayAmmunitionBuiltCount--;
            for (int i = 0; i < sprayAmmunition.Count;) { if (sprayAmmunition[i].name == part.name) { sprayAmmunition.RemoveAt(i); } else i++; }
        }
    }
}