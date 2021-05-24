using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Tower that can be placed to effect enemies in range. 
    /// </summary>
    public class TComp_TowerState : TowerComponent, ISerializableTowerComponent
    {
        public static TC_UI_TP_Inventory Inventory;
        public static TC_Fac_Tower Factory;
        
        //Key value pairs representing the lower (key) and upper (value) bounds of attributes
        public static readonly KeyValuePair<int, int> CONSTRUCTION_TIME_BOUNDS = new KeyValuePair<int, int>(1, 100);
        
        public int StartingConstructionTime = 1;
        public int PathNumber = -1;
        TComp_TowerState[] _path1 = new TComp_TowerState[5];
        TComp_TowerState[] _path2 = new TComp_TowerState[5];
        TComp_TowerState[] _path3 = new TComp_TowerState[5];
        
        [SerializeField] int price;
        [SerializeField] TComp_TowerState _predecessor;
        [SerializeField] TComp_TowerState _successor;
        [SerializeField] TP_TowerBase _towerBase;
        [SerializeField] TP_WeaponMount _weaponMount;
        
        TComp_TowerState _root;
        bool _isRoot;
        bool _isPathed;
        
        public void SaveToFile()
        {
            _towerBase.name = name + " Tower Base";
            _weaponMount.name = name + " Weapon Mount";
            _weaponMount.GetStyle().name = name + " Weapon Mount Style";
            
            _towerBase.SaveToFile();
            _weaponMount.SaveToFile();
            _weaponMount.GetStyle().SaveToFile();
            
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Name", name},
                {"Slot Number", SlotNumber.ToString()},
                {"Base Path", _towerBase.CustomPartFilePath},
                {"Mount Path", _weaponMount.CustomPartFilePath},
                {"Mount Style Path", _weaponMount.GetStyle().CustomPartFilePath},
                {"Size", ((int)_towerBase.GetSize()).ToString()},
                {"Construction Time", StartingConstructionTime.ToString()},
                {"Is Root", _isRoot.ToString()}
            };

            if (_isRoot) { AddPathsToDict(ref dict); }
            
            StreamWriter writer = new StreamWriter(CustomPartFilePath);
            writer.Write(HU_Functions.Dict_To_JSON(dict));
            writer.Dispose();
        }
        public void SetPropertiesFromJSON(Dictionary<string, string> jsonDict)
        {
            name = jsonDict["Name"];
            _towerBase = TowerPart.LoadTowerPartFromFile(jsonDict["Base Path"]).GetComponent<TP_TowerBase>();
            _weaponMount = TowerPart.LoadTowerPartFromFile(jsonDict["Mount Path"]).GetComponent<TP_WeaponMount>();
            _weaponMount.SetStyle(TowerPart.LoadTowerPartFromFile(jsonDict["Mount Style Path"]).GetComponent<TP_WeaponMountStyle>());
            StartingConstructionTime = int.Parse(jsonDict["Construction Time"]);
            _isRoot = bool.Parse(jsonDict["Is Root"]);
            CustomPartFilePath = jsonDict["File Path"];
            SlotNumber = int.Parse(jsonDict["Slot Number"]);

            PartSize size = (PartSize)int.Parse(jsonDict["Size"]);
            _towerBase.SetSize(size, true);
            _weaponMount.SetSize(size, true);
            _weaponMount.GetStyle().SetSize(size, true);
            
            
            SetBase(_towerBase);
            SetMount(_weaponMount);
            SetRotatableParts(new GameObject[]{_towerBase.gameObject, _weaponMount.gameObject});

            ClampStats();
            
            StartCoroutine(LoadPathsFromJson(jsonDict));
        }
        
        public void DeleteFile(bool forceDelete)
        {
            if(forceDelete) {DeleteFiles(false); return; }
            if (_isPathed)
            {
                string message1 = "This Tower is a part of the path of " + 
                    TD_Globals.PartNameColor + _root.name + 
                    TD_Globals.StandardWordColor + ".\n\nAre you sure you want to delete " +
                    TD_Globals.PartNameColor + name + TD_Globals.StandardWordColor + "?";
                TC_UI_ConfirmationManager.Instance.PromptMessage(message1, true, false, DeleteFilesAndManagePaths);
                return;
            }
            string message2 = "Are you sure you want to delete " + TD_Globals.PartNameColor + name + TD_Globals.StandardWordColor + "?";
            TC_UI_ConfirmationManager.Instance.PromptMessage(message2, true, false, DeleteFilesAndManagePaths);
        }

        /// <summary>
        /// On edit, updates all towers that reference this.
        /// </summary>
        /// <param name="oldPart"></param>
        public void UpdateDependencies(TowerComponent oldPart)
        {
            TComp_TowerState oldState = oldPart.GetComponent<TComp_TowerState>();
            if (oldState._isRoot)
            {
                SetPath1(oldState._path1);
                SetPath2(oldState._path2);
                SetPath3(oldState._path3);
                return;
            }
            if (!oldState._isPathed) { return; }
            TransferSelfToOldRoot(ref oldState);
        }

        public TC_UI_TP_Inventory GetInventory() { return Inventory;}
        public TC_Fac_TowerPartFactory GetFactory() { return Factory; }

        public void GenerateFileName()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TOWER_STATE_DIR + name + ".json";
        }

        public override string GetStats()
        {
            string stats = "";

            stats += TD_Globals.StandardWordColor + "Cost: " + TD_Globals.PartNameColor + Cost() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Size: " + TD_Globals.PartNameColor + _towerBase.GetSize() + "\n"; 
            stats += TD_Globals.StandardWordColor + "Turn Radius: " + TD_Globals.PartNameColor + _weaponMount.TurretAngle + "\n";
            stats += TD_Globals.StandardWordColor + "Rotation Speed: " + TD_Globals.PartNameColor + _weaponMount.RotationalSpeed + "\n"; 
            stats += TD_Globals.StandardWordColor + "Range: " + TD_Globals.PartNameColor + _weaponMount.Range + "\n"; 
            stats += TD_Globals.StandardWordColor + "Construction Time: " + TD_Globals.PartNameColor + StartingConstructionTime + "\n"; 
            stats += TD_Globals.StandardWordColor + "Weapons:\n\t";
            foreach (WeaponMountSlot slot in _weaponMount.GetStyle().GetSlots())
            {
                stats += TD_Globals.PartNameColor + slot.GetWeapon().name + "\n\t";
            }
            
            return stats;
        }

        public override int Cost()
        {
            int result = 0;

            //TODO

            return result;
        }

        public override void SetIsPreview(bool preview)
        {
            base.SetIsPreview(preview);
            _towerBase.SetIsPreview(false);
            _weaponMount.SetIsPreview(false);
            _weaponMount.GetStyle().SetIsPreview(false);
        }

        public static TowerComponent LoadTowerPartFromFile(string file)
        {
            StreamReaderPro streamReader = new StreamReaderPro(file);
            Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
            
            GameObject towerObject = new GameObject();
            TComp_TowerState state = towerObject.AddComponent<TComp_TowerState>();
            state.SetPropertiesFromJSON(jsonDict);
            
            string cameraPath = "Prefabs/TowerParts/StateCamera";
            Camera newView = Instantiate(Resources.Load<Camera>(cameraPath));

            newView.transform.SetParent(state.transform);
            newView.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            state.SetView(newView);

            return state;
        }

        public void SetPath1(TComp_TowerState[] newPath) { SetPath(ref _path1, ref newPath, 1); }
        public void SetPath2(TComp_TowerState[] newPath) { SetPath(ref _path2, ref newPath, 2); }
        public void SetPath3(TComp_TowerState[] newPath) { SetPath(ref _path3, ref newPath, 3); }

        public TComp_TowerState[] GetPath1() { return _path1;}
        public TComp_TowerState[] GetPath2() { return _path2;}
        public TComp_TowerState[] GetPath3() { return _path3;}

        void ClampStats()
        {
            KeyValuePair<int, int> pair = TP_WeaponMount.ROTATION_SPEED_BOUNDS;
            _weaponMount.RotationalSpeed = Mathf.Clamp(_weaponMount.RotationalSpeed, pair.Key, pair.Value);
            pair = TP_WeaponMount.RANGE_BOUNDS;
            _weaponMount.Range = Mathf.Clamp(_weaponMount.Range, pair.Key, pair.Value);
            pair = CONSTRUCTION_TIME_BOUNDS;
            StartingConstructionTime = Mathf.Clamp(StartingConstructionTime, pair.Key, pair.Value);
        }

        void TransferSelfToOldRoot(ref TComp_TowerState oldState)
        {
            TComp_TowerState[] path = null;
            switch (oldState.PathNumber)
            {
                case 1: path = oldState._root._path1; break;
                case 2: path = oldState._root._path2; break;
                case 3: path = oldState._root._path3; break;
            }

            foreach (TComp_TowerState state in path) { Debug.Log(state); }

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == oldState)
                {
                    path[i] = this;
                }
            }
            
            SetRoot(oldState._root, oldState.PathNumber);
            _root.SaveToFile();
            
            foreach (TComp_TowerState state in path) { Debug.Log(state); }
        }

        void SetPath(ref TComp_TowerState[] oldPath, ref TComp_TowerState[] newPath, int pathNumber)
        {
            _isRoot = true;
            oldPath = newPath;
            foreach (TComp_TowerState state in oldPath) { state?.SetRoot(this, pathNumber); }
            CheckIfAllPathsAreNull();
        }

        void CheckIfAllPathsAreNull()
        {
            foreach (TComp_TowerState state in _path1) { if (state) { return; } }
            foreach (TComp_TowerState state in _path2) { if (state) { return; } }
            foreach (TComp_TowerState state in _path3) { if (state) { return; } }
            _isRoot = false;
        }

        public void SetRoot(TComp_TowerState newRoot, int newPathNumber)
        {
            _root = newRoot;
            _isPathed = true;
            _isRoot = false;
            PathNumber = newPathNumber;
        }

        public TComp_TowerState GetRoot() { return _root; }

        public void ClearRoot()
        {
            _isPathed = false;
            _root = null;
            PathNumber = -1;
        }

        public void ClearPaths()
        {
            for (int i = 0; i < _path1.Length; i++) { _path1[i] = null;}
            for (int i = 0; i < _path2.Length; i++) { _path2[i] = null;}
            for (int i = 0; i < _path3.Length; i++) { _path3[i] = null;}
            _isRoot = false;
        }

        public bool IsRoot() { return _isRoot;}

        public bool IsPathed() { return _isPathed; }

        public int GetPathNumber() { return PathNumber; }
        public void SetPredecessor(TComp_TowerState newPredecessor) { _predecessor = newPredecessor; }
        public void SetSuccessor(TComp_TowerState newSuccessor) { _successor = newSuccessor; }
        public TP_TowerBase GetBase() { return _towerBase;}
        public TP_WeaponMount GetMount() { return _weaponMount;}
        
        public void SetBase(TP_TowerBase newBase)
        {
            _towerBase = newBase;
            _towerBase.transform.SetParent(transform);
            _towerBase.transform.localPosition = Vector3.zero;
            _towerBase.SetIsPreview(false);
        }
        public void SetMount(TP_WeaponMount mount)
        {
            _weaponMount = mount;
            _weaponMount.transform.SetParent(transform);
            _weaponMount.transform.localPosition = new Vector3(0, 1.5f, 0);
            _weaponMount.SetIsPreview(false);
        }
        
        /// <summary>
        /// Removes null slots before filled slots.
        /// </summary>
        /// <param name="path"></param>
        static void LeftAlignPath(ref TComp_TowerState[] path)
        {
            int nullSlot = -1;
            for (int i = 0; i < path.Length; i++)
            {
                if (nullSlot != -1)
                {
                    if (path[i] == null) { continue; }
                    path[nullSlot] = path[i];
                    nullSlot = i;
                    path[i] = null;
                    i--;
                    continue;
                }
                if (path[i] != null){ continue; }
                nullSlot = i;
            }
        }
        
        void DeleteFilesAndManagePaths(){DeleteFiles(true);}

        void DeleteFiles(bool managePaths)
        {
            if (managePaths)
            {
                if (_isPathed) { CleanUpRootsTowerPaths(); }
                if (_isRoot) { ClearRootFromPaths(); }
            }
            _towerBase.DeleteFile(false);
            _weaponMount.DeleteFile(false);
            _weaponMount.GetStyle().DeleteFile(false);
            File.Delete(CustomPartFilePath);
            Inventory.RemovePartFromInventory(this);
        }

        void CleanUpRootsTowerPaths()
        {
            switch (PathNumber)
            {
                case 1: RemoveSelfFromRootPath(ref _root._path1); break;
                case 2: RemoveSelfFromRootPath(ref _root._path2); break;
                case 3: RemoveSelfFromRootPath(ref _root._path3); break;
            }
            _root.SaveToFile();
        }

        void RemoveSelfFromRootPath(ref TComp_TowerState[] path)
        {
            for(int i = 0; i < path.Length; i++)
            {
                if (path[i] == this) { path[i] = null; break; }
            }
            LeftAlignPath(ref path);
        }

        void ClearRootFromPaths()
        {
            switch (PathNumber)
            {
                case 1: ClearRootFromPath(ref _path1); break;
                case 2: ClearRootFromPath(ref _path2); break;
                case 3: ClearRootFromPath(ref _path3); break;
            }
        }

        void ClearRootFromPath(ref TComp_TowerState[] path)
        {
            foreach (TComp_TowerState state in path) { state.ClearRoot(); }
        }

        void AddPathsToDict(ref Dictionary<string, string> dict)
        {
            AddPathToDict(ref dict, ref _path1, 'A');
            AddPathToDict(ref dict, ref _path2, 'B');
            AddPathToDict(ref dict, ref _path3, 'C');
        }

        static void AddPathToDict(ref Dictionary<string, string> dict, ref TComp_TowerState[] path, char pathChar)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (!path[i]) { break;}
                dict.Add("Path " + pathChar + i, path[i].name);
            }
        }

        IEnumerator LoadPathsFromJson(Dictionary<string, string> jsonDict)
        {
            yield return new WaitForEndOfFrame();
            LoadPathFromJson(ref jsonDict, ref _path1, 'A');
            foreach (TComp_TowerState state in _path1) { state?.SetRoot(this, 1); }
            LoadPathFromJson(ref jsonDict, ref _path2, 'B');
            foreach (TComp_TowerState state in _path2) { state?.SetRoot(this, 2); }
            LoadPathFromJson(ref jsonDict, ref _path3, 'C');
            foreach (TComp_TowerState state in _path3) { state?.SetRoot(this, 3); }
            
        }

        static void LoadPathFromJson(ref Dictionary<string, string> jsonDict, ref TComp_TowerState[] path, char pathChar)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (!jsonDict.ContainsKey("Path " + pathChar + i)) break;
                path[i] = Inventory.FindTowerComponent(jsonDict["Path " + pathChar + i]).GetComponent<TComp_TowerState>();
            }
        }
    }
}