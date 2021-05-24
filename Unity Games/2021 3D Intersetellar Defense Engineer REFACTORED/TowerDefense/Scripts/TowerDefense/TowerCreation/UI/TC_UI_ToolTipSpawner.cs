using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Spawns a tooltip on mouse over
    /// </summary>
    public class TC_UI_ToolTipSpawner : MonoBehaviour
    {
        [SerializeField] string _toolTipText;
        [SerializeField] TC_UI_FloatingToolTip _toolTipPrefab;
        Transform _imageParent;
        
        TC_UI_FloatingToolTip _toolTipInstance;

        void Awake()
        {
            _imageParent = GetComponentInParent<Canvas>().transform;
            AddEventTrigger((data) => { MouseEnter(); }, EventTriggerType.PointerEnter);
            AddEventTrigger((data) => { MouseExit(); }, EventTriggerType.PointerExit);
        }

        public void MouseEnter()
        {
            _toolTipInstance = Instantiate(_toolTipPrefab, _imageParent);
            _toolTipInstance.Initialize(_toolTipText);
        }

        public void MouseExit()
        {
            if(_toolTipInstance){Destroy(_toolTipInstance.gameObject);}
            _toolTipInstance = null;
        }
        
        
        
        void AddEventTrigger(UnityAction<BaseEventData> call, EventTriggerType triggerType)
        {
            EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry eventEntry = new EventTrigger.Entry();
            eventEntry.eventID = triggerType;
            eventEntry.callback.AddListener(call);
            eventTrigger.triggers.Add(eventEntry);
        }
    }
}
