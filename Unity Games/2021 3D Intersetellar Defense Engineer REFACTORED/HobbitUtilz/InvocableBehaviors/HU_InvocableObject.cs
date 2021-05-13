using UnityEngine;

namespace HobbitUtilz.InvocableBehaviors
{
    /// <summary>
    /// Interface for invocable behaviors.
    /// </summary>
    public abstract class HU_InvocableObject : MonoBehaviour
    {
        [SerializeField] bool _InvokeOnStart;

        void Start() { if(_InvokeOnStart){Invoke();} }
        public abstract void Invoke();
    }
}