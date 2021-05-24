using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Keeps handle of a scroll bar a consistent size;
    /// </summary>
    public class TC_UI_ScrollBarFixedSize : MonoBehaviour
    {
        [SerializeField] Scrollbar _scrollbar;
        [SerializeField] float _size = 0.1f;

        void Update() { _scrollbar.size = _size; }

        void OnEnable(){ _scrollbar.size = _size; }
    }
}
