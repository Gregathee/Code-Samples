using System;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using UnityEngine;

namespace TowerDefense.TowerCreation
{
    /// <summary>
    /// Object that is responsible for positioning 3D Models to prevent camera overlaps for 3DUI
    /// </summary>
    public class TC_ModelManager : MonoBehaviour
    {
        [SerializeField] int _yDistance = 100;
        int count = 0;
        
        // Adds a tower component model as a child and positions it to prevent overlapping cameras. 
        public void PlaceModel(GameObject model)
        {
            model.transform.SetParent(transform);
            Vector3 pos = transform.position;
            pos.y += 100 + (count * _yDistance);
            model.transform.position = pos;
            count++;
        }
        
        /// <summary>
        /// Activates all models of a given type and deactivates all others to save performance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void ActivateType<T>() where T : TowerComponent
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.SetActive(child.GetComponent<T>());
            }
        }

        public void SyncRotations()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).transform.localRotation = new Quaternion();
            }
        }

        /// <summary>
        /// Returns true if a tower part is using a given name.
        /// </summary>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public bool NameInUse(string inputName)
        {
            foreach (TowerComponent part in transform.GetComponentsInChildren<TowerComponent>(true))
            {
                if (part.name == inputName) return true;
            }
            return false;
        }
    }
}
