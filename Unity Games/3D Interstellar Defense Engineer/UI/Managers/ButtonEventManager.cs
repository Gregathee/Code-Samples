using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventManager : MonoBehaviour
{
    public static ButtonEventManager buttonEventManager = null;
    public delegate void ConfirmationDelegate();
    public static readonly float coolDown = 0.75f;
    public static bool cooledDown = true;
    public static bool returnToWeapon = false;
    public static bool returnToTower = false;
    public static readonly int maxInputLength = 30;

    public static PartSize projectileWeaponSize = PartSize.Medium;
    public static PartSize sprayWeaponSize = PartSize.Medium;
    public static PartSize meleeWeaponSize = PartSize.Medium;
    public static PartSize targetingWeaponSize = PartSize.Medium;
    public static bool forceEdit = false;

    

    private void Start()  {  SetMainStageTower();  buttonEventManager = this; }

    public static void ChangeBaseSize()
    {
        PartSize size = PartSize.Small;
        switch (UIElements.baseSizeDD.value)
        {
            case 1: size = (PartSize.Medium); break;
            case 2: size = (PartSize.Large); break;
        }
        UIElements.baseSelector.SetSize(size);
        UIElements.mountSelector.SetSize(size);
        UIElements.styleSelector.SetSize(size);
        TowerAssembler.towerAssembler.ChangeBaseSize(size);
    }

    public static void ChangeProjectileWeaponSize()
    {
        if(UIElements.projWepSizeDD.value > (int)projectileWeaponSize && !UIElements.dontShowSizeWarning.isOn && InventoryManager.inventoryManager.updateAfterSubPartEdit)
            UIElements.weaponSizeChangeCanvas.SetActive(true);
        projectileWeaponSize = (PartSize)UIElements.projWepSizeDD.value;
        UIElements.projectileWeaponSelector.SetSize(projectileWeaponSize);
    }

    public static void ChangeSprayWeaponSize()
    {
        if (UIElements.sprayWepSizeDD.value > (int)sprayWeaponSize && !UIElements.dontShowSizeWarning.isOn && InventoryManager.inventoryManager.updateAfterSubPartEdit) 
            UIElements.weaponSizeChangeCanvas.SetActive(true);
        sprayWeaponSize = (PartSize)UIElements.sprayWepSizeDD.value;
        UIElements.sprayerWeaponSelector.SetSize(sprayWeaponSize);
    }

    public static void ChangeMeleeWeaponSize()
    {
        if (UIElements.meleeWepSizeDD.value > (int)meleeWeaponSize && !UIElements.dontShowSizeWarning.isOn && InventoryManager.inventoryManager.updateAfterSubPartEdit)
            UIElements.weaponSizeChangeCanvas.SetActive(true);
        meleeWeaponSize = (PartSize)UIElements.meleeWepSizeDD.value;
        UIElements.meleeSelector.SetSize(meleeWeaponSize);
    }

    public static void ChangeTargetingWeaponSize()
    {
        if (UIElements.targetSizeDD.value > (int)targetingWeaponSize && !UIElements.dontShowSizeWarning.isOn && InventoryManager.inventoryManager.updateAfterSubPartEdit)
            UIElements.weaponSizeChangeCanvas.SetActive(true);
        targetingWeaponSize = (PartSize)UIElements.targetSizeDD.value;
        UIElements.targetingSelector.SetSize(targetingWeaponSize);
    }

    public static void SetMainStageTower() { BMStageSetter.SetMainStageTower(); }
    public static void SetMainStageWeapon() { BMStageSetter.SetMainStageWeapon(); }
    public static void SetMainStageTowerAmmo() { BMStageSetter.SetMainStageTowerAmmo(); }

    public static void SetCreateTowerStage() { BMStageSetter.SetCreateTowerStage(); }
    public static void SetCreateWeaponStage() { BMStageSetter.SetCreateWeaponStage(); }
    public static void SetCreateSprayerWeaponStage() { BMStageSetter.SetCreateSprayerWeaponStage(); }
    public static void SetCreateMeleeWeaponStage() { BMStageSetter.SetCreateMeleeWeaponStage(); }
    public static void SetCreateTargetingWeaponStage() { BMStageSetter.SetCreateTargetingWeaponStage(); }
    public static void SetCreateProjectileAmmoStage() { BMStageSetter.SetCreateProjectileAmmoStage(); }
    public static void SetCreateSprayAmmoStage() { BMStageSetter.SetCreateSprayAmmoStage(); }

    public static void SetQuitButtonFromWeaponToProjectileAmmo(){SetReturnBackButtons(true);}
    public static void SetQuitButtonFromWeaponToSprayAmmo(){SetReturnBackButtons(false);}

    static void SetReturnBackButtons(bool projectile)
    {
        if (UIElements.returnToAttachedWeaponsButton.activeInHierarchy) { BMStageSetter.SetBackButtons(true, false, false); }
        else { BMStageSetter.SetBackButtons(false, true, false); }

        if(projectile) SetCreateProjectileAmmoStage();
        else SetCreateSprayAmmoStage();

        UIElements.returnToAttachAmmoButton.SetActive(true);
    }

    public static void SetReturnToTower() { returnToTower = true; }
    public static void SetReturnToWeapon() { returnToWeapon = true; }
    public static void ClearReturnToTower() { returnToTower = false;  }
    public static void ClearReturnToWeapon() { returnToWeapon = false; }

    public static bool CheckValidInput()
	{
        if (UIElements.fileNameInput.text.Length > maxInputLength) UIElements.PromptErrorMessage("This name cannot be longer than 30 characters.");
        else
        {
            if (EnumsAndStaticFunctions.ValidInput(UIElements.fileNameInput.text)) return true;
            else UIElements.PromptErrorMessage("This name includes invalid characters.");
        }
        UIElements.creationConfirmation.SetActive(false);
        return false;
    }

    public static void CheckTowerPrereqs(bool test)
    {
        UIElements.rejectDeleteCanvas.SetActive(false);
        if (TowerAssembler.towerAssembler.AllSlotsHaveWeapons())
        {
            if (TowerAssembler.towerAssembler.WeaponsTouching()) { UIElements.rejectDeleteCanvas.SetActive(true); UIElements.rejectDeleteText.text = "Weapons cannot be intersecting."; }
            else
            {
                if (!test)
                {
                    UIElements.creationConfirmation.SetActive(true);
                    UIElements.towerConfirm.SetActive(true);
                    UIElements.fileNameInput.gameObject.SetActive(true);
                    UIElements.fileNameInputBorder.SetActive(true);
                    UIElements.weaponConfirm.SetActive(false);
                    UIElements.ammoConfirm.SetActive(false);
                }
				else
				{
                    TowerAssembler.towerAssembler.SetMountProperties();
                    TowerAssembler.towerAssembler.ConfirmTower(true, "", true);
                    UIElements.builders.SetActive(false);
                    CameraManager.cameraManager.ChangeState(5);
                    UIElements.firingRangeCanvas.SetActive(true);
                    FiringRange.firingRange.TestTower();
                }
            }
        }
        else { UIElements.rejectDeleteCanvas.SetActive(true); UIElements.rejectDeleteText.text = "This tower needs a weapon for each weapon slot."; }
    }

    public static void CheckWeaponPrereqs()
    {
        if (TowerAssembler.towerAssembler.AmmoAssigned() || UIElements.meleeCanvas.activeInHierarchy || UIElements.advancedTargetingSystemCanvas.activeInHierarchy)
        {
            UIElements.creationConfirmation.SetActive(true);
            UIElements.towerConfirm.SetActive(false);
            UIElements.fileNameInput.gameObject.SetActive(true);
            UIElements.fileNameInputBorder.SetActive(true);
            UIElements.weaponConfirm.SetActive(true);
            UIElements.ammoConfirm.SetActive(false);
        }
        else { UIElements.PromptErrorMessage("This weapon needs ammunition."); }
    }

    public static void ConfirmTower()
    {
        if (CheckValidInput())
        {
            if (UIElements.fileNameInput.text.Length > 0)
            {
                TowerAssembler.towerAssembler.SetMountProperties();
                TowerAssembler.towerAssembler.ConfirmTower(true, "", false);
                //Stops Pipline if name is already in use
                if (InventoryManager.inventoryManager.clearToAdd )SetMainStageTower();

                UIElements.fileNameInput.gameObject.SetActive(false);
                UIElements.fileNameInputBorder.SetActive(false);
                UIElements.creationConfirmation.SetActive(false);
            }
            else { buttonEventManager.StartFlashInputField(); }
        }
    }

    public static void ConfirmWeapon()
    {
        if (CheckValidInput())
        {
            if (UIElements.fileNameInput.text.Length > 0)
            {
                TowerAssembler.towerAssembler.SetWeaponProperties();
                TowerAssembler.towerAssembler.ConfirmWeapon();
                //Stops Pipline if name is already in use
                if (InventoryManager.inventoryManager.clearToAdd)
                {
                    if (returnToTower) { returnToTower = false; ReturnToAttachWeapon(); }
                    else { SetMainStageWeapon(); }
                }
                UIElements.creationConfirmation.SetActive(false);
                UIElements.fileNameInput.gameObject.SetActive(false);
                UIElements.fileNameInputBorder.SetActive(false);
            }
        }
    }

    public static void ConfirmAmmo()
    {
        if (CheckValidInput())
        {
            if (UIElements.fileNameInput.text.Length > 0)
            {
                TowerAssembler.towerAssembler.SetAmmoProperties();
                TowerAssembler.towerAssembler.ConfirmAmmo(); 
                //Stops pipeline if name is already in use
                if (InventoryManager.inventoryManager.clearToAdd)
                {
                    if (returnToWeapon) { returnToWeapon = false; ReturnToAttachAmmo(); }
                    else { SetMainStageTowerAmmo(); }
                }
                UIElements.creationConfirmation.SetActive(false);
                UIElements.fileNameInput.gameObject.SetActive(false);
                UIElements.fileNameInputBorder.SetActive(false);
             }
            else { buttonEventManager.StartFlashInputField(); }
        }
    }



    public static void ReturnToAttachWeapon()
    {
        CameraManager.cameraManager.ChangeState(4);
        UIElements.returnToAttachedWeaponsButton.SetActive(false);
        UIElements.weaponsBuilder.SetActive(false);
        UIElements.towerBuilder.SetActive(true);
        ClearReturnToTower();
    }

    public static void ReturnToAttachAmmo()
    {
        CameraManager.cameraManager.ChangeState(2);
        UIElements.returnToAttachAmmoButton.SetActive(false);
        UIElements.munitonsBuilder.SetActive(false);
        UIElements.weaponsBuilder.SetActive(true);
        ClearReturnToWeapon();
    }



    public static void SubmitParts()
    {
        UIElements.styleSelector.SubmitPart();
        UIElements.baseSelector.SubmitPart();
        UIElements.mountSelector.SubmitPart();
    }

    public static void CreateTowerBaseToStyleTransition()
    {
        if(cooledDown)
        {
            buttonEventManager.StartCoolDown();
            UIElements.modifyBaseBtn.SetActive(true);
            UIElements.modifyMountBtn.SetActive(false);
            UIElements.towerBaseCanvas.SetActive(false);
            UIElements.mountStyleCanvas.SetActive(true);
            UIElements.attachWeaponCanvas.SetActive(false);
            CameraManager.cameraManager.ChangeState(1);
            UIElements.styleSelector.UnHide(true);
            UIElements.baseSelector.Hide(false);
            UIElements.mountSelector.Hide(false);
        }
    }

    public static void CreateTowerStyleToBaseTransition()
    {
        if(cooledDown)
        {
            buttonEventManager.StartCoolDown();
            UIElements.modifyMountBtn.SetActive(true);
            UIElements.modifyBaseBtn.SetActive(false);
            UIElements.mountStyleCanvas.SetActive(false);
            UIElements.attachWeaponCanvas.SetActive(false);
            UIElements.towerBaseCanvas.SetActive(true);
            CameraManager.cameraManager.ChangeState(0);
            TowerAssembler.towerAssembler.ShowMountStyle(true);
            UIElements.styleSelector.Hide(true);
            UIElements.baseSelector.UnHide(false);
            UIElements.mountSelector.UnHide(true);
        }
    }


    //Transitions between create weapon types
    public static void ProjectileToSpray()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, true, false, false);
            BMStageSetter.SetWeaponSelectors(false, true, false, false);
            UIElements.sprayerWeaponSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void ProjectileToTargeting()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, false, false, true);
            BMStageSetter.SetWeaponSelectors(false, false, false, true);
            UIElements.targetingSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void SprayToMelee()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, false, true, false);
            BMStageSetter.SetWeaponSelectors(false, false, true, false);
            UIElements.meleeSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void SprayToProjectile()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(true, false, false, false);
            BMStageSetter.SetWeaponSelectors(true, false, false, false);
            UIElements.projectileWeaponSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void MeleeToTargeting()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, false, false, true);
            BMStageSetter.SetWeaponSelectors(false, false, false, true);
            UIElements.targetingSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void MeleeToSpray()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, true, false, false);
            BMStageSetter.SetWeaponSelectors(false, true, false, false);
            UIElements.sprayerWeaponSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void TargetingToProjectile()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(true, false, false, false);
            BMStageSetter.SetWeaponSelectors(true, false, false, false);
            UIElements.projectileWeaponSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public static void TargetingToMelee()
    {
        if (cooledDown)
        {
            BMStageSetter.SetWeaponCanvases(false, false, true, false);
            BMStageSetter.SetWeaponSelectors(false, false, true, false);
            UIElements.meleeSelector.SubmitPart();
            buttonEventManager.StartCoolDown();
        }
    }

    public void StartCoolDown() { StartCoroutine(CoolDown()); }

    public void StartFlashInputField() { StartCoroutine(FlashInputField()); }

    public IEnumerator CoolDown()
    {
        cooledDown = false;
        yield return new WaitForSeconds(coolDown);
        cooledDown = true;
    }

    public IEnumerator FlashInputField()
    {
        for (int i = 0; i < 2; i++)
        {
            Color color = UIElements.fileNameInput.GetComponent<Image>().color;
            color.a = 0;
            UIElements.fileNameInput.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.1f);
            color.a = 1;
            UIElements.fileNameInput.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
