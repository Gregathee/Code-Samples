using System;
using System.Collections;
using HobbitUtilz.InvocableBehaviors;
using TowerDefense.TowerCreation.Factories.Weapon;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Manages Tower's tower paths
    /// </summary>
    public class TC_Fac_TowerPath : MonoBehaviour
    {
        public static TC_Fac_TowerPath Instance;
        
        [SerializeField] HU_InvocableObject _enterTowerInvocation;
        [SerializeField] HU_InvocableObject _exitTowerInvocation;
        [SerializeField] GameObject _towerContent;
        [SerializeField] TC_TComp_AssignmentSlot _rootSlot;
        [SerializeField] TC_TComp_AssignmentSlot[] _path1;
        [SerializeField] TC_TComp_AssignmentSlot[] _path2;
        [SerializeField] TC_TComp_AssignmentSlot[] _path3;

        TComp_TowerState _rootState;
        TC_UI_TP_InventorySlot[] _towerInventory;

        void Awake()
        {
            if (!Instance) { Instance = this; }
            else{ Destroy(gameObject); return; }
            TC_TComp_AssignmentSlot.OnSlotAssign += TakeSlotFromInventory;
            TC_TComp_AssignmentSlot.OnSlotAssign += LeftAlignSlots;
            TC_TComp_AssignmentSlot.OnSlotClear += ReturnSlotToInventory;
            TC_TComp_AssignmentSlot.OnSlotClear += LeftAlignSlots;
        }
        
        /// <summary>
        /// Method for button to active the tower path editor
        /// </summary>
        public void EnterPathEditor()
        {
            if (!TC_UI_TP_InventorySlot.SlotFollowingCursor)
            {
                TC_UI_ConfirmationManager.Instance.PromptMessage("You must select a tower first.", false, false);
                return;
            }
            _towerInventory = _towerContent.GetComponentsInChildren<TC_UI_TP_InventorySlot>();
            _rootSlot.SetTowerComponent(TC_UI_TP_InventorySlot.SlotFollowingCursor.GetTowerComponent());
            _rootState = _rootSlot.GetTowerComponent().GetComponent<TComp_TowerState>();

            TC_UI_TP_InventorySlot.SlotFollowingCursor = null;
            
            // Prevent a pathed tower from entering the tower path editor
            if (!_rootState.IsRoot() && _rootState.IsPathed())
            {
                TComp_TowerState root = _rootState.GetRoot();
                string message = "This tower belongs to the path of " + TD_Globals.PartNameColor + root.name + 
                    TD_Globals.StandardWordColor + ".";
                TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
                return;
            }
            
            EnableInventorySlot(ref _rootState, false);
            LoadTowerPaths();
            
            foreach (TC_UI_TP_InventorySlot slot in _towerContent.GetComponentsInChildren<TC_UI_TP_InventorySlot>())
            {
                TComp_TowerState state = slot.GetTowerComponent().GetComponent<TComp_TowerState>();
                if (!state) { continue; }
                
                // Remove tower states that are a part of another towers path.
                if(state.IsRoot() || state.IsPathed()){EnableInventorySlot(ref state, false);}
            }
            _enterTowerInvocation.Invoke();
        }
        
        public void ExitPathEditor()
        {
            foreach (TC_UI_TP_InventorySlot slot in _towerContent.GetComponentsInChildren<TC_UI_TP_InventorySlot>(true))
            {
                slot.gameObject.SetActive(true);
            }
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = true;
            foreach(TC_TComp_AssignmentSlot slot in _path1){slot.ClearSlot();}
            foreach(TC_TComp_AssignmentSlot slot in _path2){slot.ClearSlot();}
            foreach(TC_TComp_AssignmentSlot slot in _path3){slot.ClearSlot();}
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = false;
            _exitTowerInvocation.Invoke();
        }

        public void Confirm()
        {
            TC_UI_ConfirmationManager.Instance.PromptMessage("Save these Tower Paths?", true, false, AssignPaths);
        }
        
        /// <summary>
        /// Shifts tower path slots so there are no empty slots before filled slots.
        /// </summary>
        /// <param name="path"></param>
        static void LeftAlignPath(ref TC_TComp_AssignmentSlot[] path)
        {
            TC_TComp_AssignmentSlot nullSlot = null;
            for (int i = 0; i < path.Length; i++)
            {
                if (nullSlot != null)
                {
                    if (path[i].GetTowerComponent() == null) { continue; }
                    nullSlot.SetTowerComponent(path[i].GetTowerComponent());
                    path[i].ClearSlot();
                    nullSlot = path[i];
                    i--;
                    continue;
                }
                if (path[i].GetTowerComponent() != null){ continue; }
                nullSlot = path[i];
            }
        }
        
        void LeftAlignSlots(object sender, EventArgs e)
        {
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = true;
            LeftAlignPath(ref _path1);
            LeftAlignPath(ref _path2);
            LeftAlignPath(ref _path3);
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = false;
        }
        
        void EnableInventorySlot(ref TComp_TowerState towerState, bool enable)
        {
            if (_towerInventory == null) return;
            foreach (TC_UI_TP_InventorySlot slot in _towerInventory)
            {
                if (slot.GetTowerComponent() != towerState) continue;
                slot.gameObject.SetActive(enable);
                break;
            }
        }

        void TakeSlotFromInventory(object sender, TC_TComp_AssignmentSlotEventArgs e)
        {
            TComp_TowerState state = e.TowerComponent.GetComponent<TComp_TowerState>();
            EnableInventorySlot(ref state, false);
            TC_UI_TP_InventorySlot.SlotFollowingCursor?.DeselectSlot();
        }

        public void ReturnSlotToInventory(object sender, TC_TComp_AssignmentSlotEventArgs e)
        {
            TComp_TowerState state = e.TowerComponent?.GetComponent<TComp_TowerState>();
            if(!state) return;
            EnableInventorySlot(ref state, true);
        }
        
        void LoadTowerPaths()
        {
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = true;
            LoadTowerPath(ref _path1, _rootState.GetPath1());
            LoadTowerPath(ref _path2, _rootState.GetPath2());
            LoadTowerPath(ref _path3, _rootState.GetPath3());
            TC_TComp_AssignmentSlot.IgnoreOnSlotAssign = false;
        }

        void LoadTowerPath(ref TC_TComp_AssignmentSlot[] _path, TComp_TowerState[] towerPath)
        {
            for (int i = 0; i < towerPath.Length; i++)
            {
                if (!towerPath[i]) { return; }
                _path[i].SetTowerComponent(towerPath[i]);
                EnableInventorySlot(ref towerPath[i], false);
            }
        }

        /// <summary>
        /// Assigns tower states in assignment slots to be the path of the root state.
        /// </summary>
        void AssignPaths()
        {
            _rootState.SetPath1(AssignPath(ref _path1));
            _rootState.SetPath2(AssignPath(ref _path2));
            _rootState.SetPath3(AssignPath(ref _path3));
            _rootState.SaveToFile();
            foreach (TC_UI_TP_InventorySlot slot in _towerContent.GetComponentsInChildren<TC_UI_TP_InventorySlot>())
            {
                slot.GetTowerComponent().GetComponent<TComp_TowerState>().ClearRoot();
            }
            ExitPathEditor();
        }

        TComp_TowerState[] AssignPath(ref TC_TComp_AssignmentSlot[] path)
        {
            TComp_TowerState[] resultPath = new TComp_TowerState[_path1.Length];
            for (int i = 0; i < _path1.Length; i++)
            {
                resultPath[i] = path[i].GetTowerComponent()?.GetComponent<TComp_TowerState>();
            }
            return resultPath;
        }
    }
}
