using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerPartInventorySlot : MonoBehaviour
{
	public static TowerPartInventorySlot slotFollowingCursor;
	[SerializeField] TowerPart towerPart;
	[SerializeField] SlotDetector detector;
	[SerializeField] float distanceFromCamera = 15;
	public static List<TowerState> states = new List<TowerState>();
	public bool Shrink = false;
	Camera view;
	GameObject part;
	public bool followCursor = false;
	PartSize currentSize;
	bool adjusted = false;
	bool mouseOver = false;

	private void Update()
	{
		if (towerPart)
		{
			if (towerPart.GetComponent<TowerState>()) { if (!adjusted) { StartCoroutine(CheckForTouchingWeapons()); } }
			TowerState state = towerPart.GetComponent<TowerState>();
			Weapon weapon = towerPart.GetComponent<Weapon>();
			Ammo ammo = towerPart.GetComponent<Ammo>();
			if (followCursor)
				if (state) PlaceTower(ref state);
				else if (weapon) { WeaponSnap(weapon); }
				else if (ammo) { LoadAmmo(ref ammo); }
			Vector3 temp = Input.mousePosition;
			temp.z = distanceFromCamera; // Set this to be the distance you want the object to be placed in front of the camera.
			detector.transform.position = Camera.main.ScreenToWorldPoint(temp);
			if (followCursor && part)
			{
				if (detector.IsTouchingSlot() && weapon) { part.transform.position = detector.GetSlot().transform.position; }
				else { part.transform.position = Camera.main.ScreenToWorldPoint(temp); }
			}
			Button button = GetComponent<Button>();
			if (gameObject == EventSystem.current.currentSelectedGameObject) { FileManager.SelectSlot(this); }
			if (mouseOver && Input.GetMouseButtonUp(0)) { if (slotFollowingCursor) 
				{ if (towerPart != slotFollowingCursor.towerPart && slotFollowingCursor.followCursor) { SwapDirectoryOrder(this); } } }
			if (Input.GetMouseButtonUp(0) && gameObject.activeInHierarchy) StartCoroutine(StopFollowingCursor());
		}
	}

	IEnumerator StopFollowingCursor() { yield return new WaitForEndOfFrame(); followCursor = false; }

	public static void SwapDirectoryOrder(TowerPartInventorySlot nextSlot)
	{
		if (nextSlot != slotFollowingCursor)
		{
			slotFollowingCursor.transform.SetSiblingIndex(nextSlot.transform.GetSiblingIndex());
			string path = FileManager.rootDir + FileManager.GetPartDirectory(nextSlot.towerPart) + FileManager.directoryListFileName;
			StreamReaderPro streamReader = new StreamReaderPro(path);
			streamReader.ChangeLineNumber(slotFollowingCursor.towerPart.customePartFilePath, nextSlot.towerPart.customePartFilePath, nextSlot);
			slotFollowingCursor.SyncSlotOrders();
		}
	}

	public void Print(string s) { Debug.Log(s); }

	public void SyncSlotOrders()
	{
		GameObject p1 = transform.parent.gameObject;
		foreach(TowerPartInventorySlot partOtherSlot in towerPart.inventorySlots)
		{
			if(partOtherSlot != this && partOtherSlot)
			{
				GameObject p2 = partOtherSlot.transform.parent.gameObject;
				for(int i = 0; i<p1.transform.parent.childCount; i++)
				{
					foreach(TowerPartInventorySlot p2Slot in p2.GetComponentsInChildren<TowerPartInventorySlot>())
					{
						if(p1.transform.GetChild(i).GetComponent<TowerPartInventorySlot>().towerPart == p2Slot.towerPart)
						{
							p2Slot.transform.SetSiblingIndex(i);
						}
					}
				}
			}
		}
	}

	public void EnterSlot() { mouseOver = true; }
	public void ExitSlot() { mouseOver = false; }
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

	IEnumerator CheckForTouchingWeapons()
	{
		adjusted = true;
		yield return null;
		currentSize = towerPart.GetComponentInChildren<TowerBase>().GetSize();
		bool upSize = false;
		bool update = false;
		foreach (WeaponMountSlot slot in towerPart.GetComponentInChildren<WeaponMountStyle>().GetSlots())
		{
			if (slot.GetWeapon().IsTouchingWeapon() && currentSize != PartSize.Large) { upSize = true; update = true;  }
		}
		if (upSize)
		{
			UpSize(); upSize = false;
			yield return null;yield return null;
			foreach (WeaponMountSlot slot in towerPart.GetComponentInChildren<WeaponMountStyle>().GetSlots())
			{
				if (slot.GetWeapon().IsTouchingWeapon() && currentSize != PartSize.Large) { upSize = true; update = true;  }
			}
			if (upSize) { UpSize(); }
			currentSize = towerPart.GetComponentInChildren<TowerBase>().GetSize();
			if (update) {  states.Add(towerPart.GetComponent<TowerState>()); FileManager.fileManager.AdjustTowerSize(); }
		}
	}

	void UpSize()
	{
		if (currentSize == PartSize.Small) currentSize = PartSize.Medium;
		else if (currentSize == PartSize.Medium) currentSize = PartSize.Large;
		towerPart.GetComponentInChildren<TowerBase>().SetSize(currentSize);
		towerPart.GetComponentInChildren<WeaponMount>().SetSize(currentSize);
		towerPart.GetComponentInChildren<WeaponMountStyle>().SetSize(currentSize);
	}

	public TowerPart GetTowerPart() { return towerPart; }

	public void Initialize(TowerPart towerPartIn, SlotDetector slotDetectorIn, float distanceFromCameraIn)
	{
		towerPart = towerPartIn;
		towerPartIn.inventorySlots.Add(this);
		towerPart.SetIsPreview(true);
		detector = slotDetectorIn;
		distanceFromCamera = distanceFromCameraIn;
		view = towerPart.GetView();
		GetComponentInChildren<RawImage>().texture = view.targetTexture;
	}

	void PlaceTower(ref TowerState state)
	{
		if(PathBuilder.pathBuilder && Input.GetMouseButtonUp(0))
		{
			if (PathBuilder.pathBuilder.TowerOverSlot()) PathBuilder.pathBuilder.LoadTower(ref state);
			Destroy(part); 
		}
		else if (Input.GetMouseButtonUp(0)) { followCursor = false; Destroy(part); }
	}

	void LoadAmmo(ref Ammo ammo)
	{
		if (!TowerAssembler.towerAssembler.AmmoOverSlot() && Input.GetMouseButtonUp(0)) { if (part) Destroy(part); }
		else if (Input.GetMouseButtonUp(0) && TowerAssembler.towerAssembler.AmmoOverSlot() && followCursor)
		{
			followCursor = false;
			TowerAssembler.towerAssembler.LoadAmmoSlot(ref ammo);
			Destroy(part);
		}
	}

	void WeaponSnap(Weapon weapon)
	{
		if (Input.GetMouseButtonUp(0) && !detector.IsTouchingSlot())
		{
			if (part) Destroy(part);
		}
		else if (Input.GetMouseButtonUp(0) && detector.IsTouchingSlot() && followCursor && !weapon.IsTouchingWeapon())
		{
			TowerAssembler.towerAssembler.AttachedWeapon(weapon, detector.GetSlot().GetSlotNubmer(), distanceFromCamera, true);
			Destroy(part);
		}
	}

	public void FollowCursor() 
	{
		followCursor = true;
		if (part) Destroy(part);
		slotFollowingCursor = this;
		part = Instantiate(towerPart.gameObject);
		if(towerPart.GetComponent<TowerState>())
			part.GetComponentInChildren<MountContainer>().HideIndicators();
		if (Shrink)
		{
			part.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			part.GetComponentInChildren<Light>().intensity = 1;
		}
		part.GetComponent<TowerPart>().ActivateCamera(false);
		part.GetComponent<TowerPart>().SetIsPreview(false);
		if (part.GetComponent<Ammo>()) part.GetComponent<TowerPart>().SetIsPreview(true);
		part.GetComponent<TowerPart>().SetHide(false);
		part.GetComponent<TowerPart>().SetShrink(false);
		part.name = "part";
		part.transform.eulerAngles = new Vector3(0, 0, 0);
		part.GetComponent<TowerPart>().CorrectRotation();
	}
}
