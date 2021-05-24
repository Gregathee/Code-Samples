using System;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories
{
    public class TC_TComp_AssignmentSlotEventArgs : EventArgs
    {
        public TC_TComp_AssignmentSlotEventArgs(TowerComponent towerComponent) { TowerComponent = towerComponent;}
        public TowerComponent TowerComponent;
    }
    
    /// <summary>
    /// A UI slot that holds and displays a reference to tower component such as ammo during weapon creation or tower states during path editing.
    /// </summary>
    public class TC_TComp_AssignmentSlot : MonoBehaviour
    {
        public static event EventHandler<TC_TComp_AssignmentSlotEventArgs> OnSlotAssign;
        public static event EventHandler<TC_TComp_AssignmentSlotEventArgs> OnSlotClear;
        public static bool IgnoreOnSlotAssign;
        
        TowerComponent _towerComponent;
        [SerializeField] bool _isInteractable = true;
        [SerializeField] TC_Fac_TowerPart _factory;
        [SerializeField] RawImage _rawImage;
        [SerializeField] TC_UI_FloatingToolTip _toolTipPrefab;
        Transform _imageParent;
        
        TC_UI_FloatingToolTip _toolTipInstance;

        bool _mouseOver;
        bool _destroyOnClearSlot;

        void Awake()
        {
            _rawImage.color = new Color(0, 0, 0, 0);
            _imageParent = GetComponentInParent<Canvas>().transform;
        }

        void Update()
        {
            if (!_mouseOver) return;
            if (_towerComponent && Input.GetMouseButtonDown(1) && _isInteractable) { ClearSlot(); return; }
            if (!Input.GetMouseButtonUp(0)) return;
            // When tower component is being dragged, the component's inventory slot is SlotFollowingCursor.
            if(TC_UI_TP_InventorySlot.SlotFollowingCursor && _isInteractable) {SetTowerComponent(TC_UI_TP_InventorySlot.SlotFollowingCursor.GetTowerComponent());}
        }

        /// <summary>
        /// Sets the tower component and displays its image in the slot.
        /// </summary>
        /// <param name="towerComponent"></param>
        /// <param name="destroyOnClearSlot"></param>
        public void SetTowerComponent(TowerComponent towerComponent, bool destroyOnClearSlot = false)
        {
            if(!_destroyOnClearSlot)_destroyOnClearSlot = destroyOnClearSlot;
            if (!towerComponent){ return;}
            if (_towerComponent)
            {
                TC_Fac_TowerPath.Instance?.ReturnSlotToInventory(this, new TC_TComp_AssignmentSlotEventArgs(_towerComponent));
                if(_factory.IsEditing()) {ClearSlot();}
            }
            SetComponent(towerComponent);
        }

        /// <summary>
        /// Sets the tower component and displays its image in the slot.
        /// </summary>
        /// <param name="towerComponent"></param>
        /// <param name="destroyOnClearSlot"></param>
        void SetComponent(TowerComponent towerComponent)
        {
            _towerComponent = towerComponent;
            _rawImage.texture = _towerComponent.GetView().targetTexture;
            _rawImage.color = Color.white;
            if(!IgnoreOnSlotAssign) OnSlotAssign?.Invoke(this, new TC_TComp_AssignmentSlotEventArgs(towerComponent));
            TC_UI_TP_InventorySlot.SlotFollowingCursor = null;
            if (!_mouseOver) { return;}
            _toolTipInstance = Instantiate(_toolTipPrefab, _imageParent);
            _toolTipInstance.Initialize(_towerComponent.name);
        }

        public TowerComponent GetTowerComponent() { return _towerComponent;}

        // Mouse Enter is called from UI events attached to this object.
        public void MouseEnter()
        {
            _mouseOver = true;
            if (!_towerComponent) { return;}
            _toolTipInstance = Instantiate(_toolTipPrefab, _imageParent);
            _toolTipInstance.Initialize(_towerComponent.name);
        }
        public void MouseExit()
        {
            _mouseOver = false;
            if(_toolTipInstance){Destroy(_toolTipInstance.gameObject);}
            _toolTipInstance = null;
        }

        public void ClearSlot()
        {
            if (!_towerComponent) return;
            if (_destroyOnClearSlot)
            {
                Destroy(_towerComponent.gameObject);
                _towerComponent = null; 
                _rawImage.color = new Color(0, 0, 0, 0);
                return;
            }
            TowerComponent towerComponent = _towerComponent;
            _towerComponent = null; 
            _rawImage.color = new Color(0, 0, 0, 0);
            if(!IgnoreOnSlotAssign)OnSlotClear?.Invoke(this, new TC_TComp_AssignmentSlotEventArgs(towerComponent));
            
            if(_toolTipInstance){Destroy(_toolTipInstance.gameObject);}
            _toolTipInstance = null;
        }
    }
}
