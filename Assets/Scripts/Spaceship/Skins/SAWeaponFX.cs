using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SBWeapon), typeof(ParticleSystem))]
public class SAWeaponFX : MonoBehaviour
{
	private const float projectileSpeed = 25f;
	
	private SBWeapon weapon;
	
	private void Start()
	{
		// find weapon
		if(weapon == null) weapon = GetComponent<SBWeapon>();
		
	}/*Start()*/
	
	public void Play(Vector3 targetPos)
	{
		Vector3 velo = targetPos - transform.position;
		
		velo.Normalize();
		
		velo *= projectileSpeed;
		
		particleSystem.Emit(transform.position, velo, 0.1f, 1f, particleSystem.startColor);
		
	}/*Play()*/
	
}/*SAWeaponFX : MonoBehaviour*/
