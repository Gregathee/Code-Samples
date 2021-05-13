using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PathBuilder : MonoBehaviour
{
    public static PathBuilder pathBuilder;
    [SerializeField] GameObject towerContent;
    [SerializeField] RawImage rootSlot;
    [SerializeField] PathBuilderSlot a1;
    [SerializeField] PathBuilderSlot a2;
    [SerializeField] PathBuilderSlot a3;
    [SerializeField] PathBuilderSlot a4;
    [SerializeField] PathBuilderSlot a5;
    [SerializeField] PathBuilderSlot b1;
    [SerializeField] PathBuilderSlot b2;
    [SerializeField] PathBuilderSlot b3;
    [SerializeField] PathBuilderSlot b4;
    [SerializeField] PathBuilderSlot b5;
    [SerializeField] PathBuilderSlot c1;
    [SerializeField] PathBuilderSlot c2;
    [SerializeField] PathBuilderSlot c3;
    [SerializeField] PathBuilderSlot c4;
    [SerializeField] PathBuilderSlot c5;
    public TowerState root;
    List<PathBuilderSlot> pathBuilderSlots;

    private void Awake()
    {
        pathBuilder = this;
    }

    private void Start()
    {
        pathBuilderSlots = new List<PathBuilderSlot>() { a1, a2, a3, a4, a5, b1, b2, b3, b4, b5, c1, c2, c3, c4, c5 };
    }

    public bool TowerOverSlot()
    {
        foreach (PathBuilderSlot slot in pathBuilderSlots) if (slot.towerOverSlot) return true;
        return false;
    }

    public void LoadTower(ref TowerState state)
    {
        foreach (PathBuilderSlot slot in pathBuilderSlots) if (slot.towerOverSlot)
            {
                slot.Initialize(state, state.GetView().targetTexture);
                foreach (TowerPartInventorySlot _slot in towerContent.GetComponentsInChildren<TowerPartInventorySlot>())
                {
                    if (state == _slot.GetTowerPart()) {  /*Destroy(_slot.gameObject);*/ _slot.gameObject.SetActive(false); }
                }
            }
    }

    public void UnLoadTower(TowerState state)
    {
        foreach (TowerPartInventorySlot _slot in towerContent.GetComponentsInChildren<TowerPartInventorySlot>(true))
        {
            if (state == _slot.GetTowerPart()) {  _slot.gameObject.SetActive(true); }
        }
    }


    public void LoadTowerStateInventory()
	{
        List<TowerState> towerStates = InventoryManager.inventoryManager.GetTowerStates();
        
        for (int i = 0; i < towerStates.Count; i++)
        {
            if (towerStates[i] != root && !towerStates[i].isRoot)
            {
                LoadTowerSlot(towerStates[i]);
				if (towerStates[i].IsPathed())
				{
                    foreach (TowerPartInventorySlot _slot in towerContent.GetComponentsInChildren<TowerPartInventorySlot>()) 
                    {
                        if(_slot.GetTowerPart() == towerStates[i]) _slot.gameObject.SetActive(false); 
                    }
                }
            }
        }
    }

    public void LoadTowerSlot(TowerState state)
	{
        InventoryManager.inventoryManager.BuildObjects(towerContent);
        InventoryManager.inventoryManager.AddButtonToObject();
        InventoryManager.inventoryManager.AddEventToObject(false);
        InventoryManager.inventoryManager.towerPartInventorySlot.Initialize(state, InventoryManager.inventoryManager.slotDetector, 1);
        InventoryManager.inventoryManager.towerPartInventorySlot.Shrink = true;
    }

    public void SetUpPaths(ref TowerState newRoot)
	{
        root = newRoot;
        rootSlot.texture = newRoot.GetView().targetTexture;
        while(root.path1.Count < 5) { root.path1.Add(new KeyValuePair<TowerState, string> (null, "")); }
        while (root.path2.Count < 5) { root.path2.Add(new KeyValuePair<TowerState, string>(null, "")); }
        while (root.path3.Count < 5) { root.path3.Add(new KeyValuePair<TowerState, string>(null, "")); }
        if ( root.path1[0].Key) { a1.Initialize(root.path1[0].Key, root.path1[0].Key.GetView().targetTexture); }
        if ( root.path1[1].Key) a2.Initialize(root.path1[1].Key, root.path1[1].Key.GetView().targetTexture);
        if (root.path1[2].Key) a3.Initialize(root.path1[2].Key, root.path1[2].Key.GetView().targetTexture);
        if ( root.path1[3].Key) a4.Initialize(root.path1[3].Key, root.path1[3].Key.GetView().targetTexture);
        if ( root.path1[4].Key) a5.Initialize(root.path1[4].Key, root.path1[4].Key.GetView().targetTexture);
        if ( root.path2[0].Key) b1.Initialize(root.path2[0].Key, root.path2[0].Key.GetView().targetTexture);
        if ( root.path2[1].Key) b2.Initialize(root.path2[1].Key, root.path2[1].Key.GetView().targetTexture);
        if ( root.path2[2].Key) b3.Initialize(root.path2[2].Key, root.path2[2].Key.GetView().targetTexture);
        if ( root.path2[3].Key) b4.Initialize(root.path2[3].Key, root.path2[3].Key.GetView().targetTexture);
        if ( root.path2[4].Key) b5.Initialize(root.path2[4].Key, root.path2[4].Key.GetView().targetTexture);
        if ( root.path3[0].Key) c1.Initialize(root.path3[0].Key, root.path3[0].Key.GetView().targetTexture);
        if ( root.path3[1].Key) c2.Initialize(root.path3[1].Key, root.path3[1].Key.GetView().targetTexture);
        if ( root.path3[2].Key) c3.Initialize(root.path3[2].Key, root.path3[2].Key.GetView().targetTexture);
        if ( root.path3[3].Key) c4.Initialize(root.path3[3].Key, root.path3[3].Key.GetView().targetTexture);
        if ( root.path3[4].Key) c5.Initialize(root.path3[4].Key, root.path3[4].Key.GetView().targetTexture);
    }

    public void SetPaths()
	{
        foreach (TowerPartInventorySlot _slot in towerContent.GetComponentsInChildren<TowerPartInventorySlot>()) _slot.GetTowerPart().GetComponent<TowerState>().ClearRoot();
        List<KeyValuePair<TowerState, string>> path1 = MakePathsFromSlots(a1, a2, a3, a4, a5);
        List<KeyValuePair<TowerState, string>> path2 = MakePathsFromSlots(b1, b2, b3, b4, b5);
        List<KeyValuePair<TowerState, string>> path3 = MakePathsFromSlots(c1, c2, c3, c4, c5);
        RemoveNulls(ref path1, ref path2, ref path3);
        List<KeyValuePair<TowerState, string>>[] paths = new List<KeyValuePair<TowerState, string>>[] { path1, path2, path3 };
        AddFilePaths(ref paths);
        root.path1 = paths[0]; root.path2 = paths[1]; root.path3 = paths[2];
        FileManager.SelectSlot(root.inventorySlots[0]);
        FileManager.EditPart();
        ButtonEventManager.ConfirmTower();
    }

    void AddFilePaths(ref List<KeyValuePair<TowerState, string>>[] paths)
	{
        for(int i = 0; i < paths.Length; i++)
		{
            List<KeyValuePair<TowerState, string>> newPath = new List<KeyValuePair<TowerState, string>>();
            foreach (KeyValuePair<TowerState, string> pair in paths[i])
            {
                pair.Key.SetRoot(root, i+1);
                newPath.Add(new KeyValuePair<TowerState, string>(pair.Key, pair.Key.customePartFilePath));
            }
            paths[i] = newPath;
		}
	}

    List<KeyValuePair<TowerState, string>> MakePathsFromSlots(PathBuilderSlot s1, PathBuilderSlot s2, PathBuilderSlot s3, PathBuilderSlot s4, PathBuilderSlot s5 )
	{
        return new List<KeyValuePair<TowerState, string>>()
        {
            new KeyValuePair<TowerState, string>(s1.State, ""),
            new KeyValuePair<TowerState, string>(s2.State, ""),
            new KeyValuePair<TowerState, string>(s3.State, ""),
            new KeyValuePair<TowerState, string>(s4.State, ""),
            new KeyValuePair<TowerState, string>(s5.State, "")
        };
    }

    void RemoveNulls(ref List<KeyValuePair<TowerState, string>> path1, ref List<KeyValuePair<TowerState, string>> path2, ref List<KeyValuePair<TowerState, string>> path3)
	{
        for (int i = 0; i < path1.Count;)
        {
            if (!path1[i].Key) path1.RemoveAt(i);
            else i++;
        }
        for (int i = 0; i < path2.Count;)
        {
            if (!path2[i].Key) path2.RemoveAt(i);
            else i++;
        }
        for (int i = 0; i < path3.Count;)
        {
            if (!path3[i].Key) path3.RemoveAt(i);
            else i++;
        }
    }


    public void ClearSlots() 
    {
        foreach (TowerPartInventorySlot slot in towerContent.GetComponentsInChildren<TowerPartInventorySlot>(true)) Destroy(slot.gameObject);
        foreach (PathBuilderSlot slot in pathBuilderSlots) slot.ResetSlot();
    }
}
