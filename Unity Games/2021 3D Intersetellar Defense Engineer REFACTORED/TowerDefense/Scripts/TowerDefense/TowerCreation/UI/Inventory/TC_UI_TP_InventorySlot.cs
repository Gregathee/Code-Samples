using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using UnityEngine.Events;

namespace TowerDefense.TowerCreation.UI.Inventory
{
	public struct TC_UI_TP_InventorySlotProperties
	{
		public TowerComponent TowerComponent;
		public TC_UI_SlotDetector SlotDetector;
		public float DistanceFromCamera;
		public Color SelectedColor;
		public Color NormalColor;
		public Sprite ButtonSprite;
		public TC_UI_FloatingToolTip ToolTip;
	}
	/// <summary>
	/// UI element depicting a created tower part with some inventory management functionality.
	/// </summary>
	public class TC_UI_TP_InventorySlot : MonoBehaviour, IComparable<TC_UI_TP_InventorySlot>
	{
		/// <summary>
		/// Used to communicate with other scripts to invoke behavior involving drag and dropping tower parts in the creation interface.
		/// </summary>
		public static TC_UI_TP_InventorySlot SlotFollowingCursor;
		public static List<TComp_TowerState> States = new List<TComp_TowerState>();
		public static bool DragImage = true;
		
		public bool IsFollowingCursor;
		
		TowerComponent _towerComponent;
		TC_UI_SlotDetector _detector;
		float _distanceFromCamera = 15;
		TC_UI_FloatingToolTip _toolTipPrefab;
		TC_UI_FloatingToolTip _toolTipInstance;

		Color _selectedColor;
		Color _normalColor;
		Sprite _buttonSprite;
		Image _slotImage;
		Camera _view;
		GameObject _followCursorObject;
		Transform _imageParent;
		PartSize _currentSize;
		bool _adjusted;
		bool _mouseOver;

		void Awake()
		{
			_imageParent = GetComponentInParent<Canvas>().transform;
		}

		void Update()
		{
			if (!_towerComponent) return;
			FollowCursorProcedure();
			CheckForSlotOverSlot();

			// Deselect slot on mouse up
			if(Input.GetMouseButtonUp(0) && !_mouseOver && SlotFollowingCursor == this) { StartCoroutine(DeselectSlotDelayed()); }
			_slotImage.color = this == SlotFollowingCursor ? _selectedColor : _normalColor;
		}
		
		public int CompareTo(TC_UI_TP_InventorySlot other)
		{
			if (_towerComponent.SlotNumber > other._towerComponent.SlotNumber) return 1;
			if (_towerComponent.SlotNumber == other._towerComponent.SlotNumber) return 0;
			return -1;
		}

		public void Initialize(TC_UI_TP_InventorySlotProperties slotProperties)
		{
			_towerComponent = slotProperties.TowerComponent;
			_towerComponent.SetIsPreview(true);
			_detector = slotProperties.SlotDetector;
			_distanceFromCamera = slotProperties.DistanceFromCamera;
			_view = _towerComponent.GetView();
			GetComponentInChildren<RawImage>().texture = _view.targetTexture;
			_selectedColor = slotProperties.SelectedColor;
			_normalColor = slotProperties.NormalColor;
			_buttonSprite = slotProperties.ButtonSprite;
			_toolTipPrefab = slotProperties.ToolTip;
			AddEvents();
			AddButtonToObject();
		}
		
		/// <summary>
		/// Adds components to simulate button functionality such as showing is selected or highlited. 
		/// </summary>
		void AddButtonToObject()
		{
			RectTransform rectTrans = GetComponentInChildren<RawImage>().GetComponent<RectTransform>();
			rectTrans.offsetMin = Vector2.zero;
			rectTrans.offsetMax = Vector2.zero;
			rectTrans.anchorMin = Vector2.zero; 
			rectTrans.anchorMax = new Vector2(1, 1);
             
			_slotImage = gameObject.AddComponent<Image>();
             
			Image slotBoarder = new GameObject().AddComponent<Image>();
			slotBoarder.sprite = _buttonSprite;
			slotBoarder.transform.SetParent(transform);
			slotBoarder.rectTransform.sizeDelta = new Vector2(210, 210);
			slotBoarder.rectTransform.anchoredPosition = Vector2.zero;
			slotBoarder.name = "Slot Border";
		}
		
		/// <summary>
		/// Used to change the order of slots in an inventory.
		/// </summary>
		/// <param name="nextSlot"></param>
		void SwapDirectoryOrder(TC_UI_TP_InventorySlot nextSlot)
		{
			SlotFollowingCursor.transform.SetSiblingIndex(_towerComponent.SlotNumber);
			TC_UI_TP_InventorySlot[] slots = transform.parent.GetComponentsInChildren<TC_UI_TP_InventorySlot>();
			for(int i = 0; i < slots.Length; i++)
			{
				slots[i].GetTowerComponent().SlotNumber = i;
				slots[i].GetTowerComponent().GetComponent<ISerializableTowerComponent>().SaveToFile();
			}
			_towerComponent.GetComponent<ISerializableTowerComponent>().GetInventory().SortSlots();
		}

		public void EnterSlot()
		{
			_mouseOver = true;
			_toolTipInstance = Instantiate(_toolTipPrefab, _imageParent);
			_toolTipInstance.Initialize(_towerComponent.name + "\n" + _towerComponent.GetStats());
		}
		public void ExitSlot()
		{
			_mouseOver = false;
			if(_toolTipInstance){Destroy(_toolTipInstance.gameObject);}
			_toolTipInstance = null;
		}
		public void AddEvents()
		{
			AddEventTrigger((data) => { EnterSlot(); }, EventTriggerType.PointerEnter);
			AddEventTrigger((data) => { ExitSlot(); }, EventTriggerType.PointerExit);
			AddEventTrigger((data) => { FollowCursor(); }, EventTriggerType.BeginDrag);
			AddEventTrigger((data) => { StartCoroutine(DeselectSlotDelayed()); }, EventTriggerType.EndDrag);
			AddEventTrigger((data) => { SelectThis(); }, EventTriggerType.PointerClick);
		}

		void SelectThis() { SlotFollowingCursor = this; }

		void AddEventTrigger(UnityAction<BaseEventData> call, EventTriggerType triggerType)
		{
			EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry eventEntry = new EventTrigger.Entry();
			eventEntry.eventID = triggerType;
			eventEntry.callback.AddListener(call);
			eventTrigger.triggers.Add(eventEntry);
		}
		
		
		public TowerComponent GetTowerComponent() { return _towerComponent; }

		void FollowCursor()
		{
			IsFollowingCursor = true;
			if (_followCursorObject) Destroy(_followCursorObject.gameObject);
			SelectThis();
			
			if(DragImage) {InstantiateImage(); return; }
			InstantiatePart();
		}

		void InstantiateImage()
		{
			_followCursorObject = new GameObject();
			RawImage rawImage = _followCursorObject.AddComponent<RawImage>();
			rawImage.texture = _towerComponent.GetView().targetTexture;
			_followCursorObject.transform.SetParent(_imageParent);
			rawImage.raycastTarget = false;
		}
		
		void InstantiatePart()
		{
			_followCursorObject = Instantiate(_towerComponent.gameObject);
			TowerComponent towerComponent = _followCursorObject.GetComponent<TowerComponent>();
			
			towerComponent.ActivateCamera(false);
			towerComponent.SetIsPreview(!TP_Weapon.AttachingWeapon);
			towerComponent.SetHide(false);
			towerComponent.SetShrink(false);
			towerComponent.name = "part";
			towerComponent.transform.eulerAngles = new Vector3(0, 0, 0);
			towerComponent.CorrectRotation();
		}

		public void DeselectSlot()
		{
			SlotFollowingCursor = null;
			IsFollowingCursor = false;
			if (_followCursorObject) { Destroy(_followCursorObject); }
		}

		/// <summary>
		/// Delay so other components can reference the selected slot before it is deselected.
		/// </summary>
		/// <returns></returns>
		IEnumerator DeselectSlotDelayed()
		{
			yield return new WaitForEndOfFrame();
			DeselectSlot();
		}

		void FollowCursorProcedure()
		{
			if (!_followCursorObject) { return;}
			if(_followCursorObject.GetComponent<TowerComponent>()) {FollowCursorPart(); return; }
			FollowCursorImage();
			
		}

		void FollowCursorImage()
		{
			Vector2 mousePosition = Input.mousePosition;
			_followCursorObject.GetComponent<RectTransform>().position = mousePosition;
		}

		void FollowCursorPart()
		{
			TP_Weapon weapon = _towerComponent.GetComponent<TP_Weapon>();
			if (weapon) { WeaponSnap(weapon); }
			Vector3 temp = Input.mousePosition;
			temp.z = _distanceFromCamera; 
				
			// Set this to be the distance you want the object to be placed in front of the camera.
			if (Camera.main is null) return;
			
			_detector.transform.position = Camera.main.ScreenToWorldPoint(temp);
			
			if (!IsFollowingCursor || !_followCursorObject) return;
			
			if (_detector.IsTouchingSlot && weapon && _detector.GetSlot()) { _followCursorObject.transform.position = _detector.GetSlot().transform.position; }
			else { _followCursorObject.transform.position = Camera.main.ScreenToWorldPoint(temp); }
		}

		void CheckForSlotOverSlot()
		{
			if (!_mouseOver || !Input.GetMouseButtonUp(0)) return;
			if (SlotFollowingCursor.IsFollowingCursor) { SwapDirectoryOrder(this); }
		}
		
		void UpSize()
		{
			if (_currentSize == PartSize.Small) _currentSize = PartSize.Medium;
			else if (_currentSize == PartSize.Medium) _currentSize = PartSize.Large;
			_towerComponent.GetComponentInChildren<TP_TowerBase>().SetSize(_currentSize);
			_towerComponent.GetComponentInChildren<TP_WeaponMount>().SetSize(_currentSize);
			_towerComponent.GetComponentInChildren<TP_WeaponMountStyle>().SetSize(_currentSize);
		}

		void WeaponSnap(TP_Weapon weapon)
		{
			if (Input.GetMouseButtonUp(0) && !_detector.IsTouchingSlot)
			{
				if (_followCursorObject) Destroy(_followCursorObject.gameObject);
				return;
			}
			if (!Input.GetMouseButtonUp(0) || !_detector.IsTouchingSlot || !IsFollowingCursor || weapon.IsTouchingWeapon()) return;
			_detector.GetSlot().SetWeapon(_followCursorObject.GetComponent<TP_Weapon>(), true);
			_followCursorObject = null;
		}
	}
}