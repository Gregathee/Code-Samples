using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.TowerParts.UX
{
	/// <summary>
	/// Displays the angle that a turret can rotate.
	/// </summary>
	public class TP_UX_AngleIndicator : MonoBehaviour
	{
		[SerializeField] GameObject _beam;
		[SerializeField] GameObject[] _indicator45;
		[SerializeField] GameObject[] _indicator90; 
		[SerializeField] GameObject[] _indicator135;
		[SerializeField] GameObject[] _indicator180;
		[SerializeField] GameObject[] _indicator225;
		[SerializeField] GameObject[] _indicator270;
		[SerializeField] GameObject[] _indicator315;
		[SerializeField] GameObject[] _indicator360;
		
		GameObject[][] _indicators;
		int i;
		float _turretRange;

		void Awake()
		{
			_indicators = new GameObject[][]
			{
				_indicator45, _indicator90, _indicator135, _indicator180,
				_indicator225, _indicator270, _indicator315, _indicator360
			};
		}

		void ActivateIndicatorGroup(IEnumerable<GameObject> indicatorsIn, bool activate)
		{
			_beam.transform.localScale = new Vector3(1, 1, _turretRange);
			foreach (GameObject indicator in indicatorsIn)
			{
				indicator.SetActive(activate);
				indicator.transform.localScale = new Vector3(_turretRange, 1, _turretRange);
			}
		}

		public void ShowIndicators(bool show)
		{
			if (show)
			{
				if (i <= 0) return;
				i--;
				for (; i >= 0; i--) { ActivateIndicatorGroup(_indicators[i], true); }
			}
			else { foreach (GameObject[] indicator in _indicators) { ActivateIndicatorGroup(indicator, false); } }
		}

		public void SetAngle(TurretAngle turretAngle, float turretRange)
		{
			this._turretRange = turretRange;
			foreach (GameObject[] indicator in _indicators) { ActivateIndicatorGroup(indicator, false); }

			switch (turretAngle)
			{
				case TurretAngle.A45: i++; break;
				case TurretAngle.A90: i += 2; break;
				case TurretAngle.A135: i += 3; break;
				case TurretAngle.A180: i += 4; break;
				case TurretAngle.A225: i += 5; break;
				case TurretAngle.A270: i += 6; break;
				case TurretAngle.A315: i += 7; break;
				case TurretAngle.A360: i += 8; break;
			}
		}
	}


}