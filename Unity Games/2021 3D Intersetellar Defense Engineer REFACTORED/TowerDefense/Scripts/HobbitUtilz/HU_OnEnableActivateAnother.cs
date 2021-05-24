using UnityEngine;

namespace HobbitUtilz
{
    /// <summary>
    /// When this object enables, it enables another object;
    /// </summary>
    public class HU_OnEnableActivateAnother : MonoBehaviour
    {
        [SerializeField] GameObject[] _others;

        void OnEnable() { foreach (GameObject other in _others) { other.SetActive(true); } }
    }
}
