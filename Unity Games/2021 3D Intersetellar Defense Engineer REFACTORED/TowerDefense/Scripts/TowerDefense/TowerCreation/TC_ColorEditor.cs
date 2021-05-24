using System.Collections.Generic;
using TMPro;
using TowerDefense.TowerCreation.Factories;
using TowerDefense.TowerParts;
using TowerDefense.TowerParts.Ammo;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.TowerCreation
{
	/// <summary>
	/// Edits the colors of tower part materials during tower part creation
	/// </summary>
	public class TC_ColorEditor : MonoBehaviour
	{
		[SerializeField] TC_Fac_TowerPart _factory;
		[SerializeField] TMP_Text _header;
		[SerializeField] Slider _colorSliderR;
		[SerializeField] Slider _colorSliderG;
		[SerializeField] Slider _colorSliderB;
		[SerializeField] Image _colorSample;
		[SerializeField] Image _p1_colorIcon1;
		[SerializeField] Image _p1_colorIcon2;
		[SerializeField] Image _p1_colorIcon3;
		[SerializeField] Image _p2_colorIcon1;
		[SerializeField] Image _p2_colorIcon2;
		[SerializeField] Image _p2_colorIcon3;

		Image _colorIcon1;
		Image _colorIcon2;
		Image _colorIcon3;
		Material _mat1;
		Material _mat2;
		Material _mat3;
		Material _currentMat;
		Color _color1;
		Color _color2;
		Color _color3;
		Color _currentColor;
		Color _ogColor1;
		Color _ogColor2;
		Color _ogColor3;
		ColoredTowerPart _part;
		int _matNumber = 1;

		bool _slidersLocked;

		public void EditPart1()
		{
			_colorIcon1 = _p1_colorIcon1;
			_colorIcon2 = _p1_colorIcon2;
			_colorIcon3 = _p1_colorIcon3;
			_part = _factory.GetColoredTowerPart1();
			EditPart();
		}
		
		public void EditPart2()
		{
			_colorIcon1 = _p2_colorIcon1;
			_colorIcon2 = _p2_colorIcon2;
			_colorIcon3 = _p2_colorIcon3;
			_part = _factory.GetColoredTowerPart2();
			EditPart();
		}

		void EditPart()
		{
			SetMats();
			_matNumber = 1;
			_header.text = "Section 1";
			
			_slidersLocked = true;
			
			// Change sliders to match the curent color.
			_colorSliderR.value = _part.Mat1.color.r;
			_colorSliderG.value = _part.Mat1.color.g;
			_colorSliderB.value = _part.Mat1.color.b;
			_colorSample.color = _part.Mat1.color;
			
			_slidersLocked = false;
		}

		// Confirms the color changes and applies them to the tower component
		public void SetPartColors() { _part.SetMaterials(_mat1, _mat2, _mat3); }

		public void ClearEditor()
		{
			// Revert tower component colors back to original on cancel
			_mat1.color = _ogColor1;
			_mat2.color = _ogColor2;
			_mat3.color = _ogColor3;
			_part.SetMaterials(_mat1, _mat2, _mat3);
			_mat1 = null;
			_mat2 = null;
			_mat3 = null;
		}

		// Updates color on slider value change.
		public void UpdateColor()
		{
			if (_slidersLocked) return;
			float a = _part.GetComponent<TP_Ammo_Spray>() ? 0.5f : 1;
			Color newColor = new Color(_colorSliderR.value, _colorSliderG.value, _colorSliderB.value, a);
			_currentColor = newColor;
			_currentMat.color = newColor;
			_colorSample.color = newColor;
			
			switch (_matNumber)
			{
				case 1: 
					_color1 = _currentColor;
					_colorIcon1.color = _currentColor;
					break;
				case 2: 
					_color2 = _currentColor; 
					_colorIcon2.color = _currentColor;
					break;
				case 3: 
					_color3 = _currentColor; 
					_colorIcon3.color = _currentColor;
					break;
			}
			_part.SetMaterials(_mat1, _mat2, _mat3);
		}

		public void SetIcon1Colors(Color color1, Color color2, Color color3)
		{
			_p1_colorIcon1.color = color1;
			_p1_colorIcon2.color = color2;
			_p1_colorIcon3.color = color3;
		}
		
		public void SetIcon2Colors(Color color1, Color color2, Color color3)
		{
			_p2_colorIcon1.color = color1;
			_p2_colorIcon2.color = color2;
			_p2_colorIcon3.color = color3;
		}

		void SetMats()
		{
			_mat1 = new Material(_part.Mat1);
			_mat2 = new Material(_part.Mat2);
			_mat3 = new Material(_part.Mat3);
			
			_ogColor1 = _mat1.color;
			_ogColor2 = _mat2.color;
			_ogColor3 = _mat3.color;
			
			_color1 = _mat1.color;
			_color2 = _mat2.color;
			_color3 = _mat3.color;
			
			_colorIcon1.color = _mat1.color;
			_colorIcon2.color = _mat2.color;
			_colorIcon3.color = _mat3.color;
			
			_currentColor = _color1;
			_currentMat = _mat1;
			_matNumber = 1;
			SetSliders();
		}

		void SetSliders()
		{
			_slidersLocked = true;
			_colorSliderR.value = _currentColor.r;
			_colorSliderG.value = _currentColor.g;
			_colorSliderB.value = _currentColor.b;
			float a = _part.GetComponent<TP_Ammo_Spray>() ? 0.5f : 1;
			_colorSample.color = new Color(_colorSliderR.value, _colorSliderG.value, _colorSliderB.value, a);
			_slidersLocked = false;
		}

		public void NextMat()
		{
			_matNumber++;
			if (_matNumber > 3) _matNumber = 1;
			ChangeMat(_matNumber);
			SetSliders();
			_header.text = "Section " + _matNumber;
		}

		public void PreviousMat()
		{
			_matNumber--;
			if (_matNumber < 1) _matNumber = 3;
			ChangeMat(_matNumber);
			SetSliders();
			_header.text = "Section " + _matNumber;
		}

		void ChangeMat(int matNum)
		{
			switch (matNum)
			{
				case 1:
					_currentMat = _mat1;
					_currentColor = _color1;
					break;
				case 2:
					_currentMat = _mat2;
					_currentColor = _color2;
					break;
				case 3:
					_currentMat = _mat3;
					_currentColor = _color3;
					break;
			}
		}
	}
}