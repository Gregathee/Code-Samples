using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI.Inventory
{
    /// <summary>
    /// Sets the content object of a scroll rect. Used to have multiple content displays under a single scroll view.
    /// </summary>
    public class TC_UI_TP_InventoryContentSetter : MonoBehaviour
    {
        [SerializeField] ScrollRect _scrollRect;

        void OnEnable() { _scrollRect.content = GetComponent<RectTransform>(); }
    }
}
