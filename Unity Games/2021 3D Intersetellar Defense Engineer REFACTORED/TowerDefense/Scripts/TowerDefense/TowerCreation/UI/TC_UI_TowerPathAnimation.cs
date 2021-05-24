using System;
using UnityEngine;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Plays Animation on enable
    /// </summary>
    public class TC_UI_TowerPathAnimation : MonoBehaviour
    {
        [SerializeField] RectTransform panel;
        Vector2 position;
        
        void Awake()
        {
            position = panel.anchoredPosition;
        }
        void OnEnable()
        {
            GetComponent<Animator>().SetBool("OnEnable", true);
            GetComponent<Animator>().Play("Tower Path Enter");
        }

        void Disable()
        {
            GetComponent<Animator>().SetBool("OnEnable", false);
            panel.anchoredPosition = position;
        }

        void OnDisable() { Disable(); }
    }
}
