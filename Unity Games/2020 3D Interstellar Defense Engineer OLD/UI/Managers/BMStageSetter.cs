using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BMStageSetter : MonoBehaviour
{
    public static void SetMainStageTower()
    {
        SetMainStage(true, false, false, 0);
        TowerAssembler.towerAssembler.ClearAssembler();
        UIElements.styleSelector.Hide(false);
        UIElements.mountSelector.Hide(false);
        UIElements.baseSelector.Hide(false);
        ButtonEventManager.returnToTower = false;
    }

    public static void SetMainStageWeapon()
    {
		if (ButtonEventManager.returnToTower) { SetMainStage(true, false, false, 0); }
        else SetMainStage(false, true, false, 2);
        SetWeaponViewPorts(true, false, false, false);
        UIElements.munitionsInventory.SetActive(false);
        SetWeaponSelectors(false, false, false, false);
        ButtonEventManager.returnToWeapon = false;
    }

    public static void SetMainStageTowerAmmo()
    {
        if (ButtonEventManager.returnToTower) { SetMainStage(true, false, false, 0); }
        else if(ButtonEventManager.returnToWeapon) SetMainStage(false, true, false, 2);
        else SetMainStage(false, false, true, 3);
        UIElements.ammoProjctileViewPort.SetActive(true);
        UIElements.ammoSprayViewPort.SetActive(false);
        UIElements.spraySelector.Hide(false);
        UIElements.projectileSelector.Hide(false);
    }

    static void SetMainStage(bool tower, bool weapon, bool ammo, int state)
    {
        UIElements.quitConfirm.SetActive(false);
        UIElements.inventories.SetActive(true);
        UIElements.builders.SetActive(false);
        UIElements.towerInventory.SetActive(tower);
        UIElements.weaponInventory.SetActive(weapon);
        UIElements.munitionsInventory.SetActive(ammo);
        CameraManager.cameraManager.ChangeState(state);
        ButtonEventManager.returnToTower = false;
        ButtonEventManager.returnToWeapon = false;
    }

    public static void SetCreateTowerStage()
    {
        SetCreateStage(true, false, false, 0);
        ButtonEventManager.SubmitParts();

        UIElements.towerBaseCanvas.SetActive(true);
        UIElements.mountStyleCanvas.SetActive(false);
        UIElements.modifyBaseBtn.SetActive(false);
        UIElements.modifyMountBtn.SetActive(true);
        UIElements.attachWeaponCanvas.SetActive(false);

        UIElements.baseSelector.UnHide(false);
        UIElements.mountSelector.UnHide(false);

        TowerAssembler.towerAssembler.ShowMountStyle(true);
        UIElements.baseSizeDD.value = 1;
        TowerAssembler.towerAssembler.ChangeBaseSize(PartSize.Medium);
        SetBackButtons(true, false, false);
    }

    public static void SetCreateWeaponStage()
    {
        ButtonEventManager.buttonEventManager.StartCoolDown();
        SetCreateStage(false, true, false, 2);
        CameraManager.cameraManager.ChangeState(2);
        SetWeaponSelectors(true, false, false, false);
        UIElements.ammoSlot.SetActive(false);
        SetWeaponCanvases(true, false, false, false);
        SetWeaponViewPorts(true, false, false, false);
        UIElements.projectileWeaponSelector.SubmitPart();
        SetBackButtons(false, true, false);
    }

    public static void SetCreateSprayerWeaponStage()
    {
        ButtonEventManager.buttonEventManager.StartCoolDown();
        SetCreateStage(false, true, false, 2);
        CameraManager.cameraManager.ChangeState(2);
        SetWeaponSelectors(false, true, false, false);
        UIElements.ammoSlot.SetActive(false);
        SetWeaponCanvases(false, true, false, false);
        SetWeaponViewPorts(false, true, false, false);
        UIElements.sprayerWeaponSelector.SubmitPart();
        SetBackButtons(false, true, false);
    }

    public static void SetCreateMeleeWeaponStage()
    {
        ButtonEventManager.buttonEventManager.StartCoolDown();
        SetCreateStage(false, true, false, 2);
        CameraManager.cameraManager.ChangeState(2);
        SetWeaponSelectors(false, false, true, false);
        UIElements.ammoSlot.SetActive(false);
        SetWeaponCanvases(false, false, true, false);
        SetWeaponViewPorts(false, false, true, false);
        UIElements.meleeSelector.SubmitPart();
        SetBackButtons(false, true, false);
    }

    public static void SetCreateTargetingWeaponStage()
    {
        ButtonEventManager.buttonEventManager.StartCoolDown();
        SetCreateStage(false, true, false, 2);
        CameraManager.cameraManager.ChangeState(2);
        SetWeaponSelectors(false, false, false, true);
        UIElements.ammoSlot.SetActive(false);
        SetWeaponCanvases(false, false, false, true);
        SetWeaponViewPorts(false, false, false, true);
        UIElements.targetingSelector.SubmitPart();
        SetBackButtons(false, true, false);
    }

    public static void SetWeaponCanvases(bool projectile, bool spray, bool melee, bool targeting)
	{
        UIElements.barrelCanvas.SetActive(projectile);
        UIElements.sprayerCanvas.SetActive(spray);
        UIElements.advancedTargetingSystemCanvas.SetActive(melee);
        UIElements.meleeCanvas.SetActive(targeting);

        UIElements.attachProjectileAmmoCanvas.SetActive(false);
        UIElements.returnToAttachedWeaponsButton.SetActive(false);
        UIElements.attachSprayAmmoCanvas.SetActive(false);
    }

    static void SetWeaponViewPorts(bool projectile, bool spray, bool melee, bool targeting)
	{
        UIElements.weaponProjectileViewPort.SetActive(projectile);
        UIElements.weaponSprayViewPort.SetActive(spray);
        UIElements.meleeViewPort.SetActive(melee);
        UIElements.weaponAdvTarSysViewPort.SetActive(targeting);
    }

    public static void SetWeaponSelectors(bool projectile, bool sprayer, bool melee, bool targetingSystem)
	{
        if (projectile) UIElements.projectileWeaponSelector.UnHide(false);
        else UIElements.projectileWeaponSelector.Hide(false);
        if (sprayer) UIElements.sprayerWeaponSelector.UnHide(false);
        else UIElements.projectileWeaponSelector.Hide(false);
        if (melee) UIElements.meleeSelector.UnHide(false);
        else UIElements.projectileWeaponSelector.Hide(false);
        if (targetingSystem) UIElements.targetingSelector.UnHide(false);
        else UIElements.projectileWeaponSelector.Hide(false);
    }

    public static void SetCreateProjectileAmmoStage()
    {
        SetCreateStage(false, false, true, 3);
        UIElements.projectileSelector.SubmitPart();
        UIElements.projectileSelector.UnHide(false);
        UIElements.spraySelector.Hide(false);
        UIElements.returnToAttachAmmoButton.SetActive(false);
        UIElements.projectileCanvas.SetActive(true);
        UIElements.sprayCanvas.SetActive(false);
        UIElements.ammoProjctileViewPort.SetActive(true);
        UIElements.ammoSprayViewPort.SetActive(false);
        SetBackButtons(false, false, true);
    }

    public static void SetCreateSprayAmmoStage()
    {
        SetCreateStage(false, false, true, 3);
        UIElements.spraySelector.SubmitPart();
        UIElements.projectileSelector.Hide(false);
        UIElements.spraySelector.UnHide(false);
        UIElements.returnToAttachAmmoButton.SetActive(false);
        UIElements.projectileCanvas.SetActive(false);
        UIElements.sprayCanvas.SetActive(true);
        UIElements.ammoProjctileViewPort.SetActive(true);
        UIElements.ammoSprayViewPort.SetActive(false);
        UIElements.confirmAmmoBtn.SetActive(true);
        SetBackButtons(false, false, true);
    }

    public static void SetBackButtons(bool tower, bool weapon, bool ammo)
    {
        UIElements.backButtonTower.SetActive(tower);
        UIElements.backButtonWeapon.SetActive(weapon);
        UIElements.backButtonAmmo.SetActive(ammo);
    }

    static void SetCreateStage(bool tower, bool weapon, bool ammo, int state)
    {
        TowerAssembler.towerAssembler.ClearAmmo();
        StatWindow.statWindow.ClearStats();
        CameraManager.cameraManager.ChangeState(state);
        UIElements.builders.SetActive(true);
        UIElements.inventories.SetActive(false);
        UIElements.towerBuilder.SetActive(tower);
        UIElements.weaponsBuilder.SetActive(weapon);
        UIElements.munitonsBuilder.SetActive(ammo);
    }
}
