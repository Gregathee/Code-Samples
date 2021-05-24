using TMPro;
using UnityEngine;
using HobbitUtilz;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Adjust the camera position and UI within the tower creation menu.
    /// </summary>
    public class TC_UI_SectionNavigator : MonoBehaviour
    {
        [SerializeField] TC_UI_Section[] _sections;
        [SerializeField] TMP_Text _nextSectionButtonText;
        [SerializeField] TMP_Text _previousSectionButtonText;
        [SerializeField] TMP_Text _sectionTitleText;
        [SerializeField] bool _setSceneAfterSectionChange;
        [SerializeField] bool _buttonTextChanges = true;

        HU_ArrayIterator<TC_UI_Section> _sectionIterator;

        void Start() { Reset(); }

        public void GoToNextSection()
        {
            if (_sections.Length == 0) { Debug.Log("[TC_UI_Inv_Navigator] No sections are assigned to navigate."); return; }
            
            if(_buttonTextChanges) {_previousSectionButtonText.text = _sections[_sectionIterator].GetTitle();}
            
            _sections[_sectionIterator].ActivateVCam(false);
            _sections[_sectionIterator].gameObject.SetActive(false);
            
            _sectionTitleText.text = _sectionIterator.Advance().GetTitle();
            
            _sections[_sectionIterator].ActivateVCam(true);
            _sections[_sectionIterator].gameObject.SetActive(true);
            
            if(_buttonTextChanges) {_nextSectionButtonText.text = _sectionIterator.PeakNext().GetTitle();}
            if(_setSceneAfterSectionChange) {SetScene();}
        }

        public void GoToPreviousSection()
        {
            if (_sections.Length == 0) { Debug.Log("[TC_UI_Inv_Navigator] No sections are assigned to navigate."); return; }
            
            if(_buttonTextChanges){_nextSectionButtonText.text = _sections[_sectionIterator].GetTitle();}
            
            _sections[_sectionIterator].ActivateVCam(false);
            _sections[_sectionIterator].gameObject.SetActive(false);
            
            _sectionTitleText.text = _sectionIterator.Advance(-1).GetTitle();
            
            _sections[_sectionIterator].ActivateVCam(true);
            _sections[_sectionIterator].gameObject.SetActive(true);
            
            if(_buttonTextChanges) {_previousSectionButtonText.text = _sectionIterator.PeakPrevious().GetTitle();}
            if(_setSceneAfterSectionChange) {SetScene();}
        }

        /// <summary>
        /// Invoke all behaviors used to set scene of current section.
        /// </summary>
        public void SetScene()
        {
            if (_sections.Length == 0) { Debug.Log("[TC_UI_Inv_Navigator] No sections are assigned to navigate."); return; }
            _sections[_sectionIterator].Invoke();
        }

        /// <summary>
        /// Reset section index to default position
        /// </summary>
        public void Reset(int index = 0)
        {
            if (_sectionIterator == null)
            {
                if (_sections.Length == 0) { Debug.Log("[TC_UI_Inv_Navigator] No sections are assigned to navigate."); return; }
                _sectionIterator = new HU_ArrayIterator<TC_UI_Section>(_sections);
            }
            foreach (TC_UI_Section section in _sections)
            {
                section.gameObject.SetActive(false);
                section.ActivateVCam(false);
            }
            _sectionIterator.Index = index;
            _sections[_sectionIterator].ActivateVCam(true);
            _sections[_sectionIterator].gameObject.SetActive(true);
            _sectionTitleText.text = _sections[_sectionIterator].GetTitle();
            
            if (!_buttonTextChanges) return;
            _nextSectionButtonText.text = _sectionIterator.PeakNext().GetTitle();
            _previousSectionButtonText.text = _sectionIterator.PeakPrevious().GetTitle();
        }
    }
}
