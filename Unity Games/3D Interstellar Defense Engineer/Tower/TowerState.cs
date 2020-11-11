using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerState : TowerPart
{
    [SerializeField] int price;
    [SerializeField] TowerState predecessor;
    [SerializeField] TowerState successor;
    [SerializeField] SelectedTowerUI towerUI;
    [SerializeField] TowerBase towerBase;
    [SerializeField] WeaponMount weaponMount;
    public int startingConstructionTime = 1;
    TowerState root;
    public bool isRoot = false;
    bool isPathed = false;
    public int pathNumber = -1;
    public List<KeyValuePair<TowerState, string>> path1 = new List<KeyValuePair<TowerState, string>>();
    public List<KeyValuePair<TowerState, string>> path2 = new List<KeyValuePair<TowerState, string>>();
    public List<KeyValuePair<TowerState, string>> path3 = new List<KeyValuePair<TowerState, string>>();

    public void SetRoot(TowerState newRoot, int newPathNumber)
	{
        root = newRoot;
        isPathed = true;
        pathNumber = newPathNumber;
	}

    public void SetRootsAfterTime()
	{
        StartCoroutine(SetRootsTimer());
	}

    IEnumerator SetRootsTimer()
	{
        yield return new WaitForSeconds(0.1f);

        foreach (KeyValuePair<TowerState, string> state in path1) { if(state.Key) state.Key.SetRoot(this, 1); }
        foreach (KeyValuePair<TowerState, string> state in path2) { if (state.Key) state.Key.SetRoot(this, 2); }
        foreach (KeyValuePair<TowerState, string> state in path3) { if (state.Key) state.Key.SetRoot(this, 3); }
        yield return new WaitForSeconds(1);
        TowerState[] states = FindObjectsOfType<TowerState>();
        foreach (TowerState state in states) { if (state.name == "") { Destroy(state.gameObject); } }
    }

    public TowerState GetRoot() { return root; }

    public void ClearRoot() { isPathed = false; root = null; pathNumber = -1; }

    public bool IsPathed() { return isPathed; }

    public int GetPathNumber() { return pathNumber; }

	public void SetPath1(List<KeyValuePair<TowerState, string>> newPath) { path1 = newPath; }
    public void SetPath2(List<KeyValuePair<TowerState, string>> newPath) { path2 = newPath; }
    public void SetPath3(List<KeyValuePair<TowerState, string>> newPath) { path3 = newPath; }
    public void SetPredecessor(TowerState newPredecessor) { predecessor = newPredecessor; }
    public void SetSuccessor(TowerState newSuccessor) { successor = newSuccessor; }
    public void SetBase(TowerBase newTowerBase) { towerBase = newTowerBase; }
    public void SetMount(WeaponMount mount) { weaponMount = mount; }
    public void FindPaths()
	{
        List<KeyValuePair<TowerState, string>> newPath1 = new List<KeyValuePair<TowerState, string>>();
        List<KeyValuePair<TowerState, string>> newPath2 = new List<KeyValuePair<TowerState, string>>();
        List<KeyValuePair<TowerState, string>> newPath3 = new List<KeyValuePair<TowerState, string>>();
        FindTowersFromScene(ref newPath1, path1); 
        FindTowersFromScene(ref newPath2, path2); 
        FindTowersFromScene(ref newPath3, path3); 
        path1 = newPath1; path2 = newPath2; path3 = newPath3;
        foreach (KeyValuePair<TowerState, string> state in path1) { state.Key.SetRoot(this, 1);  }
        foreach (KeyValuePair<TowerState, string> state in path2) { state.Key.SetRoot(this, 2); }
        foreach (KeyValuePair<TowerState, string> state in path3) { state.Key.SetRoot(this, 3); }
    }

    void FindTowersFromScene(ref List<KeyValuePair<TowerState, string>> newPath, List<KeyValuePair<TowerState, string>> oldPath)
	{
        List<TowerState> states = InventoryManager.inventoryManager.GetTowerStates();
        for (int i = 0; i < oldPath.Count; i++)
        {
            for (int j = 0; j < states.Count; j++)
            {
                if (states[j].customePartFilePath == oldPath[i].Value)
                { newPath.Add(new KeyValuePair<TowerState, string>(states[j], states[j].customePartFilePath)); }
            }
        }
    }

    void SetUpgradePaths()
	{
        for(int i = 0; i < path1.Count-1; i++)
		{
            path1[i + 1].Key.SetPredecessor(path1[i].Key);
            path1[i].Key.SetSuccessor(path1[i + 1].Key);
		}
        for(int i = 0; i < path2.Count-1; i++)
		{
            path2[i + 1].Key.SetPredecessor(path2[i].Key);
            path2[i].Key.SetSuccessor(path2[i + 1].Key);
        }
        for (int i = 0; i < path3.Count - 1; i++)
        {
            path3[i + 1].Key.SetPredecessor(path3[i].Key);
            path3[i].Key.SetSuccessor(path3[i + 1].Key);
        }
    }
}
