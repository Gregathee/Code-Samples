using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Propagates scrolling behavior of a scroll view when blocked by this UI element.
    /// </summary>
    public class TC_UI_PropagateDrag : MonoBehaviour
    {
        void Start()
        {
            ScrollRect scrollView = GetComponentInParent<ScrollRect>();
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entryScroll = new EventTrigger.Entry { eventID = EventTriggerType.Scroll };

            entryScroll.callback.AddListener((data) => { scrollView.OnScroll((PointerEventData)data); });
            trigger?.triggers?.Add(entryScroll);
        }
    }
}
