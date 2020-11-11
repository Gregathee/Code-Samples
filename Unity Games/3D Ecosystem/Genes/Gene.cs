using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gene : MonoBehaviour
{
	[SerializeField] float stat = 0.5f;
	[SerializeField] float mutateDelta = 0.1f;
	[SerializeField] float upperBound = 1000;
	[SerializeField] float lowerBound = 0.1f;


	public float Stat { get => stat; set => stat = value; }
	float mutateChance = 25;
    public void Mutate()
	{
		float chance = Random.Range(0, 100);
		if(chance < mutateChance)
		{
			Spawner.instance.mutations++;
			stat += Random.Range(-mutateDelta, mutateDelta);
			stat = Mathf.Clamp(stat, lowerBound, upperBound);
		}
	}
}

