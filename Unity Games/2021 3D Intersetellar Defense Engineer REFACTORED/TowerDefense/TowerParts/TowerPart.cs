using System;
using System.Collections.Generic;
using HobbitUtilz;
using TowerDefense.TowerParts;
using UnityEngine;
using TowerDefense.TowerParts.Weapon;

namespace TowerDefense.TowerParts
{
    /// <summary>
    /// Base class for tower parts that have prefabricated meshes.
    /// </summary>
    public abstract class TowerPart : TowerComponent, ISerializableTowerComponent
    {
        //public List<TC_UI_TowerPartInventorySlot> inventorySlots = new List<TC_UI_TowerPartInventorySlot>();

        [SerializeField] protected PartSize size = PartSize.Medium;
        [SerializeField] protected string _prefabFilePath = "";

        public abstract void SaveToFile();
        public abstract void SetPropertiesFromJSON(Dictionary<string, string> jsonDict);
        public  static TowerComponent LoadTowerPartFromFile(string file)
        {
            StreamReaderPro streamReader = new StreamReaderPro(file);
            Dictionary<string, string> jsonDict = HU_Functions.JSON_To_Dict(streamReader.ToString());
            TowerComponent towerComponent = Instantiate(Resources.Load<TowerComponent>(jsonDict["Prefab Path"]));
            towerComponent.GetComponent<ISerializableTowerComponent>().SetPropertiesFromJSON(jsonDict);
            return towerComponent;
        }
        
        public override void SetShrink(bool shrinkIn) { Shrink = shrinkIn; if (!Shrink && !Hide) { ScaleToSize(); }}
        public override void SetHide(bool hideIn) { Hide = hideIn; if (!Shrink && !Hide) { ScaleToSize(); }}
        public void TurnOffCamera(){if(_view) {_view.gameObject.SetActive(false);}}

        public string GetPrefabFilePath() { return _prefabFilePath; }
        public void SetSize(PartSize sizeIn) { size = sizeIn; }
        public PartSize GetSize() { return size; }

        public void ScaleToSize()
        {
            Shrink = false;
            Hide = false;
            if(GetComponentInParent<TP_WeaponMount>() && GetComponent<TP_WeaponMountStyle>()) transform.localScale = new Vector3(1, 1, 1);
            else
            switch (size)
            {
                case PartSize.Small: gameObject.transform.localScale = GetComponent<TP_Weapon>() ? new Vector3(0.5f, 0.5f, 0.5f) : new Vector3(0.5f, 1, 0.5f); break;
                case PartSize.Medium: gameObject.transform.localScale = new Vector3(1, 1, 1); break;
                case PartSize.Large: gameObject.transform.localScale = GetComponent<TP_Weapon>() ? new Vector3(1.5f, 1.5f, 1.5f) : new Vector3(1.5f, 1, 1.5f); break;
            }
        }

        //public void DestroySlotsThenSelf() { foreach (TC_UI_TowerPartInventorySlot slot in inventorySlots) { if(slot)Destroy(slot.gameObject); } Destroy(gameObject); }
    }

}

