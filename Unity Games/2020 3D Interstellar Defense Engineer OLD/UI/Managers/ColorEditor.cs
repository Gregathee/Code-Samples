using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorEditor : MonoBehaviour
{
    public static ColorEditor colorEditor;
    Material mat1;
    Material mat2;
    Material mat3;
    Material currentMat;
    Color color1;
    Color color2;
    Color color3;
    Color currentColor;
    Color ogcolor1;
    Color ogcolor2;
    Color ogcolor3;
    [SerializeField] Slider colorSlider1;
    [SerializeField] Slider colorSlider2;
    [SerializeField]Slider colorSlider3;
    [SerializeField] GameObject matScreen1;
    [SerializeField] GameObject matScreen2;
    [SerializeField] GameObject matScreen3;
    [SerializeField] List<TowerPartSelector> partSelectors;
    [SerializeField] RawImage colorSample;
    TowerPart part;
    int matNumber = 1;
    int selectorNumber = 0;

	private void Awake()
	{
        colorEditor = this;
	}

	private void Update()
	{
		if(mat1 && mat2 && mat3)
		{
            Color newColor = new Color(colorSlider1.value, colorSlider2.value, colorSlider3.value, 1);
            if (part.GetComponent<Spray>())  newColor = new Color(colorSlider1.value, colorSlider2.value, colorSlider3.value, 0.5f);
            currentColor = newColor;
            currentMat.color = newColor;
            colorSample.color = newColor;
            UpDateColor();
            part.SetMaterials(mat1, mat2, mat3);
            partSelectors[selectorNumber].GetCurrentPart().SetMaterials(mat1, mat2, mat3);
        }
	}

    public void SetPartColors()
	{
        part.SetMaterials(mat1, mat2, mat3);
        matScreen1.SetActive(false);
        matScreen2.SetActive(false);
        matScreen3.SetActive(false);
    }

	public void SetUpEditor()
	{
        matScreen1.SetActive(true);
        matScreen2.SetActive(false);
        matScreen3.SetActive(false);
	}

    public void ClearEditor() 
    {
        mat1.color = ogcolor1;
        mat2.color = ogcolor2;
        mat3.color = ogcolor3;
        matScreen1.SetActive(false);
        matScreen2.SetActive(false);
        matScreen3.SetActive(false);
        part.SetMaterials(mat1, mat2, mat3);
        partSelectors[selectorNumber].GetCurrentPart().SetMaterials(mat1, mat2, mat3);
        mat1 = null; mat2 = null; mat3 = null;
    }

    public void EditWeapon() 
    {
        part = TowerAssembler.towerAssembler.GetWeapon(); SetMats();
        if (part.GetComponent<WeaponProjectile>()) selectorNumber = 2;
        if (part.GetComponent<WeaponSprayer>()) selectorNumber = 3;
        if (part.GetComponent<WeaponMelee>()) selectorNumber = 4;
        if (part.GetComponent<AdvancedTargetingSystem>()) selectorNumber = 5;
    }
    public void EditAmmo() 
    {
        part = TowerAssembler.towerAssembler.GetAmmo(); SetMats();
        if (part.GetComponent<Projectile>()) selectorNumber = 6;
        if (part.GetComponent<Spray>()) selectorNumber = 7;
    }

    public void EditMount() { part = TowerAssembler.towerAssembler.GetWeaponMount(); SetMats(); selectorNumber = 1; }
    public void EditBase() { part = TowerAssembler.towerAssembler.GetTowerBase(); SetMats(); selectorNumber = 0; }
    
    void SetMats() 
    {
        matScreen1.SetActive(true); matScreen2.SetActive(false); matScreen3.SetActive(false);
        mat1 = new Material (part.mat1); mat2 = new Material(part.mat2); mat3 = new Material(part.mat3);
        ogcolor1 = mat1.color; ogcolor2 = mat2.color; ogcolor3 = mat3.color;
        color1 = mat1.color; color2 = mat2.color; color3 = mat3.color;
        currentColor = color1; currentMat = mat1; matNumber = 1;
        SetSliders();
    }

    void SetSliders()
	{
        colorSlider1.value = currentColor.r;
        colorSlider2.value = currentColor.g;
        colorSlider3.value = currentColor.b;
	}

    void UpDateColor()
	{
        switch (matNumber)
        {
            case 1: color1 = currentColor; break;
            case 2: color2 = currentColor; break;
            case 3: color3 = currentColor; break;
        }
    }

    public void NextMat()
	{
        matNumber++;
        if (matNumber > 3) matNumber = 1;
        ChangeMat(matNumber);
        SetSliders();
    }

    public void PreviousMat()
	{
        matNumber--;
        if (matNumber < 1) matNumber = 3;
        ChangeMat(matNumber);
        SetSliders();
    }

    void ChangeMat(int matNum)
	{
        switch (matNum)
		{
            case 1: currentMat = mat1; currentColor = color1; break;
            case 2: currentMat = mat2; currentColor = color2; break;
            case 3: currentMat = mat3; currentColor = color3; break;
        }
	}

}
