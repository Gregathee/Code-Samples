using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElements : MonoBehaviour
{
    public static Toggle dontShowSizeWarning = null;
    public static GameObject ammoSlot = null;
    public static GameObject pathBuilderCanvas;
    public static GameObject statText;
    public static TMP_Text partNameText = null;
    public static TMP_InputField fileNameInput = null;
    public static GameObject rejectDeleteCanvas = null;
    public static TMP_Text rejectDeleteText = null;
    public static GameObject deleteConfirmCanvas = null;
    [Header("Selectors")]
    public static TowerPartSelector baseSelector = null;
    public static TowerPartSelector mountSelector = null;
    public static TowerPartSelector styleSelector = null;
    public static TowerPartSelector projectileSelector = null;
    public static TowerPartSelector spraySelector = null;
    public static TowerPartSelector projectileWeaponSelector = null;
    public static TowerPartSelector sprayerWeaponSelector = null;
    public static TowerPartSelector targetingSelector = null;
    public static TowerPartSelector meleeSelector = null;
    [Header("Stat Inputs")]
    public static TMP_Dropdown baseSizeDD = null;
    public static TMP_InputField baseRotSpeedIP = null;
    public static TMP_InputField baseRangeIP = null;
    public static  TMP_InputField ConstructionTimeIP = null;
    [Space(10)]
    public static TMP_Dropdown baseTurnRadDD = null;
    public static TMP_Dropdown projWepSizeDD = null;
    public static TMP_InputField projWepRateIP = null;
    public static TMP_InputField projWepRotSpeedIP = null;
    public static GameObject projWepRotSpeedText = null;
    public static TMP_Dropdown projWepRecoilDD = null;
    public static TMP_Dropdown projWepAccuracyDD = null;
    public static Toggle projWepTargetAirTog = null;
    public static Toggle projWepTargetGroundTog = null;
    [Space(10)]
    public static TMP_Dropdown sprayWepSizeDD = null;
    public static TMP_InputField sprayWepRateIP = null;
    public static Toggle sprayWepTargetAirTog = null;
    public static Toggle sprayWepTargetGroundTog = null;
    [Space(10)]
    public static TMP_Dropdown meleeWepSizeDD = null;
    public static TMP_InputField meleeWepDamageIP = null;
    public static TMP_Dropdown meleeDamageTypeDD = null;
    [Space(10)]
    public static TMP_Dropdown targetSizeDD = null;
    public static TMP_Dropdown targetWeaknessLevelDD = null;
    public static Toggle targetBasicTog = null;
    public static Toggle targetAdvancedTog = null;
    public static Toggle targetDetectStealthTog = null;
    [Space(10)]
    public static TMP_InputField projAmmoDamageIP = null;
    public static TMP_InputField projAmmDOTIP = null;
    public static TMP_InputField projAmmDOTDurIP = null;
    public static TMP_InputField projAmmDOTRateIP = null;
    public static TMP_InputField projAmmPenIP = null;
    public static TMP_InputField projAmmSpeedIP = null;
    public static TMP_InputField projAmmLifeIP = null;
    public static TMP_InputField projRadIP = null;
    public static TMP_InputField projHomIP = null;
    public static TMP_Dropdown projDamTypeDD = null;
    [Space(10)]
    public static TMP_InputField sprayAmmoDamageIP = null;
    public static TMP_InputField sprayAmmDOTIP = null;
    public static TMP_InputField sprayAmmDOTDurIP = null;
    public static TMP_InputField sprayAmmDOTRateIP = null;
    public static TMP_InputField sprayAmmPenIP = null;
    public static TMP_Dropdown sprayDamTypeDD = null;
    [Header("Buttons")]
    public static GameObject modifyBaseBtn = null;
    public static GameObject modifyMountBtn = null;
    public static GameObject quitConfirm = null;
    public static GameObject backButtonTower = null;
    public static GameObject backButtonWeapon = null;
    public static GameObject backButtonAmmo = null;
    public static GameObject returnToAttachedWeaponsButton = null;
    public static GameObject returnToAttachAmmoButton = null;
    public static GameObject confirmAmmoBtn = null;
    [Header("Canvases")]
    public static GameObject inventories = null;
    public static GameObject towerInventory;
    public static GameObject weaponInventory = null;
    public static GameObject munitionsInventory = null;
    [Space(10)]
    public static GameObject builders = null;
    public static GameObject towerBuilder = null;
    public static GameObject weaponsBuilder = null;
    public static GameObject munitonsBuilder = null;
    [Space(10)]
    public static GameObject towerBaseCanvas = null;
    public static GameObject mountStyleCanvas = null;
    public static GameObject attachWeaponCanvas = null;
    [Space(10)]
    public static GameObject barrelCanvas = null;
    public static GameObject sprayerCanvas = null;
    public static GameObject advancedTargetingSystemCanvas = null;
    public static GameObject meleeCanvas = null;
    public static GameObject projectileCanvas = null;
    public static GameObject sprayCanvas = null;
    public static GameObject attachProjectileAmmoCanvas = null;
    public static GameObject attachSprayAmmoCanvas = null;
    [Space(10)]
    public static GameObject weaponProjectileViewPort = null;
    public static GameObject weaponSprayViewPort = null;
    public static GameObject meleeViewPort = null;
    public static GameObject weaponAdvTarSysViewPort = null;
    public static GameObject ammoProjctileViewPort = null;
    public static GameObject ammoSprayViewPort = null;
    [Space(10)]
    public static GameObject creationConfirmation = null;
    public static GameObject towerConfirm = null;
    public static GameObject weaponConfirm = null;
    public static GameObject ammoConfirm = null;
    public static GameObject fileNameInputBorder = null;
    public static GameObject weaponSizeChangeCanvas = null;
    public static GameObject firingRangeCanvas = null;

    public static GameObject targetWeaknessText;
    public static GameObject targetWeaknessBoarder;

	private void Update()
	{
        SanatizeTowerInput();
        SanatizeProjectileWeaponInput();
        SantatizeSprayerWeaponInput();
        SanatizeMeleeWeaponInput();
        SanatizeProjectileAmmoInput();
        SanatizeSprayAmmoInput();
        HiddenMenuOptions();
    }

    void SanatizeTowerInput()
	{
        if (baseRotSpeedIP.text == "" && baseRotSpeedIP.gameObject != EventSystem.current.currentSelectedGameObject || baseRotSpeedIP.text == "-") baseRotSpeedIP.text = "0";
        if (baseRotSpeedIP.text != "") if (int.Parse(baseRotSpeedIP.text) < 0) baseRotSpeedIP.text = "0";
        if (baseRotSpeedIP.text != "") if (int.Parse(baseRotSpeedIP.text) > 99) baseRotSpeedIP.text = "99";
        if (baseRangeIP.text == "" && baseRangeIP.gameObject != EventSystem.current.currentSelectedGameObject || baseRangeIP.text == "-") baseRangeIP.text = "1";
        if (baseRangeIP.text != "" ) if (int.Parse(baseRangeIP.text) < 1) baseRangeIP.text = "1";
        if (baseRangeIP.text != "") if (int.Parse(baseRangeIP.text) > 99) baseRangeIP.text = "99";
        if (ConstructionTimeIP.text == "" && ConstructionTimeIP.gameObject != EventSystem.current.currentSelectedGameObject || ConstructionTimeIP.text == "-") ConstructionTimeIP.text = "1";
        if (ConstructionTimeIP.text != "") if (int.Parse(ConstructionTimeIP.text) < 1) ConstructionTimeIP.text = "1";
        if (ConstructionTimeIP.text != "") if (int.Parse(ConstructionTimeIP.text) > 300) ConstructionTimeIP.text = "300";
    }

    void SanatizeProjectileWeaponInput()
	{
        if (projWepRateIP.text == "" && projWepRateIP.gameObject != EventSystem.current.currentSelectedGameObject || projWepRateIP.text == "-") projWepRateIP.text = "1";
        if (projWepRateIP.text != "") if (int.Parse(projWepRateIP.text) < 1) projWepRateIP.text = "1";
        if (projWepRateIP.text != "") if (int.Parse(projWepRateIP.text) > 99) projWepRateIP.text = "99";
        projWepRotSpeedIP.gameObject.SetActive(projWepTargetAirTog.isOn);
        projWepRotSpeedText.gameObject.SetActive(projWepTargetAirTog.isOn);
        if (projWepTargetAirTog.isOn)
        {
            if (projWepRotSpeedIP.text == "" && projWepRotSpeedIP.gameObject != EventSystem.current.currentSelectedGameObject || projWepRotSpeedIP.text == "-") projWepRotSpeedIP.text = "1";
            if (projWepRotSpeedIP.text != "") if (int.Parse(projWepRotSpeedIP.text) < 1) projWepRotSpeedIP.text = "1";
            if (projWepRotSpeedIP.text != "") if (int.Parse(projWepRotSpeedIP.text) > 99) projWepRotSpeedIP.text = "99";
        }
        else projWepRotSpeedIP.text = "0";
    }

    void SantatizeSprayerWeaponInput()
	{
        if (sprayWepRateIP.text == "" && sprayWepRateIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayWepRateIP.text == "-") sprayWepRateIP.text = "1";
        if (sprayWepRateIP.text != "") if (int.Parse(sprayWepRateIP.text) < 1) sprayWepRateIP.text = "1";
        if (sprayWepRateIP.text != "") if (int.Parse(sprayWepRateIP.text) > 99) sprayWepRateIP.text = "99";
    }

    void SanatizeMeleeWeaponInput()
	{
        if (meleeWepDamageIP.text == "" && meleeWepDamageIP.gameObject != EventSystem.current.currentSelectedGameObject || meleeWepDamageIP.text == "-") meleeWepDamageIP.text = "1";
        if (meleeWepDamageIP.text != "") if (int.Parse(meleeWepDamageIP.text) < 1) meleeWepDamageIP.text = "1";
        if (meleeWepDamageIP.text != "") if (int.Parse(meleeWepDamageIP.text) > 999) meleeWepDamageIP.text = "999";
    }

    void SanatizeProjectileAmmoInput()
    {
        if (projAmmoDamageIP.text == "" && projAmmoDamageIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmoDamageIP.text == "-") projAmmoDamageIP.text = "1";
        if (projAmmoDamageIP.text != "") if (int.Parse(projAmmoDamageIP.text) < 1) projAmmoDamageIP.text = "1";
        if (projAmmoDamageIP.text != "") if (int.Parse(projAmmoDamageIP.text) > 999) projAmmoDamageIP.text = "999";
        if (projAmmSpeedIP.text == "" && projAmmSpeedIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmSpeedIP.text == "-") projAmmSpeedIP.text = "1";
        if (projAmmSpeedIP.text != "") if (int.Parse(projAmmSpeedIP.text) < 1) projAmmSpeedIP.text = "1";
        if (projAmmSpeedIP.text != "") if (int.Parse(projAmmSpeedIP.text) > 999) projAmmSpeedIP.text = "999";
        if (projAmmPenIP.text == "" && projAmmPenIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmPenIP.text == "-") projAmmPenIP.text = "1";
        if (projAmmPenIP.text != "") if (int.Parse(projAmmPenIP.text) < 1) projAmmPenIP.text = "1";
        if (projAmmPenIP.text != "") if (int.Parse(projAmmPenIP.text) > 999) projAmmPenIP.text = "999";
        if (projAmmLifeIP.text == "" && projAmmLifeIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmLifeIP.text == "-") projAmmLifeIP.text = "1";
        if (projAmmLifeIP.text != "") if (int.Parse(projAmmLifeIP.text) < 1) projAmmLifeIP.text = "1";
        if (projAmmLifeIP.text != "") if (int.Parse(projAmmLifeIP.text) > 10) projAmmLifeIP.text = "10";

        if (projAmmDOTIP.text == "" && projAmmDOTIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmDOTIP.text == "-") projAmmDOTIP.text = "0";
        if (projAmmDOTIP.text != "") if (int.Parse(projAmmDOTIP.text) < 0) projAmmDOTIP.text = "0";
        if (projAmmDOTIP.text != "") if (int.Parse(projAmmDOTIP.text) > 999) projAmmDOTIP.text = "999";
        if (projAmmDOTDurIP.text == "" && projAmmDOTDurIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmDOTDurIP.text == "-") projAmmDOTDurIP.text = "0";
        if (projAmmDOTDurIP.text != "") if (int.Parse(projAmmDOTDurIP.text) < 0) projAmmDOTDurIP.text = "0";
        if (projAmmDOTDurIP.text != "") if (int.Parse(projAmmDOTDurIP.text) > 99) projAmmDOTDurIP.text = "99";
        if (projAmmDOTRateIP.text == "" && projAmmDOTRateIP.gameObject != EventSystem.current.currentSelectedGameObject || projAmmDOTRateIP.text == "-") projAmmDOTRateIP.text = "0";
        if (projAmmDOTRateIP.text != "") if (int.Parse(projAmmDOTRateIP.text) < 0) projAmmDOTRateIP.text = "0";
        if (projAmmDOTRateIP.text != "" && projAmmDOTDurIP.text != "")
        { if (int.Parse(projAmmDOTRateIP.text) > int.Parse(projAmmDOTDurIP.text)) projAmmDOTRateIP.text = projAmmDOTDurIP.text; }
        if (projRadIP.text == "" && projRadIP.gameObject != EventSystem.current.currentSelectedGameObject || projRadIP.text == "-") projRadIP.text = "0";
        if (projRadIP.text != "") if (int.Parse(projRadIP.text) < 0) projRadIP.text = "0";
        if (projRadIP.text != "") if (int.Parse(projRadIP.text) > 99) projRadIP.text = "99";
        if (projHomIP.text == "" && projHomIP.gameObject != EventSystem.current.currentSelectedGameObject || projHomIP.text == "-") projHomIP.text = "0";
        if (projHomIP.text != "") if (int.Parse(projHomIP.text) < 0) projHomIP.text = "0";
        if (projHomIP.text != "") if (int.Parse(projHomIP.text) > 99) projHomIP.text = "99";
    }

    void SanatizeSprayAmmoInput()
    {
        if (sprayAmmoDamageIP.text == "" && sprayAmmPenIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayAmmoDamageIP.text == "-") sprayAmmoDamageIP.text = "1";
        if (sprayAmmoDamageIP.text != "") if (int.Parse(sprayAmmoDamageIP.text) < 1) sprayAmmoDamageIP.text = "1";
        if (sprayAmmoDamageIP.text != "") if (int.Parse(sprayAmmoDamageIP.text) > 999) sprayAmmoDamageIP.text = "999";
        if (sprayAmmPenIP.text == "" && sprayAmmPenIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayAmmPenIP.text == "-") sprayAmmPenIP.text = "1";
        if (sprayAmmPenIP.text != "") if (int.Parse(sprayAmmPenIP.text) < 1) sprayAmmPenIP.text = "1";
        if (sprayAmmPenIP.text != "") if (int.Parse(sprayAmmPenIP.text) > 999) sprayAmmPenIP.text = "999";

        if (sprayAmmDOTIP.text == "" && sprayAmmDOTIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayAmmDOTIP.text == "-") sprayAmmDOTIP.text = "0";
        if (sprayAmmDOTIP.text != "") if (int.Parse(sprayAmmDOTIP.text) < 0) sprayAmmDOTIP.text = "0";
        if (sprayAmmDOTIP.text != "") if (int.Parse(sprayAmmDOTIP.text) > 999) sprayAmmDOTIP.text = "999";
        if (sprayAmmDOTDurIP.text == "" && sprayAmmDOTDurIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayAmmDOTDurIP.text == "-") sprayAmmDOTDurIP.text = "0";
        if (sprayAmmDOTDurIP.text != "") if (int.Parse(sprayAmmDOTDurIP.text) < 0) sprayAmmDOTDurIP.text = "0";
        if (sprayAmmDOTDurIP.text != "") if (int.Parse(sprayAmmDOTDurIP.text) > 99) sprayAmmDOTDurIP.text = "99";
        if (sprayAmmDOTRateIP.text == "" && sprayAmmDOTRateIP.gameObject != EventSystem.current.currentSelectedGameObject || sprayAmmDOTRateIP.text == "-") sprayAmmDOTRateIP.text = "0";
        if (sprayAmmDOTRateIP.text != "") if (int.Parse(sprayAmmDOTRateIP.text) < 0) sprayAmmDOTRateIP.text = "0";
        if (sprayAmmDOTRateIP.text != "" && sprayAmmDOTDurIP.text != "")
        { if (int.Parse(sprayAmmDOTRateIP.text) > int.Parse(sprayAmmDOTDurIP.text)) sprayAmmDOTRateIP.text = sprayAmmDOTDurIP.text; }
    }

    void HiddenMenuOptions()
	{
        targetWeaknessLevelDD.gameObject.SetActive(targetAdvancedTog.isOn);
        targetWeaknessBoarder.SetActive(targetAdvancedTog.isOn);
        targetWeaknessText.SetActive(targetAdvancedTog.isOn);
        if (!projWepTargetGroundTog.isOn && !projWepTargetAirTog.isOn) projWepTargetGroundTog.isOn = true;
        if (!sprayWepTargetGroundTog.isOn && !sprayWepTargetAirTog.isOn) sprayWepTargetGroundTog.isOn = true;
        if (!targetDetectStealthTog.isOn && !targetBasicTog.isOn && !targetAdvancedTog.isOn) targetBasicTog.isOn = true;
    }

    public static void PromptErrorMessage(string message)
	{
        deleteConfirmCanvas.SetActive(false);
        rejectDeleteCanvas.SetActive(true);
        rejectDeleteText.text = message;
    }
}
