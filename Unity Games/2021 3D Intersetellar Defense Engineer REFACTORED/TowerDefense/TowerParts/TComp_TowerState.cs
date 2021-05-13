using System.Collections;
using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TowerDefense.TowerCreation.UI.Inventory;
using UnityEngine;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Tower that can be placed to effect enemies in range. 
    /// </summary>
    public class TComp_TowerState : TowerComponent, ISerializableTowerComponent
    {
        public int StartingConstructionTime = 1;
        public int PathNumber = -1;
        public List<KeyValuePair<TComp_TowerState, string>> Path1 = new List<KeyValuePair<TComp_TowerState, string>>();
        public List<KeyValuePair<TComp_TowerState, string>> Path2 = new List<KeyValuePair<TComp_TowerState, string>>();
        public List<KeyValuePair<TComp_TowerState, string>> Path3 = new List<KeyValuePair<TComp_TowerState, string>>();
        
        [SerializeField] int price;
        [SerializeField] TComp_TowerState _predecessor;
        [SerializeField] TComp_TowerState _successor;
        [SerializeField] TP_TowerBase _towerBase;
        [SerializeField] TP_WeaponMount _weaponMount;
        
        TComp_TowerState _root;
        public bool _isRoot;
        bool _isPathed;
        
        public void SaveToFile()
        {
            CustomPartFilePath = TC_UI_TP_Inventory.ROOT_DIR + TC_UI_TP_Inventory.TOWER_STATE_DIR + name + ".json";
            Dictionary<string, string> dict = new Dictionary<string, string>()
            {
                {"File Path", CustomPartFilePath},
                {"Name", name},
                {"Base Path", _towerBase.CustomPartFilePath},
                {"Mount Path", _weaponMount.CustomPartFilePath},
                {"Mount Style Path", _weaponMount.GetStyle().CustomPartFilePath},
                {"Construction Time", StartingConstructionTime.ToString()}
            };
            
            _towerBase.SaveToFile();
            _weaponMount.SaveToFile();
            _weaponMount.GetStyle().SaveToFile();
            
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
            SetBase(_towerBase);
            SetMount(_weaponMount);
            SetRotatableParts(new GameObject[]{_towerBase.gameObject, _weaponMount.gameObject});
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
            newView.transform.localPosition = new Vector3(0, 13, -20);
            newView.orthographicSize = 10;
            newView.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            state.SetView(newView);

            return state;
        }

        public void SetRoot(TComp_TowerState newRoot, int newPathNumber)
        {
            _root = newRoot;
            _isPathed = true;
            PathNumber = newPathNumber;
        }

        public void SetRootsAfterTime() { StartCoroutine(SetRootsTimer()); }

        public TComp_TowerState GetRoot() { return _root; }

        public void ClearRoot()
        {
            _isPathed = false;
            _root = null;
            PathNumber = -1;
        }

        public bool IsPathed() { return _isPathed; }

        public int GetPathNumber() { return PathNumber; }

        public void SetPath1(List<KeyValuePair<TComp_TowerState, string>> newPath) { Path1 = newPath; }
        public void SetPath2(List<KeyValuePair<TComp_TowerState, string>> newPath) { Path2 = newPath; }
        public void SetPath3(List<KeyValuePair<TComp_TowerState, string>> newPath) { Path3 = newPath; }
        public void SetPredecessor(TComp_TowerState newPredecessor) { _predecessor = newPredecessor; }
        public void SetSuccessor(TComp_TowerState newSuccessor) { _successor = newSuccessor; }
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
        public void FindPaths()
        {
            List<KeyValuePair<TComp_TowerState, string>> newPath1 = new List<KeyValuePair<TComp_TowerState, string>>();
            List<KeyValuePair<TComp_TowerState, string>> newPath2 = new List<KeyValuePair<TComp_TowerState, string>>();
            List<KeyValuePair<TComp_TowerState, string>> newPath3 = new List<KeyValuePair<TComp_TowerState, string>>();
            FindTowersFromScene(ref newPath1, Path1);
            FindTowersFromScene(ref newPath2, Path2);
            FindTowersFromScene(ref newPath3, Path3);
            Path1 = newPath1;
            Path2 = newPath2;
            Path3 = newPath3;
            foreach (KeyValuePair<TComp_TowerState, string> state in Path1) { state.Key.SetRoot(this, 1); }
            foreach (KeyValuePair<TComp_TowerState, string> state in Path2) { state.Key.SetRoot(this, 2); }
            foreach (KeyValuePair<TComp_TowerState, string> state in Path3) { state.Key.SetRoot(this, 3); }
        }

        void FindTowersFromScene(ref List<KeyValuePair<TComp_TowerState, string>> newPath, List<KeyValuePair<TComp_TowerState, string>> oldPath)
        {
            // List<TowerState> states = InventoryManager.inventoryManager.GetTowerStates();
            // for (int i = 0; i < oldPath.Count; i++)
            // {
            //     for (int j = 0; j < states.Count; j++)
            //     {
            //         if (states[j].CustomPartFilePath == oldPath[i].Value)
            //         {
            //             newPath.Add(new KeyValuePair<TowerState, string>(states[j], states[j].CustomPartFilePath));
            //         }
            //     }
            // }
        }

        void SetUpgradePaths()
        {
            for (int i = 0; i < Path1.Count - 1; i++)
            {
                Path1[i + 1].Key.SetPredecessor(Path1[i].Key);
                Path1[i].Key.SetSuccessor(Path1[i + 1].Key);
            }
            for (int i = 0; i < Path2.Count - 1; i++)
            {
                Path2[i + 1].Key.SetPredecessor(Path2[i].Key);
                Path2[i].Key.SetSuccessor(Path2[i + 1].Key);
            }
            for (int i = 0; i < Path3.Count - 1; i++)
            {
                Path3[i + 1].Key.SetPredecessor(Path3[i].Key);
                Path3[i].Key.SetSuccessor(Path3[i + 1].Key);
            }
        }
        
        IEnumerator SetRootsTimer()
        {
            yield return new WaitForSeconds(0.1f);

            foreach (KeyValuePair<TComp_TowerState, string> state in Path1) { if (state.Key) state.Key.SetRoot(this, 1); }
            foreach (KeyValuePair<TComp_TowerState, string> state in Path2) { if (state.Key) state.Key.SetRoot(this, 2); }
            foreach (KeyValuePair<TComp_TowerState, string> state in Path3) { if (state.Key) state.Key.SetRoot(this, 3); }
            yield return new WaitForSeconds(1);
            TComp_TowerState[] states = FindObjectsOfType<TComp_TowerState>();
            foreach (TComp_TowerState state in states) { if (state.name == "") { Destroy(state.gameObject); } }
        }
    }
}