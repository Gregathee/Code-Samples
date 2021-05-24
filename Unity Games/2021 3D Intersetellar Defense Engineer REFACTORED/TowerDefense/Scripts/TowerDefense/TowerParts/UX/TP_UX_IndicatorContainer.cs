using UnityEngine;

namespace TowerDefense.TowerParts.UX
{
    /// <summary>
    /// Holds all UX indicators for a tower.
    /// </summary>
    public class TP_UX_IndicatorContainer : MonoBehaviour
    {
        [Tooltip("Empty object representing what turret aims at when no targets are in range.")]
        [SerializeField] GameObject _home;
        [SerializeField] GameObject _rangeIndicator;
        [SerializeField] TP_UX_AngleIndicator tpUxAngleIndicator;
        [SerializeField] TP_UX_TargetingBeam _beam;
        public GameObject GetHome() { return _home; }
        public GameObject GetRangeIndicator() { return _rangeIndicator; }
        public TP_UX_AngleIndicator GetAngleIndicator() { return tpUxAngleIndicator; }
        public TP_UX_TargetingBeam GetTargetingBeam() { return _beam; }

        public void HideIndicators()
        {
            _rangeIndicator.SetActive(false);
            tpUxAngleIndicator.gameObject.SetActive(false);
            _beam.gameObject.SetActive(false);
        }
    }
}