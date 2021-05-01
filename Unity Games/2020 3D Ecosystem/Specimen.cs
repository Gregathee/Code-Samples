using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Specimen : MonoBehaviour
{
    [SerializeField] float timeToChangeDirection = 1;
    [SerializeField] Material maleColor = null;
    [SerializeField] Material femaleColor = null;
    [SerializeField] MeshRenderer[] body = null;
    [SerializeField] GameObject mouth = null;
    public MovementSpeed movementSpeed = null;
    public DetectionRange detectionRange = null;
    public MaxHunger maxHunger = null;
    public FindMateThreshold mateThreshold = null;
    public GestationPeriod gestationPeriod = null;
    public StartingDevelopment startingDevelopment = null;
    public DevelopementSpeed developementSpeed = null;
    public Intimidation intimidation = null;
    public LifeSpan lifeSpan = null;
    public AttackDamage attackDamage = null;
    public Health health = null;
    public HealthRegen healthRegen= null;
    public LitterSize litter = null;
    Transform target;
     bool findMate;
    public float hunger = 10f;
    bool isHungry = false;
    float development = 1f;
    public float currentHealth = 10;
    Vector3 moveDirection = Vector3.zero;
    bool female = false;
    bool isPregnate = false;
    bool isAdult = true;
    bool mateFound = false;
    public bool dead = false;
    SpecimenGenes fathersGenes;
    Animator animator;
    Collider[] mates = null;
    Collider[] food = null;

    public bool Female { get => female; }

	void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth += health.Stat;
        Spawner.instance.Register(this);
		if (CompareTag("Carnivore")) { Spawner.instance.RegisterCarnivore(this); }
		if (CompareTag("Herbivore")) { Spawner.instance.RegisterHerbivore(this); }
        if (CompareTag("Omnivore")) { Spawner.instance.RegisterOmnivore(this); }
        int isMale = Random.Range(0, 2);
        female = isMale == 0;
        foreach (MeshRenderer bodyPart in body)
        {
            if (female) { bodyPart.material = femaleColor; }
            else { bodyPart.material = maleColor; }
        }
        hunger = maxHunger.Stat;
        StartCoroutine(ChangeDirection());
        StartCoroutine(DrainHunger());
        StartCoroutine(RegenHealth());
    }

    void Update()
    {
        float threshold = maxHunger.Stat * mateThreshold.Stat;
        findMate = hunger > threshold && !isPregnate && isAdult;
        if(transform.position.x > Spawner.WorldBounds) { transform.position = new Vector3(Spawner.WorldBounds, transform.position.y, transform.position.z);}
        if (transform.position.x < -Spawner.WorldBounds) { transform.position = new Vector3(-Spawner.WorldBounds, transform.position.y, transform.position.z); }
        if (transform.position.z > Spawner.WorldBounds) { transform.position = new Vector3(transform.position.x, transform.position.y, Spawner.WorldBounds); }
        if(transform.position.z < -Spawner.WorldBounds) { transform.position = new Vector3(transform.position.x, transform.position.y, -Spawner.WorldBounds); }
        target = null;
        FindTarget();
        Move();
        CheckCollision();
    }

    public void Mutate(ref SpecimenGenes genes)
	{
        movementSpeed = genes.movementSpeed;
        detectionRange = genes.detectionRange;
        maxHunger = genes.maxHunger;
        mateThreshold = genes.mateThreshold;
        gestationPeriod = genes.gestationPeriod;
        startingDevelopment = genes.startingDevelopment;
        developementSpeed = genes.developementSpeed;
        intimidation = genes.intimidation;
        lifeSpan = genes.lifeSpan;
        attackDamage = genes.attackDamage;
        health = genes.health;
        healthRegen = genes.healthRegen;
        litter = genes.litter;

        movementSpeed.Mutate();
       detectionRange.Mutate();
        maxHunger.Mutate();
        mateThreshold.Mutate();
        gestationPeriod.Mutate();
        startingDevelopment.Mutate();
        developementSpeed.Mutate();
        intimidation.Mutate();
        lifeSpan.Mutate();
        attackDamage.Mutate();
        health.Mutate();
        healthRegen.Mutate();
        litter.Mutate();
    }

    public SpecimenGenes GetGenes()
	{
        SpecimenGenes genes = new SpecimenGenes();
        genes.movementSpeed = movementSpeed;
        genes.detectionRange = detectionRange;
        genes.maxHunger = maxHunger;
        genes.mateThreshold = mateThreshold;
        genes.gestationPeriod = gestationPeriod;
        genes.startingDevelopment = startingDevelopment;
        genes.developementSpeed = developementSpeed;
        genes.intimidation = intimidation;
        genes.lifeSpan = lifeSpan;
        genes.attackDamage = attackDamage;
        genes.health = health;
        genes.healthRegen = healthRegen;
        genes.litter = litter;
        return genes;
    }

	void FindTarget()
	{
        mateFound = false;
        target = null;
        food = null;
        mates = null;
        switch (gameObject.tag)
        {
            case "Herbivore":
                if (findMate)
                {
                    mates = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 11);
                    if (mates.Length > 0) { mateFound = FindMate(ref mates); }
                }
                if (!mateFound && isHungry) { food = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 9); }
                break;
            case "Omnivore":
                if (findMate)
                {
                    mates = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 10);
                    if (mates.Length > 0) { mateFound = FindMate(ref mates); }
                }
                if (!mateFound && isHungry) { food = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 9 | 1 << 11); }
                break;
            case "Carnivore":
                if (findMate)
                {
                    mates = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 12);
                    if (mates.Length > 0) { mateFound = FindMate(ref mates); }
                }
                if (!mateFound && isHungry) { food = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 10 | 1 << 11); }
                break;
        }
        if (mates == null ) { if (food != null && isHungry) { if (food.Length > 0) { FindFood(ref food); } } }
        else if (mates.Length == 1 ) { if (food != null && isHungry) { if (food.Length > 0) { FindFood(ref food); } } }
    }

	private void CheckCollision( )
	{
        if (target)
        {
            bool mated = false;
            switch (gameObject.tag)
            {
                case "Herbivore":
                    if (female)
                    {
                        mates = Physics.OverlapSphere(transform.position, 1.5f, 1 << 11);
                        mated = Mate(ref mates);
                    }
                    if (!mated && isHungry) { food = Physics.OverlapSphere(transform.position, 1.5f, 1 << 9); }
                    break;
                case "Omnivore":
                    if (female)
                    {
                        mates = Physics.OverlapSphere(transform.position, 1.5f, 1 << 10);
                        mated = Mate(ref mates);
                    }
                    if (!mated && isHungry) { food = Physics.OverlapSphere(transform.position, 1.5f, 1 << 9 | 1 << 11); }
                    break;
                case "Carnivore":
                    if (female)
                    {
                        mates = Physics.OverlapSphere(transform.position, 1.5f, 1 << 12);
                        mated = Mate(ref mates);
                    }
                    if (!mated && isHungry) { food = Physics.OverlapSphere(transform.position, 1.5f, 1 << 10 | 1 << 11); }
                    break;
            }

            if (food != null && !mated && isHungry) { if (food.Length > 0) { Eat(ref food); } }
        }
    }

    bool Mate(ref Collider[] potentialMates)
    {
        if (findMate && !isPregnate && mateFound)
        {
            foreach (Collider potentialMate in potentialMates)
            {
                if (potentialMate.transform == target)
                {
                    fathersGenes = potentialMate.GetComponent<Specimen>().GetGenes();
                    isPregnate = true;
                    StartCoroutine(Gestation());
                    return true;
                }
            }
        }
        return false;
    }

    void Eat(ref Collider[] foodGroup)
	{
        bool eaten = false;
        foreach (Collider food in foodGroup)
        {
            if (food.transform == target)
            {
                Specimen prey = food.GetComponent<Specimen>();
                if (prey)
                {
                    prey.TakeDamage(attackDamage.Stat, this, true);
                    if (dead) Spawner.instance.deathsFromCounter++;
                    if (prey.dead) { hunger += Spawner.instance.specimenFoodValue; eaten = true; break; }
                }
                else { Destroy(food.gameObject); hunger += Spawner.instance.plantFoodValue; eaten = true; break; }
                if (eaten) { break; }
            }
        }
    }
     
    public void TakeDamage(float damage, Specimen attacker, bool attackerCanBeCountered)
	{
        currentHealth -= damage;
        if (attackerCanBeCountered) { attacker.TakeDamage(attackDamage.Stat, this, false);}
        if (currentHealth < 1)
        {
            Spawner.instance.deathsFromBeingEaten++;
            Die();
        }
        if(!attackerCanBeCountered && !dead && attacker.dead && !gameObject.CompareTag("Herbivore")) 
        { hunger += Spawner.instance.specimenFoodValue; }
    }

    bool FindMate(ref Collider[] potentialMates)
	{
        SortedDictionary<float, Collider> sortedPotentialMates = new SortedDictionary<float, Collider>();
        foreach(Collider potentialMate in potentialMates)
		{
            float distance = Vector3.Distance(transform.position, potentialMate.transform.position);
			if (!sortedPotentialMates.ContainsKey(distance) && potentialMate.gameObject != this) 
            { sortedPotentialMates.Add(distance, potentialMate); }
		}
        foreach (var pair in sortedPotentialMates)
        {
            Specimen potentialMate = pair.Value.GetComponentInParent<Specimen>();
            bool mateIsOpositeSex = female != potentialMate.Female;
            if (potentialMate.findMate && mateIsOpositeSex)
            {
                target = pair.Value.transform;
                return true;
            }
        }
        return false;
    }

    void FindFood(ref Collider[] foodIn )
    {
        SortedDictionary<float, Collider> sortedFood = new SortedDictionary<float, Collider>();
        foreach (Collider food in foodIn)
        {
            if (food)
            {
                float distance = Vector3.Distance(transform.position, food.transform.position);
                if (!sortedFood.ContainsKey(distance)) { sortedFood.Add(distance, food); }
            }
        }
        if (sortedFood.Count > 0)
        {
            if (CompareTag("Omnivore"))
            {
                bool foundPlant = false;
                foreach(var pair in sortedFood)
				{
					if (pair.Value.CompareTag("PlantFood")) { foundPlant = true; target = pair.Value.transform; break; }
				}
				if (!foundPlant) { target = sortedFood.First().Value.transform; }
            }
            else if (CompareTag("Carnivore"))
            {
                bool foundHerbivore = false;
                foreach (var pair in sortedFood)
                {
                    if (pair.Value.CompareTag("Herbivore")) { foundHerbivore = true; target = pair.Value.transform; break; }
                }
                if (!foundHerbivore) { target = sortedFood.First().Value.transform; }
            }
            else { target = sortedFood.First().Value.transform; }
        }
    }

	void Move()
	{
        if (gameObject.CompareTag("Herbivore")) { isHungry = maxHunger.Stat - hunger >= Spawner.instance.plantFoodValue; }
        else { isHungry = maxHunger.Stat - hunger >= Spawner.instance.specimenFoodValue; }
        Collider[] potentialPreditors = null; 

        //Detect potential preditors based on species type
        switch (gameObject.tag)
        {
            case "Herbivore": potentialPreditors = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 10 | 1<<12); break;
            case "Omnivore": potentialPreditors = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1<< 10 | 1 << 12); break;
            case "Carnivore": potentialPreditors = Physics.OverlapSphere(transform.position, detectionRange.Stat, 1 << 10 | 1 << 11); break;
        }
        SortedDictionary<float, Collider> sortedColliders = new SortedDictionary<float, Collider>();
        foreach (Collider collider in potentialPreditors) 
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (!sortedColliders.ContainsKey(distance)) { sortedColliders.Add(distance, collider); }
        }
        Transform preditor = null;
        foreach (var pair in sortedColliders)
        {
            Specimen potentialPreditor = pair.Value.GetComponent<Specimen>();
            if (potentialPreditor)
            {
                if (pair.Value.gameObject != this && potentialPreditor.intimidation.Stat >= intimidation.Stat && !pair.Value.CompareTag(gameObject.tag)) 
                {
                    preditor = pair.Value.transform; break; 
                }
            }
        }
        if (preditor)
        {
            if (preditor.GetComponent<Specimen>().intimidation.Stat != intimidation.Stat)
            {
                moveDirection = -(preditor.position - transform.position).normalized;
                mouth.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if(target == preditor.transform){ target = null; }
        }
        else { mouth.transform.localEulerAngles = Vector3.zero; }

        if (target && (findMate || isHungry))
        {
            Vector3 targetPosition = target.position;
            targetPosition.y = transform.position.y;
            moveDirection = (targetPosition - transform.position).normalized;
        }
        moveDirection.y = 0;
        transform.position = transform.position + (moveDirection * movementSpeed.Stat * Time.deltaTime);
        if (moveDirection != Vector3.zero) 
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection), 2);
            animator.speed = movementSpeed.Stat / 10;
            animator.SetBool("IsInMotion", true);
        }
        else { animator.SetBool("IsInMotion", false); }
    }

    public void Die()
	{
        dead = true;
        Spawner.instance.Unregister(this);
        if (CompareTag("Carnivore")) { Spawner.instance.UnregisterCarnivore(this); }
        if (CompareTag("Herbivore")) { Spawner.instance.UnregisterHerbivore(this); }
        if (CompareTag("Omnivore")) { Spawner.instance.UnregisterOmnivore(this); }
        StopAllCoroutines();
        Spawner.instance.deaths++;
        StartCoroutine(DestoyAtEndOfFrame());
	}

    IEnumerator RegenHealth()
	{
        while(!dead)
		{
            yield return new WaitForSeconds(1);
            currentHealth += healthRegen.Stat;
            if(currentHealth > health.Stat) { currentHealth = health.Stat; }
		}
	}

    IEnumerator DestoyAtEndOfFrame()
	{
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
	}

    public IEnumerator Age()
	{
        yield return new WaitForSeconds(lifeSpan.Stat);
        Spawner.instance.deathsFromOldAge++;
        Die();
	}

    public IEnumerator Develope(float parentHunger)
	{
        hunger = parentHunger;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y / 2, transform.position.z);
        isAdult = false;
        development = startingDevelopment.Stat;
        while(!isAdult && !dead)
		{
            yield return new WaitForSeconds(1f);
            development += developementSpeed.Stat;
            if(development >= 1f) { isAdult = true; }
		}
        if (!dead)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    IEnumerator Gestation()
	{
        yield return new WaitForSeconds(gestationPeriod.Stat);
        isPregnate = false;
        for (int i = 0; i < litter.Stat; i++) { Spawner.instance.GiveBirth(this, GetGenes(), fathersGenes); }
	}

    IEnumerator DrainHunger()
	{
        while (!dead)
        {
            yield return new WaitForSeconds(1f);
            hunger -= movementSpeed.Stat;
			if (isPregnate) { hunger -= movementSpeed.Stat * (litter.Stat); }
            if (hunger <= 0) { Spawner.instance.deathsFromStarvation++; Die(); }
        }
	}

    IEnumerator ChangeDirection()
	{
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        moveDirection = new Vector3(x, 0, z).normalized;
        while (!dead)
        {
            float randomTime = Random.Range(0, timeToChangeDirection);
            if(Random.Range(0,101) < 11) { moveDirection = Vector3.zero; }
            yield return new WaitForSeconds(randomTime);
            x = Random.Range(-1f, 1f);
            z = Random.Range(-1f, 1f);
            moveDirection = new Vector3(x, 0, z).normalized;
        }
	}
}
