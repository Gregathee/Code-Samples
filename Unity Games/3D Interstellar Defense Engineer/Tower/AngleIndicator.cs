using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleIndicator : MonoBehaviour
{
	[SerializeField] GameObject beam = null;
	[SerializeField] GameObject[] indicator45 = null;
	[SerializeField] GameObject[] indicator90 = null;
	[SerializeField] GameObject[] indicator135 = null;
	[SerializeField] GameObject[] indicator180 = null;
	[SerializeField] GameObject[] indicator225 = null;
	[SerializeField] GameObject[] indicator270 = null;
	[SerializeField] GameObject[] indicator315 = null;
	[SerializeField] GameObject[] indicator360 = null;
	GameObject[][] indicators = null;
	int i = 0;
	float turretRange;

	private void Awake()
	{
		indicators = new GameObject[][] {indicator45, indicator90, indicator135, indicator180,
										 indicator225, indicator270, indicator315, indicator360};
	}

	void ActivateIndicatorGroup(GameObject[] indicatorsIn, bool activate)
	{
		beam.transform.localScale = new Vector3(1, 1, turretRange);
		foreach(GameObject indicator in indicatorsIn)
		{
			indicator.SetActive(activate);
			indicator.transform.localScale = new Vector3(turretRange, 1, turretRange);
		}
	}

	public void ShowIndicators(bool show)
	{
		if(show)
		{
			if (i > 0)
			{
				i--;
				for (; i >= 0; i--)
				{
					ActivateIndicatorGroup(indicators[i], true);
				}
			}
		}
		else
		{
			foreach (GameObject[] indicator in indicators)
			{
				ActivateIndicatorGroup(indicator, false);
			}
		}
	}

	public void SetAngle(TurretAngle turretAngle, float turretRange)
	{
		this.turretRange = turretRange;
		foreach(GameObject[] indicator in indicators)
		{
			ActivateIndicatorGroup(indicator, false);
		}
		
		switch (turretAngle)
		{
			case TurretAngle.A45:
				i++;
				break;
			case TurretAngle.A90:
				i += 2;
				break;
			case TurretAngle.A135:
				i += 3;
				break;
			case TurretAngle.A180:
				i += 4;
				break;
			case TurretAngle.A225:
				i += 5;
				break;
			case TurretAngle.A270:
				i += 6;
				break;
			case TurretAngle.A315:
				i += 7;
				break;
			case TurretAngle.A360:
				i += 8;
				break;
		}
	}
}


