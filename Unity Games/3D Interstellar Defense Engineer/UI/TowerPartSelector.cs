using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerPartSelector : MonoBehaviour
{
    [SerializeField] List<TowerPart> towerPartPrefabs = null;
    List<GameObject> towerObjects = new List<GameObject>();
    [SerializeField] Vector3[] positions = null;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float shrinkSpeed = 1;
    [SerializeField] float positionWindow = 1;
    public static bool forceHide = false;
    int currentSelection = 2;
    bool exit = false;
    bool partsInMotion = false;
    float deltaX;
    float deletaZ;
    float originalRotateSpeed;

    private void Start()
    {
        originalRotateSpeed = rotateSpeed;
        deltaX = Mathf.Abs(positions[2].x - positions[3].x);
        deletaZ = Mathf.Abs(positions[2].z - positions[3].z);
        foreach(TowerPart part in towerPartPrefabs)
        {
            GameObject tPartObject = Instantiate(part, transform).gameObject;
            TowerPart tPart = tPartObject.GetComponent<TowerPart>();
            towerObjects.Add(tPartObject);
            tPart.SetShrink(true);
            tPart.transform.localScale = Vector3.zero;
            tPart.SetSize(PartSize.Medium);
        }
        int i;
        if (towerObjects.Count > 0)
        {
            for (i = 0; i < positions.Length; i++){ towerObjects[i].transform.position = positions[i];}
            for(i=i+0; i< towerObjects.Count; i++) { towerObjects[i].transform.position = positions[0]; }
        }
        else Debug.Log("TowerParts is empty");
        StartCoroutine(RotateParts());
    }

    public TowerPart GetCurrentPart() { return towerObjects[currentSelection].GetComponent<TowerPart>(); }

    public void Shrink(bool shrink) { for(int i = 0; i < towerObjects.Count; i++) towerObjects[i].GetComponent<TowerPart>().SetShrink(shrink); }

    public void SetSize(PartSize size)
    {
        Shrink(false);
        for(int i = 0; i < towerObjects.Count; i++) { towerObjects[i].GetComponent<TowerPart>().SetSize(size); }
    }

    public void StopRotation() {rotateSpeed = 0; }

    public void StartRotation() { rotateSpeed = originalRotateSpeed; }

    public void ShowMountStyle(bool hide){ towerObjects[currentSelection].SetActive(hide); }

    public void Hide(bool hideSelectionInstantly)
    {
        if (hideSelectionInstantly) towerObjects[currentSelection].transform.localScale = Vector3.zero;
        foreach (GameObject part in towerObjects)
        {
            part.GetComponent<TowerPart>().SetHide(true);
            StartCoroutine(HideParts(true, part));
        }
    }

    public void UnHide(bool scaleSelectionToSizeInstantly)
    {
        if (scaleSelectionToSizeInstantly) { towerObjects[currentSelection].GetComponent<TowerPart>().ScaleToSize(); }
        foreach (GameObject part in towerObjects)
        {
            part.GetComponent<TowerPart>().SetHide(false);
            StartCoroutine(HideParts(false, part));
        }
    }

    IEnumerator HideParts( bool hide, GameObject part)
    {
        float Xmax = 1;
        PartSize size = part.GetComponent<TowerPart>().GetSize();
        switch (size)
        {
            case PartSize.Small: Xmax = 0.5f; break;
            case PartSize.Medium: Xmax = 1; break;
            case PartSize.Large: Xmax = 1.5f; break;
        }
        Shrink(true);
        float speed = shrinkSpeed * Time.deltaTime;
        if (hide) speed *= -1;
        while (part.transform.localScale.x >= 0 && part.transform.localScale.x <= Xmax)
        {
            if ((part.transform.localScale + new Vector3(speed, speed, speed)).x < 0) break;
            part.transform.localScale = part.transform.localScale + new Vector3(speed, speed, speed);
			if (forceHide) { hide = true; break; }
            yield return null;
        }
        if (hide) part.transform.localScale = Vector3.zero;
        else { part.GetComponent<TowerPart>().ScaleToSize(); }
    }

    IEnumerator RotateParts()
    {
        while (!exit)
        {
            foreach (GameObject part in towerObjects) { part.transform.eulerAngles = new Vector3(0, part.gameObject.transform.eulerAngles.y + (rotateSpeed * Time.deltaTime), 0); }
            yield return null;
        }
    }

    int PartIterator(int iterator, int modifier)
    {
        if (modifier == 0) return iterator;
        if (iterator + modifier >= towerPartPrefabs.Count)
        {
            while (modifier > 0)
            {
                if (iterator + 1 >= towerPartPrefabs.Count) iterator = -1;
                if (modifier > 0) { modifier--; iterator++; }
            }
        }
        else if (iterator + modifier < 0)
        {
            while (modifier < 0)
            {
                if (iterator - 1 < 0) { iterator = towerPartPrefabs.Count; }
                modifier++;
                iterator--;
            }
        }
        else {  iterator+=modifier;
        }
        return iterator;
    }

    public void SelectRightButton() { StartCoroutine(MoveParts(false)); }
    public void SelectLeftButton() { StartCoroutine(MoveParts(true)); }

    IEnumerator MoveParts(bool left)
    {
        if (!partsInMotion)
        {
            partsInMotion = true;
            int i1, i2, i3, i4, p1, p2, p3, p4;
            if (left) { i1 = PartIterator(currentSelection, -1); i2 = currentSelection; i3 = PartIterator(currentSelection, 1); i4 = PartIterator(currentSelection, 2); }
            else { i1 = PartIterator(currentSelection, 1); i2 = currentSelection; i3 = PartIterator(currentSelection, -1); i4 = PartIterator(currentSelection, -2); }
            if (left) { p1 = 0; p2 = 1; p3 = 2; p4 = 3; } else { p1 = 4; p2 = 3; p3 = 2; p4 = 1; }
            deltaX = positions[p2].x - towerObjects[currentSelection].transform.position.x;
            deletaZ = positions[p2].z - towerObjects[currentSelection].transform.position.z;
            float xSpeed = deltaX * moveSpeed * Time.deltaTime;
            float zSpeed = deletaZ * moveSpeed * Time.deltaTime;
            if (left) towerObjects[i4].transform.position = positions[4];
            else towerObjects[i4].transform.position = positions[0];
            while (towerObjects[currentSelection].transform.position.x > positions[p2].x + positionWindow || towerObjects[currentSelection].transform.position.x < positions[p2].x - positionWindow)
            {
                towerObjects[i1].transform.position = new Vector3(towerObjects[i1].transform.position.x + xSpeed, towerObjects[i1].transform.position.y, towerObjects[i1].transform.position.z + zSpeed);
                towerObjects[i2].transform.position = new Vector3(towerObjects[i2].transform.position.x + xSpeed, towerObjects[i2].transform.position.y, towerObjects[i2].transform.position.z + zSpeed);
                towerObjects[i3].transform.position = new Vector3(towerObjects[i3].transform.position.x + xSpeed, towerObjects[i3].transform.position.y, towerObjects[i3].transform.position.z + zSpeed);
                towerObjects[i4].transform.position = new Vector3(towerObjects[i4].transform.position.x + xSpeed, towerObjects[i4].transform.position.y, towerObjects[i4].transform.position.z + zSpeed);
                yield return null;
            }
            towerObjects[i1].transform.position = positions[p1]; towerObjects[i2].transform.position = positions[p2]; towerObjects[i3].transform.position = positions[p3]; towerObjects[i4].transform.position = positions[p4];
            currentSelection = i3;
            SubmitPart();
            partsInMotion = false;
        }
    }

    public void SubmitPart() { towerObjects[currentSelection].GetComponent<TowerPart>().SendPartToAssembler();}

    public void JumpToPart(TowerPart partToJumpTo)
	{
        int i = 0;
        GameObject part = towerObjects[i];
        while(part.GetComponent<TowerPart>().GetPrefabFilePath() != partToJumpTo.GetPrefabFilePath() && i < towerPartPrefabs.Count-1)
        {
            if (part.GetComponent<TowerPart>().GetPrefabFilePath() != partToJumpTo.GetPrefabFilePath()) i++;
            part = towerObjects[i];
        }
        foreach (GameObject towerPart in towerObjects) {towerPart.transform.position = positions[0]; }
        towerObjects[i].transform.position = positions[2];
		currentSelection = i;
        towerObjects[PartIterator(i, -1)].transform.position = positions[1];
        towerObjects[PartIterator(i, 1)].transform.position = positions[3];
        towerObjects[PartIterator(i, 2)].transform.position = positions[4];
        towerObjects[currentSelection].GetComponent<TowerPart>().SetMaterials(partToJumpTo.mat1, partToJumpTo.mat2, partToJumpTo.mat3);
	}
}