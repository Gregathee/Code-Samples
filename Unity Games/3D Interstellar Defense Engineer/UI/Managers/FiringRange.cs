using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FiringRange : MonoBehaviour
{
    public static FiringRange firingRange;
	[SerializeField] TowerInfoPanel panel;
	[SerializeField] Transform towerPos;
	[SerializeField] TMP_InputField spawnFreqencyInput;
	[SerializeField] TMP_InputField speedInput;
	[SerializeField] TMP_InputField sizeInput;
	[SerializeField] Toggle groundToggle;
	[SerializeField] Toggle flyingToggle;
	[SerializeField] Toggle stealthToggle;
	[SerializeField] Toggle nonStealthToggle;
	[SerializeField] Toggle fireToggle;
	[SerializeField] Toggle iceToggle;
	[SerializeField] Toggle lightningToggle;
	[SerializeField] Toggle poisonToggle;
	[SerializeField] Toggle physicalToggle;
	[SerializeField] Toggle noneToggle;
	[SerializeField] List<Enemy> stockEnemies;
	[SerializeField] Transform groundSpawn;
	[SerializeField] Path path;
	[SerializeField] Path groundPath;
	List<Enemy> spawnedEnemies = new List<Enemy>();
	bool testing = false;
	bool exit = false;

	private void Awake()
	{
		firingRange = this;
		StartCoroutine(RunTest());
	}

	private void Update() 
	{
		if (spawnedEnemies.Count > 0)
		{
			DetermineEnemyPosistions();

			if (float.Parse(speedInput.text) > 0) foreach (Enemy enemy in spawnedEnemies) enemy.SetSpeed(float.Parse(speedInput.text));
			if (float.Parse(sizeInput.text) > 0) foreach (Enemy enemy in spawnedEnemies) enemy.SetSize(float.Parse(sizeInput.text));
		}
	}

	IEnumerator RunTest()
	{
		while(!exit)
		{
			yield return new WaitForSeconds(float.Parse(spawnFreqencyInput.text));
			if (testing ) 
			{
				bool suitableFound = false;
				int spawnIndex = 0;
				int i = 0;
				while (!suitableFound && i < 24) 
				{
					spawnIndex = Random.Range(0, stockEnemies.Count - 1); 
					i++;
					suitableFound = true;
					if (!groundToggle.isOn && !stockEnemies[spawnIndex].flying) suitableFound = false;
					if (!flyingToggle.isOn && stockEnemies[spawnIndex].flying) suitableFound = false; 
					if (!stealthToggle.isOn && stockEnemies[spawnIndex].IsStealthed()) suitableFound = false;
					if (!nonStealthToggle.isOn && !stockEnemies[spawnIndex].IsStealthed()) suitableFound = false;
					if (!fireToggle.isOn && stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Fire) < 0) suitableFound = false; 
					if (!iceToggle.isOn && stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Ice) < 0) suitableFound = false; 
					if (!lightningToggle.isOn && stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Lightning) < 0) suitableFound = false; 
					if (!poisonToggle.isOn && stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Poison) < 0) suitableFound = false; 
					if (!physicalToggle.isOn && stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Physical) < 0) suitableFound = false;
					if(
						!noneToggle.isOn &&
							(
								stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Fire) == 0 &&
								stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Ice) == 0 &&
								stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Lightning) == 0 &&
								stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Poison) == 0 &&
								stockEnemies[spawnIndex].GetResistance(WeaknessPriority.Physical) == 0
							) 
						)suitableFound = false; 
				}
				if (suitableFound)
				{
					Enemy spawnedEnemy = stockEnemies[spawnIndex];
					if (spawnedEnemy.flying)
					{
						spawnedEnemy = Instantiate(stockEnemies[spawnIndex], groundSpawn.position, transform.rotation).GetComponent<Enemy>();
						spawnedEnemy.AssignPath(path);
					}
					else
					{
						spawnedEnemy = Instantiate(stockEnemies[spawnIndex], groundSpawn.position, transform.rotation).GetComponent<Enemy>();
						spawnedEnemy.AssignPath(groundPath);
					}
					spawnedEnemies.Add(spawnedEnemy);
				}
			}
			yield return null;
		}
	}

	void DetermineEnemyPosistions()
	{
		int count = spawnedEnemies.Count;
		bool foundDead = false;
		int i = 0;
		while (i < count)
		{
			RemoveDead(ref foundDead, ref count);
			if (!foundDead) { i++; }
			foundDead = false;
		}
		spawnedEnemies.Sort();
		i = 0;
		while (i < count)
		{
			RemoveDead(ref foundDead, ref count);
			if (!foundDead)
			{
				if (spawnedEnemies[i]) spawnedEnemies[i].SetPosition(i);
				i++;
			}
			foundDead = false;
		}
	}

	void RemoveDead(ref bool foundDead, ref int count)
	{
		for (int j = 0; j < spawnedEnemies.Count; j++)
		{
			if (spawnedEnemies[j].IsDead()) { foundDead = true; }
			if (foundDead)
			{
				Enemy enemy = spawnedEnemies[j];
				spawnedEnemies.RemoveAt(j);
				Destroy(enemy.gameObject);
				count = spawnedEnemies.Count;
				break;
			}
		}
	}

public void TestTower()
	{
		TowerAssembler.towerAssembler.StopRotation();
		TowerAssembler.towerAssembler.GetTempTower().transform.position = towerPos.position;
		TowerAssembler.towerAssembler.GetTempTower().transform.eulerAngles = new Vector3(0, 270, 0);
		TowerAssembler.towerAssembler.GetTempTower().GetComponentInChildren<WeaponMount>().transform.localEulerAngles = new Vector3(0, 0, 0);
		TowerAssembler.towerAssembler.GetTempTower().GetComponentInChildren<WeaponMountStyle>().transform.localEulerAngles = new Vector3(0, 0, 0);
		TowerAssembler.towerAssembler.GetTempTower().SetIsPreview(false);
		TowerAssembler.towerAssembler.GetTempTower().CorrectRotation();
		panel.SetTower(TowerAssembler.towerAssembler.GetTempTower());
	}

	public void StartTest() { testing = true; }
	public void StopTest() 
	{
		testing = false;
		foreach (Enemy enemy in spawnedEnemies) Destroy(enemy.gameObject);
		spawnedEnemies.Clear();
	}
}
