using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class TowerPart : MonoBehaviour
{
    [SerializeField] TowerPartType partType = TowerPartType.Base;
    [SerializeField] Camera view = null;
    [SerializeField] protected PartSize size = PartSize.Medium;
    [SerializeField] protected List<Light> showcaseLights = new List<Light>();
    [SerializeField] string prefabFilePath = "";
    [SerializeField] List<MeshRenderer> colorSection1;
    [SerializeField] List<MeshRenderer> colorSection2;
    [SerializeField] List<MeshRenderer> colorSection3;
    public Material mat1;
    public Material mat2;
    public Material mat3;
    public GameObject[] rotatableParts;
    public List<TowerPartInventorySlot> inventorySlots = new List<TowerPartInventorySlot>();
    public string customePartFilePath = "";
    public bool isPreview = true;
    public bool shrink = true;
    public bool hide = true;
    float rotatePartSpeed = 50;

	private void Start()
	{
        if (GetComponent<WeaponMountSlot>() || GetComponent<WeaponMountStyle>() || GetComponent<TowerState>()) { }
        else
        {
            mat1 = new Material(mat1);
            mat2 = new Material(mat2);
            mat3 = new Material(mat3);
        }
    }


	//private void OnDrawGizmos() { Handles.Label(transform.position, size.ToString() + " " + shrink + " " + hide);}

    protected virtual void Update()
    {
        if (!shrink && !hide && partType != TowerPartType.TowerState){  ScaleToSize();}
        if (isPreview)
        {
            foreach (GameObject g in rotatableParts)
            {
                if (g)
                {
                    float newAngle;
                    newAngle = g.transform.localEulerAngles.y + (rotatePartSpeed * Time.deltaTime);
                    g.transform.localEulerAngles = new Vector3(0, newAngle, 0);
                }
                //else Debug.Log(name);
            }
        }
    }

    public void SetMaterials(Material m1, Material m2, Material m3 )
	{
        mat1 = m1; mat2 = m2; mat3 = m3;
        foreach (MeshRenderer renderer in colorSection1) renderer.material = mat1;
        foreach (MeshRenderer renderer in colorSection2) renderer.material = mat2;
        foreach (MeshRenderer renderer in colorSection3) renderer.material = mat3;
    }

    public void CorrectRotation() { foreach (GameObject g in rotatableParts) { g.transform.localEulerAngles = Vector3.zero; } }

	public string GetPrefabFilePath() { return prefabFilePath; }
    public void SetSize(PartSize sizeIn) { size = sizeIn; }
    public PartSize GetSize() { return size; }
    public void SetShrink(bool shrinkIn) { shrink = shrinkIn; }
    public void SetHide(bool hideIn) { hide = hideIn; }
    public void SetIsPreview(bool preview) { isPreview = preview;  }
    public void ActivateCamera(bool activate) { view.gameObject.SetActive(activate); }

    public ref Camera GetView() { return ref view; }
    public void SetView(Camera newView) { view = newView; view.targetTexture = new RenderTexture(view.targetTexture); }
    public void SetPartType(TowerPartType type) { partType = type; }
    public TowerPartType GetPartType() { return partType;}
    public void SendPartToAssembler() { TowerAssembler.towerAssembler.ChoosePart(this); }

    public void ScaleToSize()
    {
        shrink = false; hide = false;
        if(GetComponentInParent<WeaponMount>() && GetComponent<WeaponMountStyle>()) transform.localScale = new Vector3(1, 1, 1);
        else
            switch (size)
            {
                case PartSize.Small:
                    if (partType == TowerPartType.Weapon) gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    else gameObject.transform.localScale = new Vector3(0.5f, 1, 0.5f);
                    break;
                case PartSize.Medium: gameObject.transform.localScale = new Vector3(1, 1, 1); break;
                case PartSize.Large:
                    if (partType == TowerPartType.Weapon) gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    else gameObject.transform.localScale = new Vector3(1.5f, 1, 1.5f);
                    break;
            }
    }

    public void DestroySlotsThenSelf() { foreach (TowerPartInventorySlot slot in inventorySlots) { if(slot)Destroy(slot.gameObject); } Destroy(gameObject); }
}

