using UnityEngine;

namespace HobbitUtilz.InvocableBehaviors
{
    /// <summary>
    /// Selects a given object from a HU_OnlyOneActiveArray.
    /// </summary>
    public class HU_Invoke_OnlySelectOne : HU_InvocableObject
    {
        [SerializeField] HU_OnlyOneActiveArray _oneActiveArray;
        [SerializeField] GameObject _objectToSelect;
        
        public override void Invoke() { _oneActiveArray.SelectGameObject(_objectToSelect); }
    }
}
