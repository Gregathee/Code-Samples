using System;
using System.Collections;
using HobbitUtilz;
using TMPro;
using TowerDefense.TowerCreation.UI;
using TowerDefense.TowerCreation.UI.Inventory;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Weapon;
using UnityEngine;

namespace TowerDefense.TowerCreation.Factories
{
    /// <summary>
    /// Factory class that creates towers
    /// </summary>
    public class TC_Fac_Tower : TC_Fac_TowerPartFactory
    {
        [SerializeField] TC_Fac_TowerPart _mainFactory;
        [SerializeField] Camera _towerStateCamera;
        [SerializeField] Transform _towerBasePoint;
        [SerializeField] TC_UI_TowerPartSelector _towerBaseSelector;
        [SerializeField] TC_UI_TowerPartSelector _weaponMountSelector;
        [SerializeField] TC_UI_TowerPartSelector _mountStyleSelector;
        [SerializeField] TMP_Dropdown _sizeDD;
        [SerializeField] TMP_InputField _rotationSpeedIP;
        [SerializeField] GameObject _rotationSpeedObject;
        [SerializeField] TMP_InputField _rangeIP;
        [SerializeField] TMP_Dropdown _turnRadiusDD;
        [SerializeField] TMP_InputField _constructionTimeIP;

        [SerializeField] float mountHeightAdjustment = 1;

        TP_TowerBase _currentTowerBase;
        TP_WeaponMount _currentWeaponMount;
        TP_WeaponMountStyle _currentWeaponMountStyle;
        
        void Awake() { TComp_TowerState.Factory = this; }

        void Start() { _mountStyleSelector.SetMiddleActive(false); }

        void Update()
        {
            SanitizeInput();
            
            // Temporary solution sync colors of selector parts and the tower model.
            Material baseMat1 = _towerBaseSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat1;
            Material baseMat2 = _towerBaseSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat2;
            Material baseMat3 = _towerBaseSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat3;
            _currentTowerBase.SetMaterials(baseMat1, baseMat2, baseMat3);
            
            Material mountMat1 = _weaponMountSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat1;
            Material mountMat2 = _weaponMountSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat2;
            Material mountMat3 = _weaponMountSelector.GetCurrentPart().GetComponent<ColoredTowerPart>().Mat3;
            _currentWeaponMount.SetMaterials(mountMat1, mountMat2, mountMat3);
        }

        /// <summary>
        /// Constructs and returns a Spray ammo.
        /// </summary>
        /// <param name="partName"></param>
        /// <returns></returns>
        public override TowerComponent CreateTowerPart(string partName)
        {
            TComp_TowerState state = new GameObject().AddComponent<TComp_TowerState>();
            state.name = partName;

            GameObject[] rotatableObjects = new GameObject[] { _currentTowerBase.gameObject, _currentWeaponMount.gameObject };
            state.SetRotatableParts(rotatableObjects);
            
            SetUpTowerBase();
            SetUpWeaponMount();
            SetUpTowerState(ref state);
            state.SetIsPreview(true);

            _currentTowerBase = null;
            _currentWeaponMount = null;
            _currentWeaponMountStyle = null;

            return state;
        }
        
        /// <summary>
        /// Deconstructs a tower and displays its properties in the tower editor.
        /// </summary>
        /// <param name="part"></param>
        public override void DisplayPartProperties(TowerComponent part)
        {
            TComp_TowerState state = part.GetComponent<TComp_TowerState>();
            
            _currentTowerBase = Instantiate(state.GetBase());
            _towerBaseSelector.JumpToPart(_currentTowerBase);
            _currentTowerBase.transform.position = _towerBasePoint.position - new Vector3(0, mountHeightAdjustment, 0);
            
            _currentWeaponMount = Instantiate(state.GetMount());
            _weaponMountSelector.JumpToPart(_currentWeaponMount);
            _currentWeaponMount.transform.position = _towerBasePoint.position;

            _currentWeaponMountStyle = _currentWeaponMount.GetStyle();
            _currentWeaponMountStyle.transform.SetParent(null);
            _currentWeaponMountStyle.SetIsPreview(true);
            _mountStyleSelector.JumpToPart(_currentWeaponMountStyle);
            _currentWeaponMountStyle.transform.position = _towerBasePoint.position;

            _sizeDD.value = (int)_currentTowerBase.GetSize();
            ChangeSize();
            _rotationSpeedIP.text = _currentWeaponMount.RotationalSpeed.ToString();
            _rangeIP.text = _currentWeaponMount.Range.ToString();
            _turnRadiusDD.value = (int)_currentWeaponMount.TurretAngle;
            
            _constructionTimeIP.text = state.StartingConstructionTime.ToString();
        }
        
        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public override ColoredTowerPart GetColoredTowerPart1()
        {
            return _towerBaseSelector.GetCurrentPart().GetComponent<ColoredTowerPart>();
        }
        
        /// <summary>
        /// Returns the current tower component model to have its colors modified. 
        /// </summary>
        /// <returns></returns>
        public override ColoredTowerPart GetColoredTowerPart2()
        {
            return _weaponMountSelector.GetCurrentPart().GetComponent<ColoredTowerPart>();
        }
        
        public override bool ErrorsPresent()
        {
            foreach (WeaponMountSlot slot in _currentWeaponMountStyle.GetSlots())
            {
                if (!slot.GetWeapon())
                {
                    const string missingWeaponMessage = "All weapon slots must have a weapon.";
                    TC_UI_ConfirmationManager.Instance.PromptMessage(missingWeaponMessage, false, false);
                    return true;
                }
                if (!slot.GetWeapon().IsTouchingWeapon()) continue;
                const string touchingMessage = "Weapons must not be touching.";
                TC_UI_ConfirmationManager.Instance.PromptMessage(touchingMessage, false, false);
                return true;
            }
            return false;
        }
        
        public void ChangeSize()
        {
            _currentTowerBase.SetSize((PartSize)_sizeDD.value, true);
            _currentWeaponMount.SetSize((PartSize)_sizeDD.value, true);
            _currentWeaponMountStyle.SetSize((PartSize)_sizeDD.value, true);
            
            _towerBaseSelector.SetSize((PartSize)_sizeDD.value, true);
            _weaponMountSelector.SetSize((PartSize)_sizeDD.value, true);
            _mountStyleSelector.SetSize((PartSize)_sizeDD.value, true);
        }

        // Locks and unlocks the rotation of the tower model for weapon placement.
        public void ChangeRotation(bool start)
        {
            _currentTowerBase.SetIsPreview(start);
            _currentWeaponMount.SetIsPreview(start);
            _currentWeaponMountStyle.SetIsPreview(start);
            if (start) return;
            _currentTowerBase.CorrectRotation();
            _currentWeaponMount.CorrectRotation();
            _currentWeaponMountStyle.CorrectRotation();
            _currentTowerBase.transform.rotation = new Quaternion();
            _currentWeaponMount.transform.rotation = new Quaternion();
            _currentWeaponMountStyle.transform.rotation = new Quaternion();
        }

        public void SetupModels()
        {
            if (_mainFactory.IsEditing()) { return;}
            SelectTowerBase();
            SelectWeaponMount();
            SelectMountStyle();
            ChangeRotation(true);
        }

        public void ClearModels()
        {
            if(_currentTowerBase){ Destroy(_currentTowerBase.gameObject);}
            if(_currentWeaponMount){ Destroy(_currentWeaponMount.gameObject);}
            if(_currentWeaponMountStyle){ Destroy(_currentWeaponMountStyle.gameObject);}
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
            _currentWeaponMountStyle.SetIsPreview(true);
            _currentWeaponMountStyle.CorrectRotation();
            _currentWeaponMountStyle.transform.position = _towerBasePoint.position;
            _currentWeaponMountStyle.gameObject.SetActive(false);
            StartCoroutine(DelayMountStyleActivation());
        }

        /// <summary>
        /// Select part selectors currently selected part after moving it to the left or right.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="left"></param>
        /// <param name="towerPart"></param>
        /// <param name="selector"></param>
        static void SelectTowerComponent<T>(ref T towerPart, ref TC_UI_TowerPartSelector selector, Vector3 point, ref bool left) where T : TowerPart
        {
            if(left){ selector.SelectLeftButton();}
            else {selector.SelectRightButton();}
            SelectTowerComponent<T>(ref towerPart, ref selector, point);
        }
        
        /// <summary>
        /// Select Part selectors currently selected part.
        /// </summary>
        static void SelectTowerComponent<T>(ref T towerPart, ref TC_UI_TowerPartSelector selector, Vector3 point) where T : TowerPart
        {
            if (towerPart != null) { Destroy(towerPart.gameObject); }
            towerPart = Instantiate(selector.GetCurrentPart().GetComponent<T>());
            towerPart.transform.localScale = Vector3.one;
            towerPart.SetIsPreview(true);
            towerPart.CorrectRotation();
            towerPart.transform.position = point;
        }

        /// <summary>
        /// Clamps part properties within their given bounds and prevents bad input.
        /// </summary>
        void SanitizeInput()
        {
            HU_Functions.SanitizeIntIP(ref _rotationSpeedIP, TP_WeaponMount.ROTATION_SPEED_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _rangeIP, TP_WeaponMount.RANGE_BOUNDS);
            HU_Functions.SanitizeIntIP(ref _constructionTimeIP, TComp_TowerState.CONSTRUCTION_TIME_BOUNDS);
            
            _rotationSpeedObject.SetActive(_turnRadiusDD.value < _turnRadiusDD.options.Count-1);
        }

        void SetUpTowerState(ref TComp_TowerState state)
        {
            Camera stateView = Instantiate(_towerStateCamera);
            stateView.targetTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            stateView.transform.SetParent(state.transform);
            
            state.StartingConstructionTime = int.Parse(_constructionTimeIP.text);
            
            state.SetBase(_currentTowerBase);
            state.SetMount(_currentWeaponMount);
            state.SetView(stateView);
        }

        void SetUpTowerBase()
        {
            _currentTowerBase.SetSize((PartSize)_sizeDD.value);
            _currentTowerBase.transform.localPosition = Vector3.zero;
        }

        void SetUpWeaponMount()
        {
            _currentWeaponMount.SetSize((PartSize)_sizeDD.value);
            _currentWeaponMount.RotationalSpeed = int.Parse(_rotationSpeedIP.text);
            _currentWeaponMount.Range = int.Parse(_rangeIP.text);
            _currentWeaponMount.TurretAngle = (TurretAngle)_turnRadiusDD.value;
            _currentWeaponMount.SetStyle(_currentWeaponMountStyle);
            _currentWeaponMountStyle.transform.SetParent(_currentWeaponMount.transform);
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
