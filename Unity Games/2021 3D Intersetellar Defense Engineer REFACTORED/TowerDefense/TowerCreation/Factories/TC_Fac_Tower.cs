using System.Collections;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.UX;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates towers
    /// </summary>
    public class TC_Fac_Tower : MonoBehaviour, TC_Fac_ITowerPartFactory<TComp_TowerState>
    {
        [SerializeField] Camera _towerStateCamera;
        [SerializeField] Transform _towerBasePoint;
        [SerializeField] TC_UI_TowerPartSelector _towerBaseSelector;
        [SerializeField] TC_UI_TowerPartSelector _weaponMountSelector;
        [SerializeField] TC_UI_TowerPartSelector _mountStyleSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] TMP_InputField _rangeIP;
        [SerializeField] TMP_Dropdown _turnRadiusDD;
        [SerializeField] TMP_InputField _constructionTimeIP;

        [SerializeField] float mountHeightAdjustment = 1;

        TP_TowerBase _currentTowerBase;
        TP_WeaponMount _currentWeaponMount;
        TP_WeaponMountStyle _currentWeaponMountStyle;

        void Start()
        {
            _mountStyleSelector.SetMiddleActive(false);
            SelectTowerBase();
            SelectWeaponMount();
            SelectMountStyle();
        }

        public TComp_TowerState CreateTowerPart()
        {
            // TODO set this from input
            string towerName = "";

            TComp_TowerState state = new GameObject().AddComponent<TComp_TowerState>();

            GameObject[] rotatableObjects = new GameObject[] { _currentTowerBase.gameObject, _currentWeaponMount.gameObject };
            state.SetRotatableParts(rotatableObjects);
            
            SetUpTowerBase(towerName);
            SetUpWeaponMount(towerName);
            SetUpTowerState(ref state);
            state.SetIsPreview(true);

            _currentTowerBase = null;
            _currentWeaponMount = null;
            _currentWeaponMountStyle = null;

            return state;
        }
        
        /// <summary>
        /// Select Tower Base Part selectors currently selected part after moving it to the left or right.
        /// </summary>
        /// <param name="left"></param>
        public void SelectTowerBase(bool left)
        {
            SelectTowerComponent
            (
                ref _currentTowerBase, 
                ref _towerBaseSelector, 
                _towerBasePoint.position - new Vector3(0, mountHeightAdjustment, 0), 
                ref left
            );
        }

        /// <summary>
        /// Select Weapon Mount Part selectors currently selected part after moving it to the left or right.
        /// </summary>
        /// <param name="left"></param>
        public void SelectWeaponMount(bool left)
        {
            SelectTowerComponent ( ref _currentWeaponMount, ref _weaponMountSelector, _towerBasePoint.position, ref left );
        }

        /// <summary>
        /// Select Weapon Mount Style Part selectors currently selected part after moving it to the left or right.
        /// </summary>
        /// <param name="left"></param>
        public void SelectMountStyle(bool left)
        {
            _mountStyleSelector.SetMiddleActive(true);
            if(left){ _mountStyleSelector.SelectLeftButton();}
            else {_mountStyleSelector.SelectRightButton();}
            if (_currentWeaponMountStyle != null) { Destroy(_currentWeaponMountStyle.gameObject); }
            _currentWeaponMountStyle = Instantiate(_mountStyleSelector.GetCurrentPart().GetComponent<TP_WeaponMountStyle>());
            _currentWeaponMountStyle.transform.localScale = Vector3.one;
            _currentWeaponMountStyle.SetIsPreview(false);
            _currentWeaponMountStyle.CorrectRotation();
            _currentWeaponMountStyle.transform.position = _towerBasePoint.position;
            _currentWeaponMountStyle.gameObject.SetActive(false);
            StartCoroutine(DelayMountStyleActivation());
        }

        /// <summary>
        /// Select part selectors currently selected part after moving it to the left or right.
        /// </summary>
        /// <param name="left"></param>
        static void SelectTowerComponent<T>(ref T towerPart, ref TC_UI_TowerPartSelector selector, Vector3 point, ref bool left) where T : TowerPart
        {
            if(left){ selector.SelectLeftButton();}
            else {selector.SelectRightButton();}
            if (towerPart != null) { Destroy(towerPart.gameObject); }
            towerPart = Instantiate(selector.GetCurrentPart().GetComponent<T>());
            towerPart.transform.localScale = Vector3.one;
            towerPart.SetIsPreview(false);
            towerPart.CorrectRotation();
            towerPart.transform.position = point;
        }
        
        /// <summary>
        /// Select Part selectors currently selected part.
        /// </summary>
        static void SelectTowerComponent<T>(ref T towerPart, ref TC_UI_TowerPartSelector selector, Vector3 point) where T : TowerPart
        {
            if (towerPart != null) { Destroy(towerPart.gameObject); }
            towerPart = Instantiate(selector.GetCurrentPart().GetComponent<T>());
            towerPart.transform.localScale = Vector3.one;
            towerPart.SetIsPreview(false);
            towerPart.CorrectRotation();
            towerPart.transform.position = point;
        }

        void SetUpTowerState(ref TComp_TowerState state)
        {
            Camera stateView = Instantiate(_towerStateCamera.gameObject).GetComponent<Camera>();
            stateView.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);

            stateView.transform.SetParent(state.transform);
            stateView.transform.localPosition = new Vector3(0, 13, -20);
            stateView.orthographicSize = 10;
            
            state.StartingConstructionTime = int.Parse(_constructionTimeIP.text);
            
            state.SetBase(_currentTowerBase);
            state.SetMount(_currentWeaponMount);
            state.SetView(stateView);
        }

        void SetUpTowerBase(string towerName)
        {
            _currentTowerBase.name = towerName + "Tower Base";
            _currentTowerBase.SetSize((PartSize)_sizeDD.value);
            
            _currentTowerBase.transform.localPosition = Vector3.zero;
            _currentTowerBase.SaveToFile();
        }

        void SetUpWeaponMount(string towerName)
        {
            _currentWeaponMount.name = towerName + "Weapon Mount";
            _currentWeaponMount.SetSize((PartSize)_sizeDD.value);
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_WeaponMount.ROTATION_SPEED_BOUNDS);
            _currentWeaponMount.RotationalSpeed = int.Parse(_rotationSpeedIP.text);
            HU_Functions.SanitizeIntIP(ref _rangeIP, TP_WeaponMount.RANGE_BOUNDS);
            _currentWeaponMount.Range = int.Parse(_rangeIP.text);
            _currentWeaponMount.TurretAngle = (TurretAngle)_turnRadiusDD.value;
            _currentWeaponMount.SetStyle(_currentWeaponMountStyle);

            _currentWeaponMountStyle.name = towerName + "Weapon Mount Style";
            _currentWeaponMountStyle.transform.SetParent(_currentWeaponMount.transform);
            _currentWeaponMountStyle.SaveToFile();
            
            _currentWeaponMount.SaveToFile();
        }
        
        /// <summary>
        /// Select Tower Base Part selectors currently selected part.
        /// </summary>
        void SelectTowerBase()
        {
            SelectTowerComponent
            (
                ref _currentTowerBase, 
                ref _towerBaseSelector, 
                _towerBasePoint.position - new Vector3(0, mountHeightAdjustment, 0)
            );
        }
        
        /// <summary>
        /// Select Weapon Mount Part selectors currently selected part.
        /// </summary>
        void SelectWeaponMount()
        {
            SelectTowerComponent ( ref _currentWeaponMount, ref _weaponMountSelector, _towerBasePoint.position );
        }
        
        /// <summary>
        /// Select Weapon Mount Style Part selectors currently selected part.
        /// </summary>
        void SelectMountStyle()
        {
            SelectTowerComponent ( ref _currentWeaponMountStyle, ref _mountStyleSelector, _towerBasePoint.position );
            _currentWeaponMountStyle.gameObject.SetActive(true);
        }

        /// <summary>
        /// Seamlessly transition from weapon mount style part selector to _currentWeaponMountStyle in the tower editor.
        /// </summary>
        /// <returns></returns>
        IEnumerator DelayMountStyleActivation()
        {
            yield return new WaitUntil(_mountStyleSelector.PartsNotInMotion);
            _currentWeaponMountStyle.gameObject.SetActive(true);
            _mountStyleSelector.SetMiddleActive(false);
        }
    }
}
