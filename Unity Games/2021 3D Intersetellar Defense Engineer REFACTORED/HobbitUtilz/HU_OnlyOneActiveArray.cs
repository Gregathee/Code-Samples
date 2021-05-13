using UnityEngine;

namespace HobbitUtilz
{
    /// <summary>
    /// Contains an array of items and only allows one to be active. 
    /// </summary>
    public class HU_OnlyOneActiveArray : MonoBehaviour
    {
        [SerializeField] GameObject[] _gameObjects;
        
        /// <summary>
        /// Activates a GameObject, given it exists in the array, and deactivates all other GameObjects in the array.
        /// </summary>
        /// <param name="selectedGameObject"></param>
        public void SelectGameObject(GameObject selectedGameObject)
        {
            foreach(GameObject gameOb in _gameObjects) {gameOb.SetActive(gameOb == selectedGameObject);}
        }
    }
}
