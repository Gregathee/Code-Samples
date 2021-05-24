using System;
using UnityEngine;

namespace HobbitUtilz.InvocableBehaviors
{
    /// <summary>
    /// Base class for invocable behaviors. Essentially a serialized delegate.
    /// </summary>
    public abstract class HU_InvocableObject : MonoBehaviour
    {
        [SerializeField] protected bool _invokeOnStart;
        [SerializeField] protected bool _invokeOnEnable;
        [SerializeField] protected bool _invokeOnDisable;

        void Start() { if(_invokeOnStart){Invoke();} }
        public abstract void Invoke();

        void OnEnable() { if(_invokeOnEnable) {Invoke();} }

        void OnDisable() { if(_invokeOnDisable){ Invoke();} }
    }
}