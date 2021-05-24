using UnityEngine;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Class that enables player to detach weapons in the tower editor.
    /// </summary>
    public class TC_UI_DetachWeapon : MonoBehaviour
    {
        public static bool DetachWeapons;
        [SerializeField] GameObject _detachModeText;
        [SerializeField] GameObject _attachModeText;

        public void SwitchMode()
        {
            DetachWeapons = !DetachWeapons;
            _detachModeText.SetActive(DetachWeapons);
            _attachModeText.SetActive(!DetachWeapons);
        }
    }
}
