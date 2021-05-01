using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
using UnityEngine.EventSystems; 
using System.IO; 
using System.Text; 

public class FileManager : MonoBehaviour
{
    public static FileManager fileManager = null;

    public static TowerPartInventorySlot selectedSlot = null;
    

    public static readonly string rootDir = "Assets/Resources/Prefabs/PlayerMadePrefabs/";
    public static readonly string projectileAmmoDir = "Ammo/ProjectileAmmo/";
    public static readonly string sprayAmmoDir = "Ammo/SprayAmmo/";
    public static readonly string projectileWeaponDir = "Weapons/ProjectileWeapons/";
    public static readonly string sprayWeaponDir = "Weapons/SprayerWeapons/";
    public static readonly string targetingSystemDir = "Weapons/AdvancedTargetingSystems/";
    public static readonly string meleeWeaponDir = "Weapons/MeleeWeapons/";
    public static readonly string towerStateDir = "TowerStates/";
    public static readonly string directoryListFileName = "WhatsInThisDirectory.txt";

    public static bool forceDelete = false;
    static bool adjustingTowerSize = false;
    public static string editedPartsPriorCustomFilePath = "";
    static readonly int key = 545;
    static bool isSelected = false;
    public static TowerPart partToBeDeleted = null;
    public static int editedSlotSibIndex = 0;

    private void Awake() { fileManager = this; }

    private void Start()
    {
        LoadTowerPart(projectileAmmoDir);
        LoadTowerPart(sprayAmmoDir);
        LoadTowerPart(projectileWeaponDir);
        LoadTowerPart(sprayWeaponDir);
        LoadTowerPart(targetingSystemDir);
        LoadTowerPart(meleeWeaponDir);
        LoadTowerPart(towerStateDir);
        foreach(TowerState state in InventoryManager.inventoryManager.GetTowerStates()) { if (state.isRoot) { state.FindPaths(); } }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.P)
            && Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.M))
        {
            ClearAllCustomTowerFiles();
            Debug.Log("All Custom Tower Files Deleted");
        }
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected != null && isSelected) { if (!currentSelected.GetComponent<TowerPartInventorySlot>()) { StartCoroutine(DeselectSlot()); } }
        else if(isSelected){ StartCoroutine(DeselectSlot()); }
    }

    IEnumerator DeselectSlot()
	{
        isSelected = false;
        yield return new WaitForSeconds(0.2f);
         selectedSlot = null;
        UIElements.partNameText.text = ""; StatWindow.statWindow.ClearStats();
    }

    public static void SaveTowerPart(TowerPart part) { if (part.GetComponent<TowerState>()) SaveTowerState(part); else SavePart(part); }

    static void SaveTowerState(TowerPart part)
    {
        StreamWriter streamWriter = new StreamWriter(part.customePartFilePath);
        streamWriter.WriteLine(part.name);
        TowerBase towerBase = part.GetComponentInChildren<TowerBase>();
        streamWriter.WriteLine(part.GetComponent<TowerState>().startingConstructionTime);
        streamWriter.WriteLine(towerBase.GetPrefabFilePath());
        streamWriter.WriteLine(new Vector3(towerBase.mat1.color.r, towerBase.mat1.color.g, towerBase.mat1.color.b));
        streamWriter.WriteLine(new Vector3(towerBase.mat2.color.r, towerBase.mat2.color.g, towerBase.mat2.color.b));
        streamWriter.WriteLine(new Vector3(towerBase.mat3.color.r, towerBase.mat3.color.g, towerBase.mat3.color.b));
        streamWriter.WriteLine((int)towerBase.GetSize());
        WeaponMount weaponMount = part.GetComponentInChildren<WeaponMount>();
        streamWriter.WriteLine(weaponMount.GetPrefabFilePath());
        streamWriter.WriteLine(new Vector3(weaponMount.mat1.color.r, weaponMount.mat1.color.g, weaponMount.mat1.color.b));
        streamWriter.WriteLine(new Vector3(weaponMount.mat2.color.r, weaponMount.mat2.color.g, weaponMount.mat2.color.b));
        streamWriter.WriteLine(new Vector3(weaponMount.mat3.color.r, weaponMount.mat3.color.g, weaponMount.mat3.color.b));
        WeaponMountStyle mountStyle = weaponMount.GetComponentInChildren<WeaponMountStyle>();
        streamWriter.WriteLine(mountStyle.GetPrefabFilePath());
        FMTowerPartPropertySetter.SaveMountProperties(ref streamWriter, weaponMount);

        FMTowerPartPropertySetter.SaveTowerPaths(ref streamWriter, part.GetComponent<TowerState>());
        for (int i = 0; i < mountStyle.GetSlots().Length; i++)
        {
            Weapon weapon = mountStyle.GetSlots()[i].GetWeapon();
            streamWriter.WriteLine(weapon.customePartFilePath); streamWriter.WriteLine(weapon.savedLocalRotation);
        }
        SaveFileLocation(towerStateDir, part.customePartFilePath);

        streamWriter.Dispose();
    }

    static void SavePart(TowerPart part)
    {
        StreamWriter streamWriter = new StreamWriter(part.customePartFilePath);
        if (!part.GetComponent<TowerState>())
        {
            streamWriter.WriteLine(part.GetPrefabFilePath());
            streamWriter.WriteLine(new Vector3(part.mat1.color.r, part.mat1.color.g, part.mat1.color.b));
            streamWriter.WriteLine(new Vector3(part.mat2.color.r, part.mat2.color.g, part.mat2.color.b));
            streamWriter.WriteLine(new Vector3(part.mat3.color.r, part.mat3.color.g, part.mat3.color.b));
        }
        streamWriter.WriteLine(part.name);
        if (part.GetComponent<Weapon>())
        {
            if(part.GetComponent<WeaponProjectile>())Debug.Log(part.GetComponent<WeaponProjectile>().GetAmmo().customePartFilePath);
            FMTowerPartPropertySetter.SaveWeaponProperties(ref streamWriter, part.GetComponent<Weapon>());
            streamWriter.WriteLine((int)part.GetComponent<Weapon>().GetSize());
        }
        else if (part.GetComponent<Ammo>()) FMTowerPartPropertySetter.SaveAmmoProperties(ref streamWriter, part.GetComponent<Ammo>());
        SaveFileLocation(GetPartDirectory(part), part.customePartFilePath);
        streamWriter.Dispose();
    }

    public static void LoadTowerPart(string partDirectory)
    {
        if (File.Exists(rootDir + partDirectory + directoryListFileName))
        {
            StreamReaderPro dirReader = new StreamReaderPro(rootDir + partDirectory + directoryListFileName);
            while ((!dirReader.EndOfStream())) { FMTowerPartPropertySetter.SetTowerPartProperties(dirReader.ReadLine(), partDirectory, true); }
        }
    }

    public static string EncryptDecrypt(string textToEncrypt)
    {
        StringBuilder inSb = new StringBuilder(textToEncrypt);
        StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
        char c;
        for (int i = 0; i < textToEncrypt.Length; i++)
        {
            c = inSb[i];
            c = (char)(c ^ key);
            outSb.Append(c);
        }
        return outSb.ToString();
    }

    void ClearAllCustomTowerFiles()
    {
        List<string> filePaths = new List<string>();
        AddFilePathsFromDirectory(ref filePaths, projectileAmmoDir);
        AddFilePathsFromDirectory(ref filePaths, sprayAmmoDir);
        AddFilePathsFromDirectory(ref filePaths, projectileWeaponDir);
        AddFilePathsFromDirectory(ref filePaths, sprayWeaponDir);
        AddFilePathsFromDirectory(ref filePaths, targetingSystemDir);
        AddFilePathsFromDirectory(ref filePaths, meleeWeaponDir);
        AddFilePathsFromDirectory(ref filePaths, towerStateDir);
        foreach (string s in filePaths) if (File.Exists(s)) File.Delete(s);
        //public readonly string pathedTowersDir = "PathedTowers";
    }

    static void AddFilePathsFromDirectory(ref List<string> filePaths, string directory)
    {
        if (File.Exists(rootDir + directory + directoryListFileName))
        {
            StreamReader reader = File.OpenText(rootDir + directory + directoryListFileName);
            while (!reader.EndOfStream) { filePaths.Add(reader.ReadLine()); }
            reader.Dispose();
            File.Delete(rootDir + directory + directoryListFileName);
        }
    }

    static void SaveFileLocation(string partDirectory, string pathIn)
    {
        StreamWriter streamWriter = File.AppendText(rootDir + partDirectory + directoryListFileName);
        streamWriter.WriteLine(pathIn);
        streamWriter.Dispose();
    }

    public static string GetPartDirectory(TowerPart part)
    {
        if (part.GetComponent<TowerState>() != null) return towerStateDir;
        else if (part.GetComponent<Projectile>() != null) return projectileAmmoDir;
        else if (part.GetComponent<Spray>() != null) return sprayAmmoDir;
        else if (part.GetComponent<WeaponProjectile>() != null) return projectileWeaponDir;
        else if (part.GetComponent<WeaponSprayer>() != null) return sprayWeaponDir;
        else if (part.GetComponent<WeaponMelee>() != null) return meleeWeaponDir;
        else if (part.GetComponent<AdvancedTargetingSystem>() != null) return targetingSystemDir;
        return "";
    }

    public static void SelectSlot(TowerPartInventorySlot newSlot) 
    {
        if (newSlot != selectedSlot) 
        {
            isSelected = true;
            selectedSlot = newSlot; 
            UIElements.partNameText.text = newSlot.GetTowerPart().name;
            if (newSlot.GetTowerPart().GetComponent<TowerState>()) StatWindow.statWindow.DisplayTowerStats(newSlot.GetTowerPart().GetComponent<TowerState>());
            else if (newSlot.GetTowerPart().GetComponent<WeaponProjectile>()) StatWindow.statWindow.DisplayProjectileWeaponStats(newSlot.GetTowerPart().GetComponent<WeaponProjectile>());
            else if (newSlot.GetTowerPart().GetComponent<WeaponSprayer>()) StatWindow.statWindow.DisplaySprayerWeaponStats(newSlot.GetTowerPart().GetComponent<WeaponSprayer>());
            else if (newSlot.GetTowerPart().GetComponent<WeaponMelee>()) StatWindow.statWindow.DisplayMeleeWeaponStats(newSlot.GetTowerPart().GetComponent<WeaponMelee>());
            else if (newSlot.GetTowerPart().GetComponent<AdvancedTargetingSystem>()) StatWindow.statWindow.DisplayTargetingSystemStats(newSlot.GetTowerPart().GetComponent<AdvancedTargetingSystem>());
            else if (newSlot.GetTowerPart().GetComponent<Projectile>()) StatWindow.statWindow.DisplayProjectileStats(newSlot.GetTowerPart().GetComponent<Projectile>());
            else if (newSlot.GetTowerPart().GetComponent<Spray>()) StatWindow.statWindow.DisplaySprayStats(newSlot.GetTowerPart().GetComponent<Spray>());
        }
    }

    public static void DeleteTowerPart()
    {
        bool delete = false;
        StreamReader reader = File.OpenText(rootDir + GetPartDirectory(partToBeDeleted) + directoryListFileName);
        List<string> parts = new List<string>();
        while (!reader.EndOfStream) { parts.Add(reader.ReadLine()); }
        reader.Dispose();
        if (!AmmoIsInUse() || forceDelete)
        {
            if (!WeaponIsInUse() || forceDelete) { delete = true; }
            else { UIElements.PromptErrorMessage("This weapon is being used by a tower and cannot be deleted"); }
        }
        else { UIElements.PromptErrorMessage("This Ammunition is being used by a weapon and cannot be deleted"); }
        if (partToBeDeleted.GetComponent<TowerState>())
        {
            delete = true;
            if (partToBeDeleted.GetComponent<TowerState>().IsPathed() && !forceDelete)
            {
                delete = false;
                string tower = FindTowerRoot(partToBeDeleted.GetComponent<TowerState>());
                UIElements.PromptErrorMessage("This Tower belongs to the path of " + tower + " and cannot be deleted");
            }
            if(partToBeDeleted.GetComponent<TowerState>().isRoot && !forceDelete)
			{
                foreach (KeyValuePair<TowerState, string> pair in partToBeDeleted.GetComponent<TowerState>().path1)  { pair.Key.ClearRoot();  }
                foreach (KeyValuePair<TowerState, string> pair in partToBeDeleted.GetComponent<TowerState>().path2)  { pair.Key.ClearRoot();  }
                foreach (KeyValuePair<TowerState, string> pair in partToBeDeleted.GetComponent<TowerState>().path3)  { pair.Key.ClearRoot();  }
            }
        }
        if(delete)DeleteTowerPart(ref parts);
        forceDelete = false;
        partToBeDeleted = null;
    }

    public static void PromptDelete() { UIElements.deleteConfirmCanvas.SetActive(selectedSlot != null); if(selectedSlot)partToBeDeleted = selectedSlot.GetTowerPart(); }

    static void DeleteTowerPart(ref List<string> parts)
	{
        InventoryManager.inventoryManager.RemoveFromInventory(partToBeDeleted);
        //remove part from directory file
        for (int i = 0; i < parts.Count; i++) { if (parts[i] == partToBeDeleted.customePartFilePath) parts.RemoveAt(i); }
        if (parts.Count > 0)
        {
            StreamWriter writer = new StreamWriter(rootDir + GetPartDirectory(partToBeDeleted) + directoryListFileName);
            foreach (string s in parts) writer.WriteLine(s);
            writer.Dispose();
        }
        else { File.Delete(rootDir + GetPartDirectory(partToBeDeleted) + directoryListFileName); }
        File.Delete(partToBeDeleted.customePartFilePath);
        if (partToBeDeleted.inventorySlots[0]) { partToBeDeleted.DestroySlotsThenSelf();  }
        else { Destroy(partToBeDeleted.gameObject); Debug.Log(partToBeDeleted.name + " has no slots"); }
        UIElements.deleteConfirmCanvas.SetActive(false);
    }

    static bool AmmoIsInUse()
	{
        if (!partToBeDeleted.GetComponent<Ammo>()) return false;
        List<string> projectileWeaponPaths = new List<string>();
        StreamReader weaponPathReader = null;
        if (partToBeDeleted.GetComponent<Projectile>())
        {
            if (!File.Exists(rootDir + projectileWeaponDir + directoryListFileName)) return false;
            weaponPathReader = File.OpenText(rootDir + projectileWeaponDir + directoryListFileName);
        }
        else if (partToBeDeleted.GetComponent<Spray>())
        {
            if (!File.Exists(rootDir + sprayWeaponDir + directoryListFileName)) return false;
            weaponPathReader = File.OpenText(rootDir + sprayWeaponDir + directoryListFileName); 
        }
        while (!weaponPathReader.EndOfStream) { projectileWeaponPaths.Add( weaponPathReader.ReadLine()); }
        weaponPathReader.Dispose();
        foreach(string weaponPath in projectileWeaponPaths)
		{
            StreamReader weaponReader = File.OpenText(weaponPath);
            weaponReader.ReadLine();
            weaponReader.ReadLine();
            if (weaponReader.ReadLine() == partToBeDeleted.customePartFilePath) { weaponReader.Dispose(); return true; }
            weaponReader.Dispose();
        }
        return false;
	}

    static bool WeaponIsInUse()
    {
        if (!partToBeDeleted.GetComponent<Weapon>()) return false;
        if (!File.Exists(rootDir + towerStateDir + directoryListFileName)) return false;
        List<string> towerFilePaths = new List<string>();
        StreamReader towerPathReader = File.OpenText(rootDir + towerStateDir + directoryListFileName);
        while (!towerPathReader.EndOfStream) { towerFilePaths.Add(towerPathReader.ReadLine()); }
        towerPathReader.Dispose();
        foreach (string towerPath in towerFilePaths)
        {
            StreamReader towerReader = File.OpenText(towerPath);
            string tower = towerReader.ReadToEnd();
            towerReader.Dispose();
            if (tower.Contains(partToBeDeleted.customePartFilePath)) return true;
        }
        return false;
    }

    static void StoreTowerPaths
    (ref TowerState state, ref List<KeyValuePair<TowerState, string>> path1, ref List<KeyValuePair<TowerState, string>> path2, ref List<KeyValuePair<TowerState, string>> path3)
	{
        path1 = state.path1;
        path2 = state.path2;
        path3 = state.path3;
        state.GetComponent<TowerState>().path1 = new List<KeyValuePair<TowerState, string>>();
        state.GetComponent<TowerState>().path2 = new List<KeyValuePair<TowerState, string>>();
        state.GetComponent<TowerState>().path3 = new List<KeyValuePair<TowerState, string>>();
    }

    static void RestoreTowerPaths
    (ref TowerState state, ref List<KeyValuePair<TowerState, string>> path1, ref List<KeyValuePair<TowerState, string>> path2, ref List<KeyValuePair<TowerState, string>> path3)
    {
        state.path1 = path1;
        state.path2 = path2;
        state.path3 = path3;
    }

    public static void DuplicatePart()
	{
        if(selectedSlot)
		{
            List<KeyValuePair<TowerState, string>> path1 = new List< KeyValuePair<TowerState, string>> ();
            List<KeyValuePair<TowerState, string>> path2 = new List<KeyValuePair<TowerState, string>>();
            List<KeyValuePair<TowerState, string>> path3 = new List<KeyValuePair<TowerState, string>>();
            TowerPart part = selectedSlot.GetTowerPart();
            TowerState state = part.GetComponent<TowerState>();
            if(state) { StoreTowerPaths(ref state, ref path1, ref path2, ref path3); }
            string newPath = part.customePartFilePath;
            string oldPath = part.customePartFilePath;
            string newName = part.name;
            string oldName = part.name;
            while(File.Exists(newPath))
			{
                newPath = newPath.Insert(part.customePartFilePath.Length - 4, " Copy");
                newName = newName.Insert(part.name.Length , " Copy");
            }
            part.customePartFilePath = newPath;
            part.name = newName;
            SaveTowerPart(part);
            FMTowerPartPropertySetter.SetTowerPartProperties(part.customePartFilePath, GetPartDirectory(part), true);
            part.name = oldName;
            part.customePartFilePath = oldPath;
            if (state) RestoreTowerPaths(ref state, ref path1, ref path2, ref path3);
        }
	}

    public static void EditPart()
	{
        if (selectedSlot)
        {
            if (FMTowerPartEditor.editIsPathed) FMTowerPartEditor.rootSibIndex = editedSlotSibIndex;
            else editedSlotSibIndex = selectedSlot.transform.GetSiblingIndex();
            partToBeDeleted = selectedSlot.GetTowerPart();
            TowerPart part = selectedSlot.GetTowerPart();
            TowerState state = part.GetComponent<TowerState>();
            Weapon weapon = part.GetComponent<Weapon>();
            Ammo ammo = part.GetComponent<Ammo>();

            if (state) { FMTowerPartEditor.EditTower(ref part); }
            else if (weapon) { FMTowerPartEditor.EditWeapon(weapon); }
            else if (ammo) { FMTowerPartEditor.EditAmmo(ammo); }
            UIElements.fileNameInput.text = part.name;
            editedPartsPriorCustomFilePath = part.customePartFilePath;
            InventoryManager.inventoryManager.SetEdit();
        }
    }

    public void AdjustTowerSize() { if(!adjustingTowerSize)StartCoroutine(AdjustTowerSizeRoutine()); }

    IEnumerator AdjustTowerSizeRoutine()
    {
        adjustingTowerSize = true;
        while (TowerPartInventorySlot.states.Count > 0)
        {
            yield return null;
            SelectSlot(TowerPartInventorySlot.states[0].inventorySlots[0]);
            EditPart();
            TowerAssembler.towerAssembler.ChangeBaseSize(TowerPartInventorySlot.states[0].GetComponentInChildren<TowerBase>().GetSize());
            ButtonEventManager.ConfirmTower();
            TowerPartInventorySlot.states.RemoveAt(0);
        }
        adjustingTowerSize = false;
    }

    public void OpenTowerPath()
	{
        if (selectedSlot)
        {
            TowerState state = selectedSlot.GetTowerPart().GetComponent<TowerState>();
            if (state)
            {
                if (!state.IsPathed())
                {
                    PathBuilder.pathBuilder.SetUpPaths(ref state);
                    PathBuilder.pathBuilder.LoadTowerStateInventory();
                    CameraManager.cameraManager.ChangeState(6);
                    UIElements.pathBuilderCanvas.SetActive(true);
                    UIElements.towerInventory.SetActive(false);
                    UIElements.partNameText.gameObject.SetActive(false);
                    UIElements.statText.SetActive(false);
                }
                else
				{
                    string tower = FindTowerRoot(selectedSlot.GetTowerPart().GetComponent<TowerState>());
                    UIElements.PromptErrorMessage("This Tower belongs to the path of " + tower);
                }
            }
            else Debug.Log("state null");
        }
        else { UIElements.PromptErrorMessage("Select a tower to modify tower path"); }
	}

    static string FindTowerRoot(TowerState state)
	{
        foreach (TowerState _state in InventoryManager.inventoryManager.GetTowerStates())
        {
            foreach (KeyValuePair<TowerState, string> pair in _state.path1) if (pair.Key == state) return  _state.name;
            foreach (KeyValuePair<TowerState, string> pair in _state.path2) if (pair.Key == state) return _state.name;
            foreach (KeyValuePair<TowerState, string> pair in _state.path3) if (pair.Key == state) return _state.name;
        }
        return "";
    }
}
