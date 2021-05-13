using System.Collections;
using HobbitUtilz;
using UnityEngine;
using TowerDefense.TowerParts;

namespace TowerDefense.TowerCreation.UI
{
    /// <summary>
    /// Used to create a 3D GUI to select tower parts.
    /// </summary>
    public class TC_UI_TowerPartSelector : MonoBehaviour
    {
        [SerializeField] TowerPart[] towerPartPrefabs;
        
        [SerializeField] float _deltaX = 5;
        [SerializeField] float _moveSpeed = 1;
        [SerializeField] float _shrinkSpeed = 1;
        [SerializeField] float _delayBeforeGrow;
        
        Vector3[] _positions;
        GameObject[] _towerObjects;
        HU_ArrayIterator<GameObject> _partIterator;
        HU_ArrayIterator<GameObject> _tempPartIterator;

        bool _exit;
        bool _partsInMotion;
        bool _isHidden;
        bool _isShrinking;

        public bool IsHidden { get => _isHidden; }

        void Start()
        {
            SetPositions();

            // Load towerObjects with instantiated prefabs
            _towerObjects = new GameObject[towerPartPrefabs.Length];
            for (int index = 0; index < towerPartPrefabs.Length; ++index)
            {
                GameObject tPartObject = Instantiate(towerPartPrefabs[index], transform).gameObject;
                TowerPart tPart = tPartObject.GetComponent<TowerPart>();
                tPart.TurnOffCamera();
                _towerObjects[index] = tPartObject;
                tPart.SetShrink(true);
                tPart.transform.localScale = Vector3.zero;
                tPart.SetSize(PartSize.Medium);
            }
            // Position parts in GUI
            if (_towerObjects.Length > 0)
            {
                int i;
                for (i = 0; i < _positions.Length; i++) { _towerObjects[i].transform.localPosition = _positions[i]; }
                for (; i < _towerObjects.Length; i++) { _towerObjects[i].transform.localPosition = _positions[0]; }
            }
            else Debug.Log("[TC_UI_TowerPartSelector] TowerParts is empty");

            
            _partIterator = new HU_ArrayIterator<GameObject>(_towerObjects) { Index = 2 };
            _tempPartIterator = new HU_ArrayIterator<GameObject>(_towerObjects) { Index = 2 };
        }

        public bool PartsNotInMotion() => !_partsInMotion;

        /// <summary>
        /// Select a given TowerPart and center it on the 3D GUI
        /// </summary>
        /// <param name="partToJumpTo"></param>
        public void JumpToPart(TowerPart partToJumpTo)
        {
            int i = 0;
            GameObject part = _towerObjects[i];
            while (part.GetComponent<TowerPart>().GetPrefabFilePath() != partToJumpTo.GetPrefabFilePath() && i < towerPartPrefabs.Length - 1)
            {
                if (part.GetComponent<TowerPart>().GetPrefabFilePath() != partToJumpTo.GetPrefabFilePath()) i++;
                part = _towerObjects[i];
            }
            foreach (GameObject towerPart in _towerObjects) { towerPart.transform.position = _positions[0]; }
            _towerObjects[i].transform.position = _positions[2];
            _partIterator.Index = i;
            _towerObjects[_partIterator.AdjustedIndex(-1)].transform.position = _positions[1];
            _towerObjects[_partIterator.AdjustedIndex(1)].transform.position = _positions[3];
            _towerObjects[_partIterator.AdjustedIndex(2)].transform.position = _positions[4];
            ColoredTowerPart coloredTowerPart = _towerObjects[_partIterator].GetComponent<ColoredTowerPart>();
            if (coloredTowerPart) { coloredTowerPart.SetMaterials(coloredTowerPart.Mat1, coloredTowerPart.Mat2, coloredTowerPart.Mat3); }
        }
        
        public void SelectRightButton() { MoveParts(true); }
        public void SelectLeftButton() { MoveParts(false); }

        /// <summary>
        /// Returns tower part that is positioned at the middle position and is currently selected.
        /// </summary>
        /// <returns></returns>
        public TowerPart GetCurrentPart() { return _towerObjects[_partIterator].GetComponent<TowerPart>(); }

        /// <summary>
        /// Sets the shrink property of all tower parts associated with this TowerPartSelector.
        /// </summary>
        /// <param name="shrink"></param>
        public void SetPartsShrink(bool shrink) { foreach (GameObject gameOb in _towerObjects) {gameOb.GetComponent<TowerPart>().SetShrink(shrink);} }

        /// <summary>
        /// Sets size of all tower parts associated with this TowerPartSelector.
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(PartSize size)
        {
            SetPartsShrink(false);
            foreach (GameObject gameOb in _towerObjects) { gameOb.GetComponent<TowerPart>().SetSize(size); }
        }

        /// <summary>
        /// Hides all tower parts associated with this TowerPartSelector with option of not playing shrink animation.
        /// </summary>
        /// <param name="hideSelectionInstantly"></param>
        public void Hide(bool hideSelectionInstantly)
        {
            _isHidden = true;
            if (hideSelectionInstantly) _towerObjects[_partIterator].transform.localScale = Vector3.zero;
            HideParts();
        }

        /// <summary>
        /// Un-Hides all tower parts associated with this TowerPartSelector with option of not playing grow animation.
        /// </summary>
        /// <param name="scaleSelectionToSizeInstantly"></param>
        public void UnHide(bool scaleSelectionToSizeInstantly)
        {
            _isHidden = false;
            if (scaleSelectionToSizeInstantly) { _towerObjects[_partIterator].GetComponent<TowerPart>().ScaleToSize(); }
            HideParts();
        }
        
        public void SetMiddleActive(bool hide){_towerObjects[_partIterator].SetActive(hide);}
        
        void SetPositions()
        {
            Vector3 pos0 = Vector3.zero;
            pos0.x -= _deltaX * 2;
            Vector3 pos1 = Vector3.zero;
            pos1.x -= _deltaX;
            Vector3 pos3 = Vector3.zero;
            pos3.x += _deltaX;
            Vector3 pos4 = Vector3.zero;
            pos4.x += _deltaX * 2;

            _positions = new Vector3[] { pos0, pos1, Vector3.zero, pos3, pos4 };
        }

        void HideParts()
        {
            if (_towerObjects.Length == 0) return;
            float maxGrowthSize = 1;
            PartSize size = _towerObjects[0].GetComponent<TowerPart>().GetSize();
            switch (size)
            {
                case PartSize.Small: maxGrowthSize = 0.5f; break;
                case PartSize.Medium: maxGrowthSize = 1; break;
                case PartSize.Large: maxGrowthSize = 1.5f; break;
            }
            SetPartsShrink(true);
            if (!_isShrinking) StartCoroutine(HidePartsRoutine(maxGrowthSize));
        }

        void _postHidePartsRoutine()
        {
            _isShrinking = false;
            foreach (GameObject part in _towerObjects)
            {
                part.GetComponent<TowerPart>().SetHide(false);
                if (_isHidden) part.transform.localScale = Vector3.zero;
                else { part.GetComponent<TowerPart>().ScaleToSize(); }
            }
        }

        /// <summary>
        /// Move Parts in GUI one position to left or right.
        /// </summary>
        /// <param name="left"></param>
        void MoveParts(bool left)
        {
            _tempPartIterator.Index = _partIterator.Index;
            if (_partsInMotion) return;
            int i1, i2, i3, i4, p1, p2, p3, p4;
            int xSpeedFactor = 1;

            if (left)
            {
                _partIterator.Advance();
                i1 = _tempPartIterator.AdjustedIndex(-1);
                i2 = _tempPartIterator;
                i3 = _tempPartIterator.AdjustedIndex(1);
                i4 = _tempPartIterator.AdjustedIndex(2);
                p1 = 0; p2 = 1; p3 = 2; p4 = 3;
                _towerObjects[i4].transform.localPosition = _positions[4];
                xSpeedFactor *= -1;
            }
            else
            {
                _partIterator.Advance(-1);
                i1 = _tempPartIterator.AdjustedIndex(1);
                i2 = _tempPartIterator;
                i3 = _tempPartIterator.AdjustedIndex(-1);
                i4 = _tempPartIterator.AdjustedIndex(-2);
                p1 = 4; p2 = 3; p3 = 2; p4 = 1;
                _towerObjects[i4].transform.localPosition = _positions[0];
            }
            StartCoroutine(MovePartsRoutine(i1, i2, i3, i4, p1, p2, p3, p4, xSpeedFactor));
        }

        IEnumerator MovePartsRoutine(int i1, int i2, int i3, int i4, int p1, int p2, int p3, int p4, int xSpeedFactor)
        {
            Vector3 pos1 = _towerObjects[i1].transform.localPosition;
            Vector3 pos2 = _towerObjects[i2].transform.localPosition;
            Vector3 pos3 = _towerObjects[i3].transform.localPosition;
            Vector3 pos4 = _towerObjects[i4].transform.localPosition;

            _partsInMotion = true;
            while (_towerObjects[_tempPartIterator].transform.localPosition.x > -(_deltaX - 1) && _towerObjects[_tempPartIterator].transform.localPosition.x < _deltaX - 1)
            {
                float xSpeed = xSpeedFactor * _moveSpeed * Time.deltaTime;
                pos1.x += xSpeed; pos2.x += xSpeed; pos3.x += xSpeed; pos4.x += xSpeed;
                _towerObjects[i1].transform.localPosition = pos1;
                _towerObjects[i2].transform.localPosition = pos2;
                _towerObjects[i3].transform.localPosition = pos3;
                _towerObjects[i4].transform.localPosition = pos4;
                yield return null;
            }
            _partsInMotion = false;

            _towerObjects[i1].transform.localPosition = _positions[p1];
            _towerObjects[i2].transform.localPosition = _positions[p2];
            _towerObjects[i3].transform.localPosition = _positions[p3];
            _towerObjects[i4].transform.localPosition = _positions[p4];
            _tempPartIterator.Index = i3;
        }
        
        IEnumerator HidePartsRoutine(float maxGrowthSize)
        {
            // ReSharper disable once TooWideLocalVariableScope (Declared here to generate less garbage)
            float speedFactor;
            _isShrinking = true;
            foreach (GameObject part in _towerObjects) { part.GetComponent<TowerPart>().SetHide(true); }
            while (_towerObjects[0].transform.localScale.x >= 0 && _towerObjects[0].transform.localScale.x <= maxGrowthSize)
            {
                foreach (GameObject part in _towerObjects)
                {
                    speedFactor = _isHidden ? -1 : 1;
                    float speed = _shrinkSpeed * speedFactor * Time.deltaTime;
                    part.transform.localScale += new Vector3(speed, speed, speed);
                }
                yield return null;
            }
            _postHidePartsRoutine();
        }
    }
}