using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
	public static Spawner instance;
	public static float WorldBounds = 100f;
	public int plantFoodValue = 50;
	public int specimenFoodValue = 100;
	[SerializeField] float bounds = 100f;
	[SerializeField] GameObject nXBoarder = null;
	[SerializeField] GameObject pXBoarder = null;
	[SerializeField] GameObject nYBoarder = null;
	[SerializeField] GameObject pYBoarder = null;
	[SerializeField] Transform plantParent = null;
	[SerializeField] Transform herbivoreParent = null;
	[SerializeField] Transform carnivoreParent = null;
	[SerializeField] Transform omnivoreParent = null;
	[SerializeField] Transform treeParent = null;
	[SerializeField] float spawnRate = 0.2f;
	[SerializeField] GameObject plantPrefab = null;
	[SerializeField] GameObject herbivorePrefab = null;
	[SerializeField] GameObject carnivorePrefab = null;
	[SerializeField] GameObject omnivorePrefab = null;
	[SerializeField] GameObject treePrefab = null;
	[SerializeField] int treeCount = 10;
	[SerializeField] int herbivoreStartSpawnCount = 50;
	[SerializeField] int carnivoreStartSpawnCount = 10;
	[SerializeField] int omnivoreStartSpawnCount = 20;
	public TMP_Text herbivoreCount = null;
	public TMP_Text carnivoreCount = null;
	public TMP_Text omnivoreCount = null;
	public TMP_Text totalSpecimens = null;
	[HideInInspector] public int deaths = 0;
	[SerializeField] TMP_Text deathText = null;
	[HideInInspector] public int deathsFromStarvation = 0;
	[HideInInspector] public int deathsFromBeingEaten = 0;
	[HideInInspector] public int deathsFromOldAge = 0;
	[HideInInspector] public int deathsFromCounter = 0;
	[SerializeField] TMP_Text deathsFromStarvationText = null;
	[SerializeField] TMP_Text deathsFromBeingEatenText = null;
	[SerializeField] TMP_Text deathsFromOldAgeText = null;
	[SerializeField] TMP_Text deathsFromCounterText = null;
	[SerializeField] TMP_Text mutationsText = null;
	[HideInInspector] public int mutations;
	//public float averageHunger = 0;
	List<Specimen> specimens = new List<Specimen>();
	List<Specimen> herbivores = new List<Specimen>();
	List<Specimen> carnivores = new List<Specimen>();
	List<Specimen> omnivores = new List<Specimen>();

	private void Awake()
	{
		WorldBounds = bounds;
		instance = this;
	}
	void Start()
	{
		nXBoarder.transform.position = new Vector3(-WorldBounds,0,0);
		nYBoarder.transform.position = new Vector3(0,0,-WorldBounds);
		pXBoarder.transform.position = new Vector3(WorldBounds,0,0);
		pYBoarder.transform.position = new Vector3(0,0,WorldBounds);
		PopulateSpecimens(herbivorePrefab, herbivoreParent, herbivoreStartSpawnCount);
		PopulateSpecimens(carnivorePrefab, carnivoreParent, carnivoreStartSpawnCount);
		PopulateSpecimens(omnivorePrefab, omnivoreParent, omnivoreStartSpawnCount);
		PopulateSpecimens(treePrefab, treeParent, treeCount);
		StartCoroutine(SpawnPlants());
	}

	private void Update()
	{
		deathsFromStarvationText.text = "Deaths from Starvation: " + deathsFromStarvation;
		deathsFromBeingEatenText.text = "Deaths by Being Eaten: " + deathsFromBeingEaten;
		deathsFromOldAgeText.text = "Deaths by Old Age: " + deathsFromOldAge;
		deathsFromCounterText.text = "Deaths by Counter Attack: " + deathsFromCounter;
		deathText.text = "Total Deaths: " + deaths;
		totalSpecimens.text = "Total Specimens: " + specimens.Count;
		herbivoreCount.text = "Herbivores: " + herbivores.Count;
		carnivoreCount.text = "Carnivores: " + carnivores.Count;
		omnivoreCount.text = "Omnivores: " + omnivores.Count;
		mutationsText.text = "Mutations: " + mutations;
		//float total = 0;
		//foreach(Specimen specimen in herbivores) { total += specimen.hunger; }
		//averageHunger = total / herbivoreCount;
	}

	void PopulateSpecimens(GameObject specimenPrefab, Transform parent, int spawnCount)
	{
		for (int i = 0; i < spawnCount; i++)
		{
			float x = Random.Range(-WorldBounds, WorldBounds);
			float z = Random.Range(-WorldBounds, WorldBounds);

			Vector3 spawnPosition = new Vector3(x, 1.5f, z);
			if(specimenPrefab == treePrefab) { spawnPosition.y = 0; }
			Instantiate(specimenPrefab, spawnPosition, new Quaternion()).transform.SetParent(parent);
		}
	}

	public void Register(Specimen specimen) { specimens.Add(specimen); }
	public void Unregister(Specimen specimen) { specimens.Remove(specimen); }
	public void RegisterHerbivore(Specimen specimen) { herbivores.Add(specimen); }
	public void UnregisterHerbivore(Specimen specimen) { herbivores.Remove(specimen); }
	public void RegisterCarnivore(Specimen specimen) { carnivores.Add(specimen); }
	public void UnregisterCarnivore(Specimen specimen) { carnivores.Remove(specimen); }
	public void RegisterOmnivore(Specimen specimen) { omnivores.Add(specimen); }
	public void UnregisterOmnivore(Specimen specimen) { omnivores.Remove(specimen); }

	public void GiveBirth(Specimen parent, SpecimenGenes mother, SpecimenGenes father)
	{
		bool[] genes = new bool[13];
		for(int i = 0; i < 12; i++){ genes[i] = RandomBool(); }
		SpecimenGenes childGenes = new SpecimenGenes();

		if (genes[0]) { childGenes.movementSpeed = mother.movementSpeed; }
		else { childGenes.movementSpeed = father.movementSpeed; }

		if (genes[1]) { childGenes.detectionRange = mother.detectionRange; }
		else { childGenes.detectionRange = father.detectionRange; }

		if (genes[2]) { childGenes.maxHunger = mother.maxHunger; }
		else { childGenes.maxHunger = father.maxHunger; }

		if (genes[3]) { childGenes.mateThreshold = mother.mateThreshold; }
		else { childGenes.mateThreshold = father.mateThreshold; }

		if (genes[4]) { childGenes.gestationPeriod = mother.gestationPeriod; }
		else { childGenes.gestationPeriod = father.gestationPeriod; }

		if (genes[5]) { childGenes.developementSpeed = mother.developementSpeed; }
		else { childGenes.developementSpeed = father.developementSpeed; }

		if (genes[6]) { childGenes.startingDevelopment = mother.startingDevelopment; }
		else { childGenes.startingDevelopment = father.startingDevelopment; }

		if (genes[7]) { childGenes.attackDamage = mother.attackDamage; }
		else { childGenes.attackDamage = father.attackDamage; }

		if (genes[8]) { childGenes.intimidation = mother.intimidation; }
		else { childGenes.intimidation = father.intimidation; }

		if (genes[9]) { childGenes.lifeSpan = mother.lifeSpan; }
		else { childGenes.lifeSpan = father.lifeSpan; }

		if (genes[10]) { childGenes.healthRegen = mother.healthRegen; }
		else { childGenes.healthRegen = father.healthRegen; }

		if (genes[11]) { childGenes.health = mother.health; }
		else { childGenes.health = father.health; }

		if (genes[12]) { childGenes.litter = mother.litter; }
		else { childGenes.litter = father.litter; }

		Specimen child = Instantiate(parent, parent.transform.position, new Quaternion());
		child.transform.SetParent(herbivoreParent);
		child.Mutate(ref childGenes);
		StartCoroutine(child.Develope(parent.hunger/parent.litter.Stat));
	}

	bool RandomBool()
	{
		int randomBool = Random.Range(0, 2);
		return randomBool == 0;
	}

	IEnumerator SpawnPlants()
	{
		yield return new WaitForEndOfFrame();
		while (herbivores.Count > 0 || carnivores.Count > 0 || omnivores.Count > 0)
		{
			yield return new WaitForSeconds(spawnRate);
			float x = Random.Range(-WorldBounds, WorldBounds);
			float z = Random.Range(-WorldBounds, WorldBounds);

			Vector3 spawnPosition = new Vector3(x, 0.5f, z);
			Instantiate(plantPrefab, spawnPosition, new Quaternion()).transform.SetParent(plantParent);
		}
	}
}

public struct SpecimenGenes
{
	public MovementSpeed movementSpeed;
	public DetectionRange detectionRange;
	public MaxHunger maxHunger;
	public FindMateThreshold mateThreshold;
	public GestationPeriod gestationPeriod;
	public StartingDevelopment startingDevelopment;
	public DevelopementSpeed developementSpeed;
	public Intimidation intimidation;
	public LifeSpan lifeSpan;
	public AttackDamage attackDamage;
	public Health health;
	public HealthRegen healthRegen;
	public LitterSize litter;
}