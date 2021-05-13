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
        public void PlaceModel(GameObject model)
        {
            model.transform.SetParent(transform);
            Vector3 pos = transform.position;
            pos.y += 100 + (count * _yDistance);
            model.transform.position = pos;
            count++;
        }
    }
}
