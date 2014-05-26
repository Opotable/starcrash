using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Mothership : MonoBehaviour
{
	// health bonus
	public const float SAFE_ZONE = 6f;
	public const float REGEN_AURA = 1.75f;
	
	public GameObject prefabSpaceship;
	
	private HashSet<SBShip> Fleet;
	
	[SerializeField] private Material TeamMantle;
	public Material TEAM_MANTLE
	{
		get
		{
			return TeamMantle;
		}
	}
	
	// ++==========++
	// || ALGO <G> ||
	// ++==========++
	private List<ShipParams> DeadChildrenReports;
	private ShipParams Father;
	private ShipParams Mother;
	
	private void Awake()
	{
		Vector3 _C = Random.insideUnitSphere;
		
		TeamMantle = new Material(TeamMantle);
		
		TeamMantle.color = new Color(_C.x, _C.y, _C.z, TeamMantle.color.a);
		
	}/*Awake()*/
	
	
	private void Start()
	{
		// start list
//		ListFleet = new List<SBShip>();
		Fleet = new HashSet<SBShip>();
		
		// enlist to mothership manager
		ManagerMothership.Instance.Add(this);
		
	}/*Start()*/
	
	
	// génère une liste de vaisseaux.
	public void Pop(int nbShip)
	{
		for(int i = 0; i < nbShip; i++)
		{
			Vector3 pos = transform.position;
			
			// rng position
//			pos += Random.onUnitSphere * Random.Range(ManagerMothership.PopRangeMin, ManagerMothership.PopRangeMax);
			
			GameObject newShip = (GameObject)Instantiate(prefabSpaceship, pos, Quaternion.identity);
			
			SBShip ship = newShip.GetComponent<SBShip>();
			
//			ListFleet.Add(ship);
			Fleet.Add(ship);
			
			ship.Link(this);
		}
		
	}/*Pop()*/
	
	
	public void Unlink(SBShip ship)
	{
		if(Fleet.Contains(ship))
		{
			// remove from list
			Fleet.Remove(ship);
			
			// inform manager
			if(ManagerMothership.Instance != null)
			{
				ManagerMothership.Instance.RemoveFromWaitingList(this, ship);
			}
		}
		
//		if(ListFleet.Contains(ship))
//		{
//			// remove from list
//			ListFleet.Remove(ship);
//			
//			// inform Manager
//			ManagerMothership.Instance.RemoveFromWaitingList(this, ship);
//		}
		
	}/*Unlink()*/
	
	
	// rajoute un de nos vaisseaux dans la liste d'attente des vaisseaux
	// qui veulent se fight !
	public void InformWaiting(SBShip ship)
	{
		ManagerMothership.Instance.Waiting(this, ship);
		
	}/*InformWaiting()*/
	
}/*Mothership : MonoBehaviour*/
