using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using UnityEngine;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;

namespace TowerDefense.TowerParts.Weapon
{
    /// <summary>
    /// Base class for weapons.
    /// </summary>
    public abstract class TP_Weapon : ColoredTowerPart
    {
        public static bool AttachingWeapon;
        static readonly float snapAngle = 22.5f;
        const float DISTANCE_FROM_CAMERA = 21.25F;

        bool _isLookingAtMouse;

        [SerializeField] GameObject _collisionIndicator;
        
        List<Collider> _touchingWeapons = new List<Collider>();

        protected override void  Update()
        {
            base.Update();
            _collisionIndicator.SetActive(IsTouchingWeapon());
            if (!_isLookingAtMouse) return;
        }

        void OnMouseDown()
        {
            if (!TC_UI_DetachWeapon.DetachWeapons) return;
            WeaponMountSlot slot;
            if (slot = GetComponentInParent<WeaponMountSlot>()) { slot.ClearSlot(); }
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            TP_Weapon tpWeapon = other.GetComponentInParent<TP_Weapon>();
            if (tpWeapon && !IsPreview) { _touchingWeapons.Add(other); }
        }

        void OnTriggerExit(Collider other)
        {
            TP_Weapon tpWeapon = other.GetComponentInParent<TP_Weapon>();
            if (tpWeapon && !IsPreview) { _touchingWeapons.Remove(other); }
        }

        /// <summary>
        /// Change the scale of the weapon according to its size property.
        /// </summary>
        public override void ScaleToSize()
        {
            Shrink = false;
            Hide = false;
            if(GetComponentInParent<TP_WeaponMount>() && GetComponent<TP_WeaponMountStyle>())
            { transform.localScale = Vector3.one; return; }
            switch (size)
            {
                case PartSize.Small:
                    foreach(GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
                    break;
                case PartSize.Medium:
                    foreach(GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = Vector3.one; 
                    break;
                case PartSize.Large:
                    foreach(GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); 
                    break;
            }
        }

        public abstract void Fire(Transform target);

        public abstract void RemoveFromInventory();

        public override void DeleteFile(bool forceDelete)
        {
            if (forceDelete){ ForceDelete(); return; }
            if (FileInUse()) { return;}
            string message = "Are you sure you want to delete " + TD_Globals.PartNameColor + name + 
                TD_Globals.StandardWordColor + "?";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message, true, false, ForceDelete);
        }
        
        public bool FileInUse()
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TOWER_STATE_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro towerReader = new StreamReaderPro(file);
                Dictionary<string, string> towerDict = HU_Functions.JSON_To_Dict(towerReader.ToString());
                
                StreamReaderPro mountStyleReader = new StreamReaderPro(towerDict["Mount Style Path"]);
                Dictionary<string, string> mountStyleDict = HU_Functions.JSON_To_Dict(mountStyleReader.ToString());

                int i = 0;
                while (mountStyleDict.ContainsKey("Weapon " + i))
                {
                    if (mountStyleDict["Weapon " + i] == CustomPartFilePath)
                    {
                        string message = "This Weapon is being used by " + TD_Globals.PartNameColor + towerDict["Name"] + 
                            TD_Globals.StandardWordColor + ".";
                        TC_UI_ConfirmationManager.Instance.PromptMessage(message, false, false);
                        return true;
                    }
                    i++;
                }
            }
            return false;
        }

        /// <summary>
        /// On edit, this updates any tower that references this.
        /// </summary>
        /// <param name="oldPart"></param>
        public override void UpdateDependencies(TowerComponent oldPart)
        {
            string directory = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TOWER_STATE_DIR;
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                if(file.Remove(0, file.Length-5).Contains(".meta")) {continue;}
                StreamReaderPro towerReader = new StreamReaderPro(file);
                Dictionary<string, string> towerDict = HU_Functions.JSON_To_Dict(towerReader.ToString());

                TC_UI_TP_Inventory inventory = TComp_TowerState.Inventory;
                TComp_TowerState state = inventory.FindTowerComponent(towerDict["Name"]).GetComponent<TComp_TowerState>();

                foreach (WeaponMountSlot slot in state.GetMount().GetStyle().GetSlots())
                {
                    if (slot.GetWeapon().CustomPartFilePath == oldPart.CustomPartFilePath)
                    {
                        slot.SetWeapon(Instantiate(this));
                    }
                }
                state.SaveToFile();
            }
        }

        /// <summary>
        /// When the weapon becomes a child of mount slot its scale gets adjusted. This keeps the original size.
        /// </summary>
        public void CompensateScale()
        {
            transform.localScale = Vector3.one;
            SetShrink(false);
            SetHide(false);
            Transform parent = transform.parent;
            TowerPart parentPart = null;
            if (parent) { parentPart = parent.parent.GetComponent<TowerPart>(); }
            if (!parentPart) return;
            transform.localPosition = new Vector3(0, 0.08f, 0);
            PartSize parentSize = parentPart.GetSize();
            switch (parentSize)
            {
                case PartSize.Small: 
                    CompensateSize(new Vector3(1, 0.5f, 1), new Vector3(2, 1, 2), new Vector3(3, 1.5f, 3)); 
                    break;
                case PartSize.Medium: 
                    CompensateSize(new Vector3(0.5f, 0.5f, 0.5f), Vector3.one, new Vector3(1.5f, 1.5f, 1.5f)); 
                    break;
                case PartSize.Large: 
                    CompensateSize(new Vector3(0.333f, 0.5f, 0.333f), new Vector3(0.666f, 1, 0.666f), new Vector3(1, 1.5f, 1)); 
                    break;
            }
        }

        public bool IsTouchingWeapon()
        {
            int i = 0;
            int count = _touchingWeapons.Count;
            while (i < count)
            {
                if (_touchingWeapons[i] == null) { _touchingWeapons.RemoveAt(i); }
                else i++;
                count = _touchingWeapons.Count;
            }
            return _touchingWeapons.Count > 0;
        }

        public void LookAtMouse()
        {
            StartCoroutine(SelectWeaponRotation(DISTANCE_FROM_CAMERA));
        }

        void ForceDelete()
        {
            File.Delete(CustomPartFilePath);
            RemoveFromInventory();
        }

        void CompensateSize(Vector3 small, Vector3 medium, Vector3 large)
        {
            switch (size)
            {
                case PartSize.Small:
                    foreach (GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = small;
                    break;
                case PartSize.Medium:
                    foreach (GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = medium;
                    break;
                case PartSize.Large:
                    foreach (GameObject gameOb in _rotatableParts)
                        gameOb.transform.localScale = large;
                    break;
            }
        }
        
        void DestroyWeaponIfTouching()
        {
            if (IsTouchingWeapon()) { GetComponentInParent<WeaponMountSlot>().ClearSlot(); }
        }

        void CorrectWeaponRotation()
        {
            if (!Input.GetKey(KeyCode.LeftAlt)) transform.rotation = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
            else {transform.eulerAngles = GetSnapRotation(transform.eulerAngles.y);}
            CorrectRotation();
        }

        void PointWeaponToMouse(ref float cameraDistance)
        {
            Vector3 temp = Input.mousePosition;
            temp.z = cameraDistance; // Set this to be the distance you want the object to be placed in front of the camera.
            temp = Camera.main.ScreenToWorldPoint(temp);
            transform.LookAt(temp);
        }

        Vector3 GetSnapRotation(float current_Y)
        {
            int new_y = (int)Mathf.Round(current_Y / snapAngle);
            Vector3 newRotation = new Vector3(0, new_y * snapAngle, 0);
            return newRotation;
        }
        
        /// <summary>
        /// On weapon attach, look at mouse for the player to select the rotation until click.
        /// </summary>
        /// <param name="cameraDistance"></param>
        /// <returns></returns>
        IEnumerator SelectWeaponRotation(float cameraDistance)
        {
            _isLookingAtMouse = true;
            while(!Input.GetMouseButtonDown(0))
            {
                PointWeaponToMouse(ref cameraDistance);
                CorrectWeaponRotation();
                yield return null;
            }
            _isLookingAtMouse = false;
            DestroyWeaponIfTouching();
            GetComponentInParent<WeaponMountSlot>().SavedLocalRotation = transform.localRotation;
        }
    }
}