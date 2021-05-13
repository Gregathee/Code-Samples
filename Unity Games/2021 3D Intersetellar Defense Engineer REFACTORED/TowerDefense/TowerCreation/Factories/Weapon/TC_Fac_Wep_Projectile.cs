using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories.Weapon
{
    /// <summary>
    /// Factory class that creates projectile weapons
    /// </summary>
    public class TC_Fac_Wep_Projectile : MonoBehaviour, TC_Fac_ITowerPartFactory<TP_Wep_Projectile>
    {
        [SerializeField] TC_Fac_Wep_AmmoSlot _ammoSlot;
        [SerializeField] TC_UI_TowerPartSelector _partSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _fireRateIP;
        [SerializeField] TMP_Dropdown _recoilDD;
        [SerializeField] TMP_Dropdown _accuracyDD;
        [SerializeField] Toggle _canShootGroundTog;
        [SerializeField] Toggle _canSHootAirTog;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] TMP_InputField _critChanceIP;
        [SerializeField] TMP_InputField _critDamageIP;

        public TP_Wep_Projectile CreateTowerPart()
        {
            SanitizeInput();
            TP_Wep_Projectile projectileWeapon = Instantiate(_partSelector.GetCurrentPart().GetComponent<TP_Wep_Projectile>());
            projectileWeapon.SetAmmoFilePath(_ammoSlot.GetAmmo().CustomPartFilePath);
            projectileWeapon.SetSize((PartSize)_sizeDD.value);
            projectileWeapon.FireRate = float.Parse(_fireRateIP.text);
            projectileWeapon.Recoil = (Recoil)_recoilDD.value;
            projectileWeapon.Accuracy = (Accuracy)_accuracyDD.value;
            projectileWeapon.CanShootDown = _canShootGroundTog.isOn;
            projectileWeapon.CanShootUp = _canSHootAirTog.isOn;
            projectileWeapon.RotationSpeed = int.Parse(_rotationSpeedIP.text);
            projectileWeapon.CritChance = int.Parse(_critChanceIP.text);
            projectileWeapon.CritDamage = int.Parse(_critDamageIP.text);
            return projectileWeapon;
        }

        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _fireRateIP, TP_Wep_Projectile.FIRE_RATE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_Wep_Projectile.ROTATION_SPEED_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _critChanceIP, TP_Wep_Projectile.CRIT_CHANCE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _critDamageIP, TP_Wep_Projectile.CRIT_DAMAGE_BOUNDS);
        }
    }
}
