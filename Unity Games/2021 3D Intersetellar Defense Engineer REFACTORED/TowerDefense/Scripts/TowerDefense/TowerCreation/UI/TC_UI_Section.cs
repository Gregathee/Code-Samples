using Cinemachine;
using UnityEngine;
using HobbitUtilz.InvocableBehaviors;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Holds the title, virtual camera, and creation setup behavior of a tower creation inventory menu.
    /// </summary>
    public class TC_UI_Section : MonoBehaviour
    {
        [SerializeField] string _title = "";
        [SerializeField] CinemachineVirtualCamera _vCam ;
        [SerializeField] HU_InvocableObject[] _invocableObjects;

        public string GetTitle() { return _title; }
        public void ActivateVCam(bool activate) { if (_vCam) { _vCam.gameObject.SetActive(activate); } }

        public void Invoke() { foreach(HU_InvocableObject invocable in _invocableObjects){invocable.Invoke();} }
    }
}
