using UnityEngine;
namespace HobbitUtilz.InvocableBehaviors
{
    /// <summary>
    /// Invokes multiple invocable objects. Made to chain invokable objects as a single invocation. 
    /// </summary>
    public class HU_Invoke_MultiInvoke : HU_InvocableObject
    {
        [SerializeField] HU_InvocableObject[] _invocableObjects;
        public override void Invoke() { foreach (HU_InvocableObject invocableObject in _invocableObjects) { invocableObject.Invoke(); } }
    }
}