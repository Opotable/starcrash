using UnityEngine;
using System.Collections;

public class SkinMotor : MonoBehaviour
{
	private Vector3 trailOffset = Vector3.forward;
	
	private Transform tBody;
	
	private void Update()
	{
		if(tBody != null)
		{
			transform.LookAt(transform.position - tBody.forward);
		}
		
	}/*Update()*/
	
	
	public void SetShip(Transform tShip)
	{
		tBody = tShip;
		
		AddTrail();
		
	}/*SetShip*/
	
	
	private void AddTrail()
	{
		if(PrefabsSkinCollection.Instance != null && PrefabsSkinCollection.Instance.MotorTrail != null)
		{
			GameObject goTrail = PrefabsSkinCollection.Instance.MotorTrail.Get(Random.value);
			
			if(goTrail != null)
			{
				goTrail = (GameObject)Instantiate(goTrail, transform.position, Quaternion.identity);
				goTrail.transform.parent = transform;
				goTrail.transform.localPosition = trailOffset;
				goTrail.transform.localRotation = Quaternion.identity;
				goTrail.transform.localScale = Vector3.one;
				
				// set trail color
				Renderer rend = goTrail.renderer;
				SBShip ship = tBody.GetComponent<SBShip>();
				
				if(rend != null && ship != null && ship.MOTHER != null)
				{
					rend.material = ship.MOTHER.TEAM_MANTLE;
				}
			}
		}
		
	}/*AddTrail()*/
	
}/*SkinMotor : MonoBehaviour*/
