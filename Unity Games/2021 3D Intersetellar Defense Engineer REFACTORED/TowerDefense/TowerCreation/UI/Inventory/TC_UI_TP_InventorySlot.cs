using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using TowerDefense.TowerParts.Weapon;
using TowerDefense.TowerParts.UX;

namespace TowerDefense.TowerCreation.UI.Inventory
{
	/// <summary>
	/// UI element depicting a created tower part with some inventory management functionality. Contains some zombie code from old implementation that has yet to be reimplemented.
	/// </summary>
	public class TC_UI_TP_InventorySlot : MonoBehaviour
	{
		/// <summary>
		/// Used to communicate with other scripts to invoke behavior involving drag and dropping tower parts in the creation interface.
		/// </summary>
		public static TC_UI_TP_InventorySlot SlotFollowingCursor;
		public static List<TComp_TowerState> States = new List<TComp_TowerState>();
		
		public bool Shrink;
		public bool IsFollowingCursor;
		
		[SerializeField] TowerComponent _towerComponent;
		[SerializeField] TC_UI_SlotDetector _detector;
		[SerializeField] float _distanceFromCamera = 15;
		
		Camera _view;
		TowerComponent _followCursorPart;
		PartSize _currentSize;
		bool _adjusted;
		bool _mouseOver;

		void Update()
		{
			if (!_towerComponent) return;
			//if (_towerComponent.GetComponent<TComp_TowerState>()) { if (!_adjusted) { StartCoroutine(CheckForTouchingWeapons()); } }
			UniversalFollowCursorProcedure();
			CheckForSlotOverSlot();
				
			//Button button = GetComponent<Button>();
			//if (gameObject == EventSystem.current.currentSelectedGameObject) { TowerConstructionInterface.SelectSlot(this); }
			
			if (Input.GetMouseButtonUp(0) && gameObject.activeInHierarchy) StartCoroutine(StopFollowingCursor());
		}

		public void Initialize(TowerComponent towerComponent, TC_UI_SlotDetector tcUISlotDetectorIn, float distanceFromCameraIn)
		{
			_towerComponent = towerComponent;
			_towerComponent.SetIsPreview(true);
			_detector = tcUISlotDetectorIn;
			_distanceFromCamera = distanceFromCameraIn;
			_view = _towerComponent.GetView();
			GetComponentInChildren<RawImage>().texture = _view.targetTexture;
		}
		
		public static void SwapDirectoryOrder(TC_UI_TP_InventorySlot nextSlot)
		{
			// if (nextSlot != slotFollowingCursor)
			// {
			// 	slotFollowingCursor.transform.SetSiblingIndex(nextSlot.transform.GetSiblingIndex());
			// 	string path = TowerPartFileManager.rootDir + TowerPartFileManager.GetPartDirectory(nextSlot.towerPart) + TowerPartFileManager.directoryListFileName;
			// 	TPStreamReader tpStreamReader = new TPStreamReader(path);
			// 	tpStreamReader.ChangeLineNumber(slotFollowingCursor.towerPart.customePartFilePath, nextSlot.towerPart.customePartFilePath);
			// 	slotFollowingCursor.SyncSlotOrders();
			// }
		}

		public void SyncSlotOrders()
		{
			// GameObject p1 = transform.parent.gameObject;
			// foreach(TowerPartInventorySlot partOtherSlot in towerPart.inventorySlots)
			// {
			// 	if(partOtherSlot != this && partOtherSlot)
			// 	{
			// 		GameObject p2 = partOtherSlot.transform.parent.gameObject;
			// 		for(int i = 0; i<p1.transform.parent.childCount; i++)
			// 		{
			// 			foreach(TowerPartInventorySlot p2Slot in p2.GetComponentsInChildren<TowerPartInventorySlot>())
			// 			{
			// 				if(p1.transform.GetChild(i).GetComponent<TowerPartInventorySlot>().towerPart == p2Slot.towerPart)
			// 				{
			// 					p2Slot.transform.SetSiblingIndex(i);
			// 				}
			// 			}
			// 		}
			// 	}
			// }
		}

		public void EnterSlot() { _mouseOver = true; }
		public void ExitSlot() { _mouseOver = false; }
		public void AddEvents()
		{
			EventTrigger eventTrigger1 = gameObject.AddComponent<EventTrigger>();
			EventTrigger eventTrigger2 = gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry eventEntry1 = new EventTrigger.Entry();
			EventTrigger.Entry eventEntry2 = new EventTrigger.Entry();
			eventEntry1.eventID = EventTriggerType.PointerEnter;
			eventEntry2.eventID = EventTriggerType.PointerExit;
			eventEntry1.callback.AddListener((data) => { EnterSlot(); });
			eventEntry2.callback.AddListener((data) => { ExitSlot(); });
			eventTrigger1.triggers.Add(eventEntry1);
			eventTrigger2.triggers.Add(eventEntry2);
		}
		
		public TowerComponent GetTowerComponent() { return _towerComponent; }
		
		public void FollowCursor()
		{
			IsFollowingCursor = true;
			if (_followCursorPart) Destroy(_followCursorPart.gameObject);
			SlotFollowingCursor = this;
			_followCursorPart = Instantiate(_towerComponent.gameObject).GetComponent<TowerComponent>();
			if(!_towerComponent)Debug.Log("null");
			if (Shrink) { _followCursorPart.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f); }
			_followCursorPart.ActivateCamera(false);
			_followCursorPart.SetIsPreview(false);
			if (_followCursorPart.GetComponent<TP_Ammo>()) _followCursorPart.SetIsPreview(true);
			_followCursorPart.SetHide(false);
			_followCursorPart.SetShrink(false);
			_followCursorPart.name = "part";
			_followCursorPart.transform.eulerAngles = new Vector3(0, 0, 0);
			_followCursorPart.CorrectRotation();
		}

		void DetermineFollowCursorProcedure(ref TComp_TowerState state, ref TP_Weapon weapon, ref TP_Ammo ammo)
		{
			if (!IsFollowingCursor) return;
			if (state) PlaceTower(ref state);
			else if (weapon) { WeaponSnap(weapon); }
			else if (ammo) { LoadAmmo(ref ammo); }
		}

		void UniversalFollowCursorProcedure()
		{
			TComp_TowerState state = _towerComponent.GetComponent<TComp_TowerState>();
			TP_Weapon weapon = _towerComponent.GetComponent<TP_Weapon>();
			TP_Ammo ammo = _towerComponent.GetComponent<TP_Ammo>();
			
			DetermineFollowCursorProcedure(ref state, ref weapon, ref ammo);
			Vector3 temp = Input.mousePosition;
			temp.z = _distanceFromCamera; 
				
			// Set this to be the distance you want the object to be placed in front of the camera.
			if (Camera.main is null) return;
			
			_detector.transform.position = Camera.main.ScreenToWorldPoint(temp);
			
			if (!IsFollowingCursor || !_followCursorPart) return;
			
			if (_detector.IsTouchingSlot && weapon) { _followCursorPart.transform.position = _detector.GetSlot().transform.position; }
			else { _followCursorPart.transform.position = Camera.main.ScreenToWorldPoint(temp); }
		}

		void CheckForSlotOverSlot()
		{
			if (!_mouseOver || !Input.GetMouseButtonUp(0)) return;
			if (!SlotFollowingCursor) return;
			if (_towerComponent != SlotFollowingCursor._towerComponent && SlotFollowingCursor.IsFollowingCursor) { SwapDirectoryOrder(this); }
		}
		
		void UpSize()
		{
			if (_currentSize == PartSize.Small) _currentSize = PartSize.Medium;
			else if (_currentSize == PartSize.Medium) _currentSize = PartSize.Large;
			_towerComponent.GetComponentInChildren<TP_TowerBase>().SetSize(_currentSize);
			_towerComponent.GetComponentInChildren<TP_WeaponMount>().SetSize(_currentSize);
			_towerComponent.GetComponentInChildren<TP_WeaponMountStyle>().SetSize(_currentSize);
		}

		void PlaceTower(ref TComp_TowerState state)
		{
			// if(PathBuilder.pathBuilder && Input.GetMouseButtonUp(0))
			// {
			// 	if (PathBuilder.pathBuilder.TowerOverSlot()) PathBuilder.pathBuilder.LoadTower(ref state);
			// 	Destroy(part); 
			// }
			// else if (Input.GetMouseButtonUp(0)) { followCursor = false; Destroy(part); }
		}

		void LoadAmmo(ref TP_Ammo ammo)
		{
			// if (!TowerPartFactory.AmmoOverSlot() && Input.GetMouseButtonUp(0)) { if (part) Destroy(part); }
			// else if (Input.GetMouseButtonUp(0) && TowerPartFactory.AmmoOverSlot() && followCursor)
			// {
			// 	followCursor = false;
			// 	TowerPartFactory.LoadAmmoSlot(ref ammo);
			// 	Destroy(part);
			// }
		}

		void WeaponSnap(TP_Weapon weapon)
		{
			if (Input.GetMouseButtonUp(0) && !_detector.IsTouchingSlot)
			{
				if (_followCursorPart) Destroy(_followCursorPart.gameObject);
			}
			else if (Input.GetMouseButtonUp(0) && _detector.IsTouchingSlot && IsFollowingCursor && !weapon.IsTouchingWeapon())
			{
				_detector.GetSlot().SetWeapon(_followCursorPart.GetComponent<TP_Weapon>());
				_followCursorPart = null;
			}
		}

		IEnumerator StopFollowingCursor()
		{
			yield return new WaitForEndOfFrame();
			IsFollowingCursor = false;
			if(_followCursorPart){Destroy(_followCursorPart.gameObject);}
		}

		IEnumerator CheckForTouchingWeapons()
		{
			_adjusted = true;
			yield return null;
			_currentSize = _towerComponent.GetComponentInChildren<TP_TowerBase>().GetSize();
			bool upSize = false;
			bool update = false;
			foreach (WeaponMountSlot slot in _towerComponent.GetComponentInChildren<TP_WeaponMountStyle>().GetSlots())
			{
				if (!slot.GetWeapon().IsTouchingWeapon() || _currentSize == PartSize.Large) continue;
				upSize = true;
				update = true;
			}
			if (!upSize) yield break;
			{
				UpSize();
				upSize = false;
				yield return null;
				yield return null;
				foreach (WeaponMountSlot slot in _towerComponent.GetComponentInChildren<TP_WeaponMountStyle>().GetSlots())
				{
					if (!slot.GetWeapon().IsTouchingWeapon() || _currentSize == PartSize.Large) continue;
					upSize = true;
					update = true;
				}
				if (upSize) { UpSize(); }
				_currentSize = _towerComponent.GetComponentInChildren<TP_TowerBase>().GetSize();
				//if (update) {  states.Add(towerPart.GetComponent<TowerState>()); TowerPartFileManager.fileManager.AdjustTowerSize(); }
			}
		}
	}
}