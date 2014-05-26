using UnityEngine;
using System.Collections;

public class SkinWeapon : MonoBehaviour
{
	private SBShip ship;
	private Vector3 targetDir = Vector3.zero;
//	private float targetDot = 0f;
	
	// BLOOP
//	private float bloop = 0f;
	
	public void SetShip(SBShip ship)
	{
		this.ship = ship;
		
	}/*SetShip*/
	
	
	private void Update()
	{
		if(ship != null && ship.Target != null)
		{
			targetDir = ship.Target.transform.position - transform.position;
			targetDir.Normalize();
			
//			targetDot = Vector3.Dot(transform.forward, targetDir);
			
			// < 0 == on passe à travers notre vaiseau
			transform.LookAt(ship.Target.transform.position);
			
			// bloop ?
//			if(bloop > 0f)
//			{
//				bloop -= Time.deltaTime;
//			}
//			else
//			{
//				bloop = 1f;
//			}
		}
		
	}/*Update()*/
	
}/*SkinWeapon : MonoBehaviour*/
