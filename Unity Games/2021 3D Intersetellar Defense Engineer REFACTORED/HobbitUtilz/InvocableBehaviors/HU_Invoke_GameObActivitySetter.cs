using UnityEngine;

namespace HobbitUtilz.InvocableBehaviors
{
    /// <summary>
    /// Activates an array of objects and deactivates another
    /// </summary>
    public class HU_Invoke_GameObActivitySetter : HU_InvocableObject
    {
        [SerializeField] GameObject[] _objectsToActivate;
        [SerializeField] GameObject[] _objectsToDeactivate;

        public override void Invoke()
        {
            foreach (GameObject gameOb in _objectsToActivate) { gameOb.SetActive(true); }
            foreach (GameObject gameOb in _objectsToDeactivate) { gameOb.SetActive(false); }
        }
    }
}
