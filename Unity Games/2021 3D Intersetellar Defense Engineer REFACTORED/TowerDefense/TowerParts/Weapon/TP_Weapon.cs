using System.Collections.Generic;
using UnityEngine;
using TowerDefense.TowerCreation.Factories;

namespace TowerDefense.TowerParts.Weapon
{
    /// <summary>
    /// Base class for weapons.
    /// </summary>
    public abstract class TP_Weapon : ColoredTowerPart
    {
        public Quaternion SavedLocalRotation;

        [SerializeField] GameObject _collisionIndicator;
        
        List<Collider> _touchingWeapons = new List<Collider>();
        bool _isTouchingWeapon;
        bool _isMouseOver;

        protected override void Update()
        {
            base.Update();
            if (!IsPreview) { CompensateScale(); }
            int i = 0;
            int count = _touchingWeapons.Count;
            while (i < count)
            {
                if (_touchingWeapons[i] == null) { _touchingWeapons.RemoveAt(i); }
                else i++;
                count = _touchingWeapons.Count;
            }
            if (_touchingWeapons.Count > 0 || (TC_Fac_TowerPart.IsAttachingWeapons && _isMouseOver))
            {
                _collisionIndicator.SetActive(true);
                _isTouchingWeapon = true;
            }
            else if (!TC_Fac_TowerPart.IsAttachingWeapons || !_isMouseOver)
            {
                _collisionIndicator.SetActive(false);
                _isTouchingWeapon = false;
            }
            _isMouseOver = false;
        }

        void OnMouseDown()
        {
            if (!TC_Fac_TowerPart.IsAttachingWeapons) return;
            WeaponMountSlot slot;
            if (slot = GetComponentInParent<WeaponMountSlot>()) { slot.ClearSlot(); }
            Destroy(gameObject);
        }

        private void OnMouseOver() { _isMouseOver = true; }

        private void OnTriggerEnter(Collider other)
        {
            TP_Weapon tpWeapon = other.GetComponent<TP_Weapon>();
            if (tpWeapon && !IsPreview) { _touchingWeapons.Add(other); }
        }

        private void OnTriggerExit(Collider other)
        {
            TP_Weapon tpWeapon = other.GetComponent<TP_Weapon>();
            if (tpWeapon && !IsPreview) { _touchingWeapons.Remove(other); }
        }

        /// <summary>
        /// Adjust scale to maintain actual size according to size of tower base and size of weapon.
        /// </summary>
        public void CompensateScale()
        {
            SetShrink(false);
            SetHide(false);
            Transform parent = transform.parent;
            TowerPart parentPart = null;
            if (parent) { parentPart = parent.parent.GetComponent<TowerPart>(); }
            if (parentPart)
            {
                transform.localPosition = new Vector3(0, 0.08f, 0);
                PartSize parentSize = parentPart.GetSize();
                switch (parentSize)
                {
                    case PartSize.Small: CompensateSizeSmall(); break;
                    case PartSize.Medium: CompensateSizeMedium(); break;
                    case PartSize.Large: CompensateSizeLarge(); break;
                }
            }
            else base.Update();
        }

        void CompensateSizeLarge()
        {
            switch (size)
            {
                case PartSize.Small: gameObject.transform.localScale = new Vector3(0.333f, 0.5f, 0.333f); break;
                case PartSize.Medium: gameObject.transform.localScale = new Vector3(0.666f, 1, 0.666f); break;
                case PartSize.Large: gameObject.transform.localScale = new Vector3(1, 1.5f, 1); break;
            }
        }

        void CompensateSizeMedium()
        {
            switch (size)
            {
                case PartSize.Small: gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); break;
                case PartSize.Medium: gameObject.transform.localScale = new Vector3(1, 1, 1); break;
                case PartSize.Large: gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); break;
            }
        }

        void CompensateSizeSmall()
        {
            switch (size)
            {
                case PartSize.Small: gameObject.transform.localScale = new Vector3(1, 0.5f, 1); break;
                case PartSize.Medium: gameObject.transform.localScale = new Vector3(2, 1, 2); break;
                case PartSize.Large: gameObject.transform.localScale = new Vector3(3, 1.5f, 3); break;
            }
        }

        public bool IsTouchingWeapon() { return _isTouchingWeapon; }
        public abstract void Fire(Transform target);
    }
}