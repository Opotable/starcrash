using UnityEngine;
using System.Collections;

public class TrailTeam : MonoBehaviour
{
	public SBShip ship;
	
	public TrailRenderer trail;
	
	private IEnumerator Start()
	{
		while(ship == null) yield return null;
		
		while(ship.MOTHER == null) yield return null;
		
		trail.sharedMaterial = ship.MOTHER.TEAM_MANTLE;
		
		Destroy(this);
		
	}/*Start()*/
	
} /*TrailTeam : MonoBehaviour*/
