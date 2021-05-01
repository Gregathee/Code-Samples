using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementSetter : MonoBehaviour
{
    [SerializeField] Toggle dontShowSizeWarning = null;
    [SerializeField] GameObject ammoSlot = null;
    [SerializeField] GameObject pathBuilderCanvas = null;
    [SerializeField] GameObject statText = null;
    [SerializeField] TMP_Text partNameText = null;
    [SerializeField] TMP_InputField fileNameInput = null;
    [SerializeField] GameObject rejectDeleteCanvas = null;
    [SerializeField] TMP_Text rejectDeleteText = null;
    [SerializeField] GameObject deleteConfirmCanvas = null;
    [Header("Part Selectors")]
    [SerializeField] TowerPartSelector baseSelector = null;
    [SerializeField] TowerPartSelector mountSelector = null;
    [SerializeField] TowerPartSelector styleSelector = null;
    [SerializeField] TowerPartSelector projectileSelector = null;
    [SerializeField] TowerPartSelector spraySelector = null;
    [SerializeField] TowerPartSelector projectileWeaponSelector = null;
    [SerializeField] TowerPartSelector sprayerWeaponSelector = null;
    [SerializeField] TowerPartSelector targetingSelector = null;
    [SerializeField] TowerPartSelector meleeSelector = null;
    [Header("Stat Inputs")]
    [SerializeField] TMP_Dropdown baseSizeDD = null;
    [SerializeField] TMP_InputField baseRotSpeedIP = null;
    [SerializeField] TMP_InputField baseRangeIP = null;
    [SerializeField] TMP_Dropdown baseTurnRadDD = null;
    [SerializeField] TMP_InputField constructionTimeIP = null;
    [Space(10)]
    [SerializeField] TMP_Dropdown projWepSizeDD = null;
    [SerializeField] TMP_InputField projWepRateIP = null;
    [SerializeField] TMP_InputField projWepRotSpeedIP = null;
    [SerializeField] GameObject projWepRotSpeedText = null;
    [SerializeField] TMP_Dropdown projWepRecoilDD = null;
    [SerializeField] TMP_Dropdown projWepAccuracyDD = null;
    [SerializeField] Toggle projWepTargetAirTog = null;
    [SerializeField] Toggle projWepTargetGroundTog = null;
    [Space(10)]
    [SerializeField] TMP_Dropdown sprayWepSizeDD = null;
    [SerializeField] TMP_InputField sprayWepRateIP = null;
    [SerializeField] Toggle sprayWepTargetAirTog = null;
    [SerializeField] Toggle sprayWepTargetGroundTog = null;
    [Space(10)]
    [SerializeField] TMP_Dropdown meleeWepSizeDD = null;
    [SerializeField] TMP_InputField meleeWepDamageIP = null;
    [SerializeField] TMP_Dropdown meleeDamageTypeDD = null;
    [Space(10)]
    [SerializeField] TMP_Dropdown targetSizeDD = null;
    [SerializeField] TMP_Dropdown targetWeaknessLevelDD = null;
    [SerializeField] Toggle targetBasicTog = null;
    [SerializeField] Toggle targetAdvancedTog = null;
    [SerializeField] Toggle targetDetectStealthTog = null;
    [SerializeField] Toggle targetPrioritizeStealthTog = null;
    [Space(10)]
    [SerializeField] TMP_InputField projAmmoDamageIP = null;
    [SerializeField] TMP_InputField projAmmDOTIP = null;
    [SerializeField] TMP_InputField projAmmDOTDurIP = null;
    [SerializeField] TMP_InputField projAmmDOTRateIP = null;
    [SerializeField] TMP_InputField projAmmPenIP = null;
    [SerializeField] TMP_InputField projAmmSpeedIP = null;
    [SerializeField] TMP_InputField projAmmLifeIP = null;
    [SerializeField] TMP_InputField projRadIP = null;
    [SerializeField] TMP_InputField projHomIP = null;
    [SerializeField] TMP_Dropdown projDamTypeDD = null;
    [Space(10)]
    [SerializeField] TMP_InputField sprayAmmoDamageIP = null;
    [SerializeField] TMP_InputField sprayAmmDOTIP = null;
    [SerializeField] TMP_InputField sprayAmmDOTDurIP = null;
    [SerializeField] TMP_InputField sprayAmmDOTRateIP = null;
    [SerializeField] TMP_InputField sprayAmmPenIP = null;
    [SerializeField] TMP_Dropdown sprayDamTypeDD = null;
    [Header("Buttons")]
    [SerializeField] GameObject modifyBaseBtn = null;
    [SerializeField] GameObject modifyMountBtn = null;
    [SerializeField] GameObject quitConfirm = null;
    [SerializeField] GameObject backButtonTower = null;
    [SerializeField] GameObject backButtonWeapon = null;
    [SerializeField] GameObject backButtonAmmo = null;
    [SerializeField] GameObject returnToAttachedWeaponsButton = null;
    [SerializeField] GameObject returnToAttachAmmoButton = null;
    [SerializeField] GameObject confirmAmmoBtn = null;
    [Header("Canvases")]
    [SerializeField] GameObject inventories = null;
    [SerializeField] GameObject towerInventory = null;
    [SerializeField] GameObject weaponInventory = null;
    [SerializeField] GameObject munitionsInventory = null;
    [Space(10)]
    [SerializeField] GameObject builders = null;
    [SerializeField] GameObject towerBuilder = null;
    [SerializeField] GameObject weaponsBuilder = null;
    [SerializeField] GameObject munitonsBuilder = null;
    [Space(10)]
    [SerializeField] GameObject towerBaseCanvas = null;
    [SerializeField] GameObject mountStyleCanvas = null;
    [SerializeField] GameObject attachWeaponCanvas = null;
    [Space(10)]
    [SerializeField] GameObject barrelCanvas = null;
    [SerializeField] GameObject sprayerCanvas = null;
    [SerializeField] GameObject advancedTargetingSystemCanvas = null;
    [SerializeField] GameObject meleeCanvas = null;
    [SerializeField] GameObject projectileCanvas = null;
    [SerializeField] GameObject sprayCanvas = null;
    [SerializeField] GameObject attachProjectileAmmoCanvas = null;
    [SerializeField] GameObject attachSprayAmmoCanvas = null;
    [Space(10)]
    [SerializeField] GameObject weaponProjectileViewPort = null;
    [SerializeField] GameObject weaponSprayViewPort = null;
    [SerializeField] GameObject meleeViewPort = null;
    [SerializeField] GameObject weaponAdvTarSysViewPort = null;
    [SerializeField] GameObject ammoProjctileViewPort = null;
    [SerializeField] GameObject ammoSprayViewPort = null;
    [Space(10)]
    [SerializeField] GameObject creationConfirmation = null;
    [SerializeField] GameObject towerConfirm = null;
    [SerializeField] GameObject weaponConfirm = null;
    [SerializeField] GameObject ammoConfirm = null;
    [SerializeField] GameObject fileNameInputBorder = null;
    [SerializeField] GameObject weaponSizeChangeCanvas = null;
    [SerializeField] GameObject firingRangeCanvas = null;

    [SerializeField] GameObject TargetWeaknessText;
    [SerializeField] GameObject TargetWeaknessBoarder;

    private void Awake()
	{
        UIElements.pathBuilderCanvas = pathBuilderCanvas;
        UIElements.statText = statText;
        UIElements.partNameText = partNameText;
        UIElements.fileNameInput = fileNameInput;
        UIElements.rejectDeleteCanvas = rejectDeleteCanvas;
        UIElements.rejectDeleteText = rejectDeleteText;
        UIElements.deleteConfirmCanvas = deleteConfirmCanvas;

        UIElements.baseSelector = baseSelector;
        UIElements.mountSelector = mountSelector;
        UIElements.styleSelector = styleSelector;
        UIElements.projectileSelector = projectileSelector;
        UIElements.spraySelector = spraySelector;
        UIElements.projectileWeaponSelector = projectileWeaponSelector;
        UIElements.sprayerWeaponSelector = sprayerWeaponSelector;
        UIElements.targetingSelector = targetingSelector;
        UIElements.meleeSelector = meleeSelector;

        UIElements.baseSizeDD = baseSizeDD;
        UIElements.baseRotSpeedIP = baseRotSpeedIP;
        UIElements.baseRangeIP = baseRangeIP;
        UIElements.baseTurnRadDD = baseTurnRadDD;
        UIElements.ConstructionTimeIP = constructionTimeIP;
        UIElements.projWepSizeDD = projWepSizeDD;
        UIElements.projWepRateIP = projWepRateIP;
        UIElements.projWepRotSpeedIP = projWepRotSpeedIP;
        UIElements.projWepRotSpeedText = projWepRotSpeedText;
        UIElements.projWepRecoilDD = projWepRecoilDD;
        UIElements.projWepAccuracyDD = projWepAccuracyDD;
        UIElements.projWepTargetAirTog = projWepTargetAirTog;
        UIElements.projWepTargetGroundTog = projWepTargetGroundTog;
        UIElements.sprayWepSizeDD = sprayWepSizeDD;
        UIElements.sprayWepRateIP = sprayWepRateIP;
        UIElements.sprayWepTargetAirTog = sprayWepTargetAirTog;
        UIElements.sprayWepTargetGroundTog = sprayWepTargetGroundTog;
        UIElements.meleeWepSizeDD = meleeWepSizeDD;
        UIElements.meleeWepDamageIP = meleeWepDamageIP;
        UIElements.meleeDamageTypeDD = meleeDamageTypeDD;
        UIElements.targetSizeDD = targetSizeDD;
        UIElements.targetWeaknessLevelDD = targetWeaknessLevelDD;
        UIElements.targetBasicTog = targetBasicTog;
        UIElements.targetAdvancedTog = targetAdvancedTog;
        UIElements.targetDetectStealthTog = targetDetectStealthTog;
        UIElements.projAmmoDamageIP = projAmmoDamageIP;
        UIElements.projAmmDOTIP = projAmmDOTIP;
        UIElements.projAmmDOTDurIP = projAmmDOTDurIP;
        UIElements.projAmmDOTRateIP = projAmmDOTRateIP;
        UIElements.projAmmPenIP = projAmmPenIP;
        UIElements.projAmmSpeedIP = projAmmSpeedIP;
        UIElements.projAmmLifeIP = projAmmLifeIP;
        UIElements.projRadIP = projRadIP;
        UIElements.projHomIP = projHomIP;
        UIElements.projDamTypeDD = projDamTypeDD;
        UIElements.sprayAmmoDamageIP = sprayAmmoDamageIP;
        UIElements.sprayAmmDOTIP = sprayAmmDOTIP;
        UIElements.sprayAmmDOTDurIP = sprayAmmDOTDurIP;
        UIElements.sprayAmmDOTRateIP = sprayAmmDOTRateIP;
        UIElements.sprayAmmPenIP = sprayAmmPenIP;
        UIElements.sprayDamTypeDD = sprayDamTypeDD;

        UIElements.modifyBaseBtn = modifyBaseBtn;
        UIElements.modifyMountBtn = modifyMountBtn;
        UIElements.quitConfirm = quitConfirm;
        UIElements.backButtonTower = backButtonTower;
        UIElements.backButtonWeapon = backButtonWeapon;
        UIElements.backButtonAmmo = backButtonAmmo;
        UIElements.returnToAttachedWeaponsButton = returnToAttachedWeaponsButton;
        UIElements.returnToAttachAmmoButton = returnToAttachAmmoButton;
        UIElements.confirmAmmoBtn = confirmAmmoBtn;

        UIElements.inventories = inventories;
        UIElements.towerInventory = towerInventory;
        UIElements.weaponInventory = weaponInventory;
        UIElements.munitionsInventory = munitionsInventory;

        UIElements.builders = builders;
        UIElements.towerBuilder = towerBuilder;
        UIElements.weaponsBuilder = weaponsBuilder;
        UIElements.munitonsBuilder = munitonsBuilder;

        UIElements.towerBaseCanvas = towerBaseCanvas;
        UIElements.mountStyleCanvas = mountStyleCanvas;
        UIElements.attachWeaponCanvas = attachWeaponCanvas;
        UIElements.barrelCanvas = barrelCanvas;
        UIElements.sprayerCanvas = sprayerCanvas;
        UIElements.advancedTargetingSystemCanvas = advancedTargetingSystemCanvas;
        UIElements.meleeCanvas = meleeCanvas;
        UIElements.projectileCanvas = projectileCanvas;
        UIElements.sprayCanvas = sprayCanvas;
        UIElements.attachProjectileAmmoCanvas = attachProjectileAmmoCanvas;
        UIElements.attachSprayAmmoCanvas = attachSprayAmmoCanvas;

        UIElements.weaponProjectileViewPort = weaponProjectileViewPort;
        UIElements.weaponSprayViewPort = weaponSprayViewPort;
        UIElements.meleeViewPort = meleeViewPort;
        UIElements.weaponAdvTarSysViewPort = weaponAdvTarSysViewPort;
        UIElements.ammoProjctileViewPort = ammoProjctileViewPort;
        UIElements.ammoSprayViewPort = ammoSprayViewPort;

        UIElements.creationConfirmation = creationConfirmation;
        UIElements.towerConfirm = towerConfirm;
        UIElements.weaponConfirm = weaponConfirm;
        UIElements.ammoConfirm = ammoConfirm;
        UIElements.fileNameInputBorder = fileNameInputBorder;
        UIElements.weaponSizeChangeCanvas = weaponSizeChangeCanvas;
        UIElements.firingRangeCanvas = firingRangeCanvas;

        UIElements.ammoSlot = ammoSlot;
        UIElements.dontShowSizeWarning = dontShowSizeWarning;


        UIElements.targetWeaknessText =  TargetWeaknessText;
        UIElements.targetWeaknessBoarder = TargetWeaknessBoarder;
    }
}
