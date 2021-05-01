using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerAssembler : MonoBehaviour
{
    [HideInInspector]public static TowerAssembler towerAssembler;
    [SerializeField]float rotateSpeed = 0;
    [SerializeField] Camera blankView = null;
    [SerializeField] GameObject ammoSlotImage = null;
    [SerializeField] float mountHeightAdjustment = 0;
    [SerializeField] TowerPartSelector styleSelector = null;
    [SerializeField] MountContainer mountContainer = null;
    [SerializeField] Camera towerStateCamera = null;
    [SerializeField] Light lightPrefab = null;
    Ammo ammoSlotAmmo = null;
    Ammo ammoModel = null;
    TowerBase towerBase = null;
    GameObject towerBaseObject = null;
    WeaponMount weaponMount = null;
    GameObject weaponMountObject = null;
    WeaponMountStyle weaponMountStyle = null;
    GameObject weaponMountStyleObject = null;
    GameObject weaponObject = null;
    Weapon weaponModel = null;
    bool hideStyle = false;
    bool ammoOverSlot = false;
    float originalRotateSpeed = 0;
    Vector3 modelRotation = Vector3.zero;
    PartSize size = PartSize.Medium;
    bool stopRotation = false;
    readonly float snapAngle = 22.5f;
    GameObject tempTower;

    public List<KeyValuePair<TowerState, string>> path1 = new List<KeyValuePair<TowerState, string>>();
    public List<KeyValuePair<TowerState, string>> path2 = new List<KeyValuePair<TowerState, string>>();
    public List<KeyValuePair<TowerState, string>> path3 = new List<KeyValuePair<TowerState, string>>();

    private void Start()
    {
        originalRotateSpeed = rotateSpeed;
        if(towerAssembler != null) { Destroy(gameObject); }
        towerAssembler = this;
    }
    private void Update()
    {
        if (!ammoSlotAmmo) ammoSlotImage.GetComponentInChildren<RawImage>().texture = new RenderTexture(blankView.targetTexture); 
        float newAngle;
        if (!stopRotation)
        {
            newAngle = modelRotation.y = modelRotation.y + (rotateSpeed * Time.deltaTime);
            if (towerBaseObject != null) { towerBaseObject.transform.eulerAngles = new Vector3(0, newAngle, 0); }
            if (weaponMountStyle != null) { weaponMountStyle.transform.eulerAngles = new Vector3(0, newAngle, 0); }
            if (weaponMount != null) { weaponMount.transform.eulerAngles = new Vector3(0, newAngle, 0); }
        }
    }

    public void ChangeBaseSize(PartSize size)
    {
        this.size = size;
        towerBaseObject.GetComponent<TowerPart>().SetSize(size);
        weaponMountObject.GetComponent<TowerPart>().SetSize(size);
        weaponMountStyleObject.GetComponent<TowerPart>().SetSize(size);
    }

    public PartSize GetPartSize() { return size; }

    public void ConfirmTower(bool isNew, string towerStateName, bool test)
    {
        GameObject lightObject = Instantiate(lightPrefab.gameObject);
        GameObject cameraObject = Instantiate(towerStateCamera.gameObject);
        cameraObject.GetComponent<Camera>().targetTexture = new RenderTexture(cameraObject.GetComponent<Camera>().targetTexture);
        GameObject mountContainerObject = Instantiate(mountContainer.gameObject);
        tempTower = new GameObject();
        TowerState state = tempTower.AddComponent<TowerState>();
        state.startingConstructionTime = int.Parse(UIElements.ConstructionTimeIP.text);
        
        weaponMountStyleObject.SetActive(true);
        foreach(WeaponMountSlot w in weaponMountStyleObject.GetComponent<WeaponMountStyle>().GetSlots()) { w.GetWeapon().gameObject.SetActive(true); }
        AssempleWeaponMount(ref mountContainerObject);
        AssembleTower(ref state, ref mountContainerObject, ref cameraObject, ref lightObject);
        NameTowerParts(ref state, ref mountContainerObject);
        state.name = towerStateName;
        if (!test)
        {
            InventoryManager.inventoryManager.AddPart(isNew, state);
            Destroy(tempTower);
            tempTower = null;
        }
    }

    public void UnpackTower()
    {
        tempTower.GetComponentInChildren<TowerBase>().SendPartToAssembler();
        tempTower.GetComponentInChildren<WeaponMount>().SendPartToAssembler();
        tempTower.GetComponentInChildren<WeaponMountStyle>().SendPartToAssembler();
        foreach (WeaponMountSlot slot in tempTower.GetComponentInChildren<WeaponMountStyle>().GetSlots())
        {
            AttachedWeapon(slot.GetWeapon(), slot.slotNumber, 22.6f, false);
        }
        Destroy(tempTower);
        tempTower = null;
    }

    public TowerState GetTempTower() { return tempTower.GetComponent<TowerState>(); }

        void AssembleTower(ref TowerState state, ref GameObject mountContainerObject, ref GameObject cameraObject, ref GameObject lightObject)
        {
        towerBaseObject.transform.SetParent(state.transform);
        towerBaseObject.transform.localPosition = Vector3.zero;

        lightObject.transform.SetParent(state.transform);
        lightObject.transform.localPosition = new Vector3(0, 12, 0);

        mountContainerObject.transform.SetParent(state.transform);

        cameraObject.transform.SetParent(state.transform);
        cameraObject.transform.localPosition = new Vector3(0, 13, -20);
        cameraObject.GetComponent<Camera>().orthographicSize = 10;

        state.SetPartType(TowerPartType.TowerState);
        state.SetBase(towerBaseObject.GetComponent<TowerBase>());
        state.SetMount(weaponMountObject.GetComponent<WeaponMount>());
        state.SetView(cameraObject.GetComponent<Camera>());
        GameObject[] rotateParts = { mountContainerObject, towerBaseObject };
        state.rotatableParts = rotateParts;
    }

        void NameTowerParts(ref TowerState state, ref GameObject mountContainerObject)
    {
        weaponMountStyleObject.name = "WeaponMountStyle";
        weaponMountObject.name = "WeaponMount";
        towerBaseObject.name = "TowerBase";
        mountContainerObject.name = "MountContainer";
    }

        void AssempleWeaponMount(ref GameObject mountContainerObject)
    {
        weaponMountStyleObject.transform.SetParent(weaponMountObject.transform);
        weaponMountObject.GetComponent<WeaponMount>().SetStyle(weaponMountStyleObject.GetComponent<WeaponMountStyle>());
        weaponMountObject.transform.SetParent(mountContainerObject.transform);
        weaponMountObject.transform.localPosition = new Vector3(0, 1.5f, 0);
        weaponMountObject.GetComponent<WeaponMount>().SetAccessories
        (
            mountContainerObject.GetComponent<MountContainer>().GetHome(),
            mountContainerObject.GetComponent<MountContainer>().GetRangeIndicator(),
            mountContainerObject.GetComponent<MountContainer>().GetAngleIndicator(),
            mountContainerObject.GetComponent<MountContainer>().GetTargetingBeam()
        );
    }

    public void ConfirmWeapon()
    {
        if(weaponModel.GetComponent<WeaponProjectile>()) weaponModel.GetComponent<WeaponProjectile>().SetAmmo(ammoSlotAmmo);
        if (weaponModel.GetComponent<WeaponSprayer>()) weaponModel.GetComponent<WeaponSprayer>().SetAmmo(ammoSlotAmmo);
        InventoryManager.inventoryManager.AddPart(true, weaponModel);
    }

    public void ConfirmAmmo() { InventoryManager.inventoryManager.AddPart(true, ammoModel); ammoModel = null; }

    public bool AmmoAssigned()
    {
        AdvancedTargetingSystem targetingSystem = weaponModel.GetComponent<AdvancedTargetingSystem>();
        if (ammoSlotAmmo == null && !targetingSystem) return false;
        else return true;
    }

    public bool AllSlotsHaveWeapons()
    {
        bool allSlotsHaveWeapons = true;
        for(int i = 0; i < weaponMountStyle.GetSlots().Length; i++) { if (weaponMountStyle.GetSlots()[i].GetWeapon() == null) allSlotsHaveWeapons = false; }
        return allSlotsHaveWeapons;
    }

    public bool WeaponsTouching()
	{
        foreach(WeaponMountSlot slot in weaponMountStyleObject.GetComponent<WeaponMountStyle>().GetSlots()) { if (slot.GetWeapon().IsTouchingWeapon()) return true; }
        return false;
	}

    public void ClearAmmo() { ammoSlotAmmo = null; }
    public bool AmmoOverSlot() { return ammoOverSlot; }
    public void EnterAmmoSlot() { ammoOverSlot = true; }
    public void ExitAmmoSlot() { ammoOverSlot = false; }

    public void StopRotation() 
    {
        styleSelector.StopRotation(); rotateSpeed = 0; stopRotation = true;
        if (towerBaseObject != null) { towerBaseObject.transform.eulerAngles = new Vector3(0, 0, 0); }
        if (weaponMountStyle != null) { weaponMountStyle.transform.eulerAngles = new Vector3(0, 0, 0); }
        if (weaponMount != null) { weaponMount.transform.eulerAngles = new Vector3(0, 0, 0); }
    }
    public void StartRotation() { styleSelector.StartRotation(); rotateSpeed = originalRotateSpeed; stopRotation = false; }

    public void ShowMountStyle(bool hide) {if (weaponMountStyle != null){ weaponMountStyle.gameObject.SetActive(hide);} hideStyle = hide; }

    public void ClearAssembler()
    {
        towerBase = null;
        if(towerBaseObject)Destroy(towerBaseObject);
        weaponMount = null;
        if(weaponMountObject)Destroy(weaponMountObject);
        weaponMountStyle = null;
        if(weaponMountStyleObject)Destroy(weaponMountStyleObject);
    }

    public void AttachedWeapon(Weapon weaponIn, int slotNumber, float cameraDistance, bool isNew)
    {
        if (weaponMountStyle.GetSlots()[slotNumber].GetWeapon() != null) Destroy(weaponMountStyle.GetSlots()[slotNumber].GetWeapon().gameObject);
        weaponObject = Instantiate(weaponIn).gameObject;
        weaponObject.GetComponent<Weapon>().SetIsPreview(false);
        weaponObject.GetComponent<TowerPart>().SetHide(false);
        weaponObject.GetComponent<TowerPart>().SetShrink(false);
        weaponObject.SetActive(hideStyle);
        weaponObject.transform.SetParent(weaponMountStyle.GetSlots()[slotNumber].transform, true);
        weaponMountStyle.GetSlots()[slotNumber].SetWeapon(weaponObject.GetComponent<Weapon>());
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.GetComponent<Weapon>().ScaleToSize();
        if (isNew) { StartCoroutine(SelectWeaponRotation(slotNumber, cameraDistance)); }
        else { weaponObject.transform.localRotation = weaponObject.GetComponent<Weapon>().savedLocalRotation; }
    }

    public void LoadAmmoSlot(ref Ammo ammo) 
    {
        RawImage rawImage = ammoSlotImage.GetComponentInChildren<RawImage>();
        rawImage.texture = ammo.GetView().targetTexture;
        ammoSlotAmmo = ammo; 
    }

    IEnumerator SelectWeaponRotation(int slotNumber, float cameraDistance)
    {
        while(!Input.GetMouseButtonDown(0))
        {
            Camera tempCamera = weaponMountStyle.GetSlots()[slotNumber].GetWeapon().GetComponentInChildren<Camera>();
            if (tempCamera)tempCamera.gameObject.SetActive(false);

            Vector3 temp = Input.mousePosition;
            temp.z = cameraDistance; // Set this to be the distance you want the object to be placed in front of the camera.
            temp = Camera.main.ScreenToWorldPoint(temp);
            weaponMountStyle.GetSlots()[slotNumber].GetWeapon().transform.LookAt(temp);

            Weapon tempWeapon = weaponMountStyle.GetSlots()[slotNumber].GetWeapon();
            if (!Input.GetKey(KeyCode.LeftAlt)) tempWeapon.transform.rotation = new Quaternion(0, tempWeapon.transform.rotation.y, 0, tempWeapon.transform.rotation.w);
            else tempWeapon.transform.eulerAngles = GetSnapRotation(tempWeapon.transform.eulerAngles.y);
            tempWeapon.CorrectRotation();
            yield return null;
        }
        weaponMountStyle.GetSlots()[slotNumber].GetWeapon().savedLocalRotation = weaponMountStyle.GetSlots()[slotNumber].GetWeapon().transform.localRotation;
        if (weaponMountStyle.GetSlots()[slotNumber].GetWeapon().IsTouchingWeapon())
        {
            Destroy(weaponMountStyle.GetSlots()[slotNumber].GetWeapon().gameObject);
            weaponMountStyle.GetSlots()[slotNumber].SetWeapon(null);
        }
    }

    Vector3 GetSnapRotation(float current_Y)
    {
        int new_y = (int)Mathf.Round(current_Y / snapAngle);
        Vector3 newRotation = new Vector3(0, new_y * snapAngle, 0);

        return newRotation;
    }

    public void ChoosePart(TowerPart part)
    {
        if (part.GetComponentInChildren<Camera>())part.GetComponentInChildren<Camera>().gameObject.SetActive(false);
        TowerPartType towerPart = part.GetPartType();
        switch(towerPart)
        {
            case TowerPartType.Base: 
                ChooseBase(ref part);
                break;
            case TowerPartType.Mount:
                ChooseMount(ref part);
                break;
            case TowerPartType.MountStyle: ChooseMountStyle(ref part); break;
            case TowerPartType.Weapon: 
                weaponModel =  part.GetComponent<Weapon>();
                break;
            case TowerPartType.Ammo:
                ammoModel = part.GetComponent<Ammo>();
                break;
        }
    }

    void ChooseBase(ref TowerPart part )
    {
        if (towerBase != null) Destroy(towerBase.gameObject);
        AssignPartToProperties(ref part, ref towerBaseObject);
        towerBaseObject.transform.position = transform.position;
        towerBase = towerBaseObject.GetComponent<TowerBase>();
    }

    void ChooseMount(ref TowerPart part)
    {
        if (weaponMount != null) Destroy(weaponMount.gameObject);
        AssignPartToProperties(ref part, ref weaponMountObject);
        weaponMountObject.transform.position = new Vector3(transform.position.x, transform.position.y + mountHeightAdjustment, transform.position.z);
        weaponMount = weaponMountObject.GetComponent<WeaponMount>();
        //prevent duplicated styles while editing
        if (weaponMount.GetComponentInChildren<WeaponMountStyle>()) { Destroy(weaponMount.GetComponentInChildren<WeaponMountStyle>().gameObject); }
    }

    void ChooseMountStyle(ref TowerPart part)
    {
        if (weaponMountStyle != null) Destroy(weaponMountStyle.gameObject);
        AssignPartToProperties(ref part, ref weaponMountStyleObject);
        weaponMountStyleObject.transform.position = new Vector3(transform.position.x, transform.position.y + mountHeightAdjustment, transform.position.z);
        weaponMountStyle = weaponMountStyleObject.GetComponent<WeaponMountStyle>();
        weaponMountStyleObject.SetActive(hideStyle);
        //Prevent duplicated weapons while editing
        weaponMountStyleObject.GetComponent<WeaponMountStyle>().ClearSlots();
    }

    public void SetMountProperties()
    {
        weaponMountObject.GetComponent<WeaponMount>().RotationalSpeed = int.Parse(UIElements.baseRotSpeedIP.text);
        weaponMountObject.GetComponent<WeaponMount>().Range = int.Parse(UIElements.baseRangeIP.text);
        weaponMountObject.GetComponent<WeaponMount>().TurretAngle = (TurretAngle)UIElements.baseTurnRadDD.value;
    }

    public void SetWeaponProperties()
    {
        if (weaponModel.GetComponent<WeaponProjectile>()) SetProjectileWeaponProperties();
        else if (weaponModel.GetComponent<WeaponSprayer>()) SetSprayerWeaponProperties();
        else if (weaponModel.GetComponent<WeaponMelee>()) weaponModel.GetComponent<WeaponMelee>().Damage = int.Parse(UIElements.meleeWepDamageIP.text);
        else if (weaponModel.GetComponent<AdvancedTargetingSystem>()) SetTargetingWeaponProperties();
    }

    public void SetAmmoProperties()
    {
        if (ammoModel.GetComponent<Projectile>()) SetProjectileAmmoProperties();
        else if (ammoModel.GetComponent<Spray>()) SetSprayAmmoProperties();
    }

    void SetProjectileWeaponProperties()
    {
        WeaponProjectile barrel = weaponModel.GetComponent<WeaponProjectile>();
        barrel.FireRate = int.Parse(UIElements.projWepRateIP.text);
        barrel.RotationSpeed = int.Parse(UIElements.projWepRotSpeedIP.text);
        barrel.Recoil = (Recoil)UIElements.projWepRecoilDD.value;
        barrel.Accuracy = (Accuracy)UIElements.projWepAccuracyDD.value;
        barrel.CanShootUp = UIElements.projWepTargetAirTog.isOn;
        barrel.CanShootDown = UIElements.projWepTargetGroundTog.isOn;
        weaponModel = barrel;
    }

    void SetSprayerWeaponProperties()
    {
        WeaponSprayer sprayer = weaponModel.GetComponent<WeaponSprayer>();
        sprayer.FireRate = int.Parse(UIElements.sprayWepRateIP.text);
        sprayer.CanShootUp = UIElements.sprayWepTargetAirTog.isOn;
        sprayer.CanShootDown = UIElements.sprayWepTargetGroundTog.isOn;
        weaponModel = sprayer;
    }

    void SetTargetingWeaponProperties()
    {
        AdvancedTargetingSystem targetingSystem = weaponModel.GetComponent<AdvancedTargetingSystem>();
        targetingSystem.WeaknessTargetingLevel = (WeaknessTargetingLevel)UIElements.targetWeaknessLevelDD.value;
        targetingSystem.MovementTypePriorityTargeting = UIElements.targetBasicTog.isOn;
        targetingSystem.AdvancedPositionPriorityTarget = UIElements.targetAdvancedTog.isOn;
        targetingSystem.CanDetectStealth = UIElements.targetDetectStealthTog.isOn;
        weaponModel = targetingSystem;
    }

    void SetProjectileAmmoProperties()
    {
        Projectile ammo = ammoModel.GetComponent<Projectile>();
        ammo.Damage = int.Parse(UIElements.projAmmoDamageIP.text);
        ammo.Dot = int.Parse(UIElements.projAmmDOTIP.text);
        ammo.DotTime = int.Parse(UIElements.projAmmDOTDurIP.text);
        ammo.DotTicsPerSec = int.Parse(UIElements.projAmmDOTRateIP.text);
        ammo.Penatration = int.Parse(UIElements.projAmmPenIP.text);
        ammo.Speed = int.Parse(UIElements.projAmmSpeedIP.text);
        ammo.TravelTime = int.Parse(UIElements.projAmmLifeIP.text);
        ammo.AOE = int.Parse(UIElements.projRadIP.text);
        ammo.Homing = int.Parse(UIElements.projHomIP.text);
        ammo.DamageType = (WeaknessPriority)UIElements.projDamTypeDD.value;
        ammoModel = ammo;
    }

    void SetSprayAmmoProperties()
    {
        Spray ammo = ammoModel.GetComponent<Spray>();
        ammo.Damage = int.Parse(UIElements.sprayAmmoDamageIP.text);
        ammo.Dot = int.Parse(UIElements.sprayAmmDOTIP.text);
        ammo.DotTime = int.Parse(UIElements.sprayAmmDOTDurIP.text);
        ammo.DotTicsPerSec = int.Parse(UIElements.sprayAmmDOTRateIP.text);
        ammo.Penatration = int.Parse(UIElements.sprayAmmPenIP.text);
        ammo.DamageType = (WeaknessPriority)UIElements.sprayDamTypeDD.value;
        ammoModel = ammo;
    }

    void AssignPartToProperties(ref TowerPart newPart, ref GameObject partObject)
    {
        partObject = Instantiate(newPart, transform).gameObject;
        partObject.GetComponent<TowerPart>().SetHide(false);
        partObject.GetComponent<TowerPart>().SetShrink(false);
        partObject.GetComponent<TowerPart>().SetSize(size);
    }

    public TowerBase GetTowerBase() { return towerBaseObject.GetComponent<TowerBase>(); }
    public WeaponMount GetWeaponMount() { return weaponMountObject.GetComponent<WeaponMount>(); }
    public WeaponMountStyle GetWeaponMountStyle() { return weaponMountStyleObject.GetComponent<WeaponMountStyle>(); }
    public Weapon GetWeapon() { return weaponModel; }
    public Ammo GetAmmo() { return ammoModel; }
    public Ammo GetLoadedAmmo() { return ammoSlotAmmo; }
}