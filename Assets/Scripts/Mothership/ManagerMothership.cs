using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerMothership : MonoBehaviour
{
	// ++===============++
	// || TARGET FINDER ||
	// ++===============++
	public float TargetFinderFrequency = 5f;
	private float rTargetFinderFreq = 0f;
	
	// ++==================++
	// || TIMER SHIP SPAWN ||
	// ++==================++
	public Vector2 ShipTimerSpawnMinMax = new Vector2(3f, 5f);
	private float currentTimer = 0f;
	private float rTimer = 1f;
	public int NbShipPerDrop = 1;
	
	// ++==========++
	// || AGLO <G> ||
	// ++==========++
	[SerializeField] private int NbChildrenPerGeneration = 10;
	public static int NB_CHILDREN_PER_GENERATION
	{
		get
		{
			int nb = -1;
			
			if(managerMothership != null)
			{
				nb = managerMothership.NbChildrenPerGeneration;
			}
			
			return nb;
		}
	}
	
	// mutation rate
	[SerializeField] private float MutationRate = 0.025f;
	public float MUTATION_RATE
	{
		get
		{
			float muta = 0f;
			
			if(managerMothership != null)
			{
				muta = managerMothership.MutationRate;
			}
			
			return muta;
		}
	}
	
	private static ManagerMothership managerMothership;
	public static ManagerMothership Instance
	{
		get
		{
			return managerMothership;
		}
	}
	
	// la clée est le vaisseau mère
	// la liste représente les vaisseaux en attente d'ordre
	private Dictionary<Mothership, List<SBShip>> Nexus;
	
	
	private void Awake()
	{
		managerMothership = this;
		
		Nexus = new Dictionary<Mothership, List<SBShip>>();
		
		WelcomeMisterFreeman();
		
		currentTimer = Random.Range(ShipTimerSpawnMinMax.x, ShipTimerSpawnMinMax.y);
		
	}/*Awake()*/
	
	
	private void Update()
	{
		ComputeMotherManager();
		
	}/*Update()*/
	
	
	private void ComputeMotherManager()
	{
		SpawnChildren();
		
		// target finder ?
		rTargetFinderFreq += Time.deltaTime / TargetFinderFrequency;
		
		if(rTargetFinderFreq > 1f)
		{
			CheckWatingList();
			rTargetFinderFreq = 0f;
		}
		
	}/*ComputeMotherManager()*/
	
	
	private void SpawnChildren()
	{
		// timer pop
		rTimer += Time.deltaTime / currentTimer;
		
		if(rTimer > 1f)
		{
			// pop !
			foreach(Mothership mother in Nexus.Keys)
			{
				if(mother != null) mother.Pop(NbShipPerDrop);
			}
			
			// reset timer
			rTimer = 0f;
			
			// reset rng
			currentTimer = Random.Range(ShipTimerSpawnMinMax.x, ShipTimerSpawnMinMax.y);
		}
		
	}/*SpawnChildren()*/
	
	
	// récupère un nouveau rapport
	public void AddReport(ShipParams shipParam)
	{
		
		
	}/*AddReport()*/
	
	
	// rajoute un mothership à la liste
	public void Add(Mothership newMother)
	{
		if(!Nexus.ContainsKey(newMother)) Nexus[newMother] = new List<SBShip>();
		
	}/*Add()*/
	
	
	// rajoute un vaisseau dans la liste d'attente d'ordre
	public void Waiting(Mothership mother, SBShip ship)
	{
		if(!Nexus.ContainsKey(mother)) Nexus[mother] = new List<SBShip>();
		
		if(!Nexus[mother].Contains(ship)) Nexus[mother].Add(ship);
		
	}/*Waiting()*/
	
	
	private void CheckWatingList()
	{
		//*
		SBShip firstShip = null;
		SBShip secondShip = null;
		
		foreach(List<SBShip> LShips in Nexus.Values)
		{
			if(firstShip != null && secondShip != null) continue;
			
			if(LShips != null && LShips.Count > 0)
			{
				for(int i = 0; i < LShips.Count; i++)
				{
					if(LShips[i] != null)
					{
						// on trouve le premier vaisseau
						if(firstShip == null)
						{
							firstShip = LShips[i];
						}// sinon on trouve le second vaisseau
						else if(secondShip == null && LShips[i].MOTHER != firstShip.MOTHER)
						{
							secondShip = LShips[i];
						}
					}
					
				}//for()
			}
			
		}//foreach()
		
		if(firstShip != null && secondShip != null)
		{
			// set targets
			firstShip.SetTarget(secondShip);
			secondShip.SetTarget(firstShip);
			
			// remove them from list
			Nexus[firstShip.MOTHER].Remove(firstShip);
			Nexus[secondShip.MOTHER].Remove(secondShip);
		}
		//*/
		
	}/*CheckWatingList()*/
	
	
	// retire un vaisseau d'une liste d'attente
	public void RemoveFromWaitingList(Mothership mother, SBShip ship)
	{
		if(Nexus.ContainsKey(mother))
		{
			if(Nexus[mother] != null)
			{
				Nexus[mother].Remove(ship);
			}
		}
		
	}/*RemoveFromWaitingList()*/
	
	
	private void WelcomeMisterFreeman()
	{
		
	}/*WelcomeMisterFreeman()*/
	
	
	private IEnumerator INeverAskForThis()
	{
//		for(int i = 0; i < MATRIX.Length; i++)
//		{
//			for(int j = 0; j < max; j++)
//			{
//				for(int k = 0; k < max; k++)
//				{
//					
//					
//				}//for()
//			}
//		}
		
		yield return null;
		
	}/*INeverAskForThis()*/
	
}/*ManagerMothership : MonoBehaviour*/
