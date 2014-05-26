using UnityEngine;
using System.Collections;

public struct WeaponParams
{
	public float Cadence;
	public float Range;
	public float Falloff;
	public float Damage;
}

public class SBWeapon : SpaceshipBehaviour
{
	// notre vaisseau
	public SBShip ship;
	
	public const int NB_WEAPON_MAX = 4;
	
	// la vitesse de tir
	public Vector2 CadenceMinMax = new Vector3(5f, 0.1f);
	private float currentCadence = 0f;
	private float rReload = 1f;
	
	// la portée optimal
	public Vector2 RangeMinMax = new Vector2(1.75f, 2.5f);
	private float currentRange = 0f;
	
	// la zone ou tu peux toucher mais les dommages vont de 100% à 0%
	// et/ou font des jets de dés.
	// (zone de transition)
	// cette zone s'additione avec la portée
	public Vector2 FalloffMinMax = new Vector2(2f, 4f);
	private float currentFalloff = 0f;
	
	// les dommages de l'arme
	public Vector2 DamageMinMax = new Vector2(8f, 12f);
	private float currentDamage = 0f;
	
	// skins
	public GameObject[] PrefabsSkinWeapon;
	
	// fx
	public SAWeaponFX fx;
	
	// DEBUG
	// falloff range
	private Color _C_dTarget_for = new Color(0.9f, 0.6f, 0.2f, 0.75f);
	// inrange
	private Color _C_dTarget_ir = new Color(0.9f, 0.9f, 0.2f, 0.75f);
	
	/*
	private void Start()
	{
		Rng();
		
	}/*Start()*/
	
	/*
	public void Setup()
	{
//		Rng();
		
//		SetSkin();
		
	}/*Setup()*/
	
	
	public void Rng()
	{
		currentCadence = Random.Range(CadenceMinMax.x, CadenceMinMax.y);
		currentRange = Random.Range(RangeMinMax.x, RangeMinMax.y);
		currentFalloff = Random.Range(FalloffMinMax.x, FalloffMinMax.y);
		currentDamage = Random.Range(DamageMinMax.x, DamageMinMax.y);
		
	}/*Rng()*/
	
	
	public void SetSkin()
	{
		if(PrefabsSkinCollection.Instance != null && PrefabsSkinCollection.Instance.Weapon != null)
		{
			float rStartScore = GetStarterScoreRatio();
			
			// set weapon
			GameObject goSkin = PrefabsSkinCollection.Instance.Weapon.Get(rStartScore);
			
			if(goSkin != null)
			{
				goSkin = (GameObject)Instantiate(goSkin);
				goSkin.transform.parent = ship.skinRoot;
				goSkin.transform.localPosition = Random.onUnitSphere * SBShip.BODY_RADIUS;
				goSkin.transform.LookAt(goSkin.transform.parent.TransformPoint(goSkin.transform.localPosition * 2f));
				goSkin.transform.localScale = Vector3.one;
				
				// Add script to weapon
				goSkin.AddComponent<SkinWeapon>().SetShip(ship);
				
				// set link
				if(PrefabsSkinCollection.Instance.Link != null)
				{
					GameObject goLink = PrefabsSkinCollection.Instance.Link.Get(Random.value);
					if(goLink != null)
					{
						goLink = (GameObject)Instantiate(goLink, goSkin.transform.position, goSkin.transform.rotation);
						goLink.transform.parent = goSkin.transform;
						goLink.transform.localScale = Vector3.one;
					}
				}
			}
		}
		
	}/*SetSkin()*/
	
	
	private void Update()
	{
		ComputeWeapon();
		
	}/*Update()*/
	
	
	public override void Process()
	{
		
		
	}/*Process()*/
	
	
	private void ComputeWeapon()
	{
		// target ?
		if(ship != null && ship.Target != null && ship.Target.transform != null)
		{
			// In range ?
			float dist = Vector3.Distance(transform.position, ship.Target.transform.position);
			
			if(dist < currentRange + currentFalloff)
			{
				// reload
				rReload += Time.deltaTime / currentCadence;
				
				// shoot ?
				if(rReload > 1f)
				{
					// shoot ?
					float dmg = ComputeDamage(dist);
					
					// shoot && IsDead ?
					if(ship.HitTarget(dmg))
					{
						// score
						if(ship != null && ship.Score != null)
						{
							ship.Score.Add(SBScore.PP_KILL);
						}
					}
					
					// add score ?
					if(dmg > 0f)
					{
						if(ship.Score != null)
						{
							ship.Score.Add(dmg * SBScore.PP_HIT);
						}
					}
					
					// fx ?
					if(fx != null)
					{
						// hit ?
						if(dmg > 0f)
						{
							fx.Play(ship.Target.transform.position);
						}
						else
						{
							fx.Play(ship.Target.transform.position + Random.onUnitSphere * 4f);
						}
					}
					
					// reload
					rReload = 0f;
				}
			}
		}
		
	}/*ComputeWeapon()*/
	
	
	private float ComputeDamage(float dist)
	{
		// out of range ? off
		float dmg = 0f;
		
		if(dist < currentRange)
		{
			// 100%
			dmg = 1f;
		}
		else if (dist < currentRange + currentFalloff)
		{
			// set chance of hit + dmg on distance
			dmg = (dist - currentRange) / currentFalloff;
			
			if(Random.value < dmg)
			{
				// no hit !
				dmg = 0f;
			}
		}
		
		dmg *= currentDamage;
		
		return dmg;
		
	}/*ComputeDamage()*/
	
	
	private float GetStarterScoreRatio()
	{
		float ratio = 0f;
		
		// cadence
		ratio += Mathf.InverseLerp(CadenceMinMax.x, CadenceMinMax.y, currentCadence);
		
		// range
		ratio += Mathf.InverseLerp(RangeMinMax.x, RangeMinMax.y, currentRange);
		
		// falloff
		ratio += Mathf.InverseLerp(FalloffMinMax.x, FalloffMinMax.y, currentFalloff);
		
		// damage
		ratio += Mathf.InverseLerp(DamageMinMax.x, DamageMinMax.y, currentDamage);
		
		// split
		ratio *= 0.25f;
		
		return ratio;
		
	}/*GetStarterScore()*/
	
	
	private void OnDrawGizmos()
	{
		if(GameManager.Instance != null && GameManager.Instance.isDebug == false) return;
		
		if(ship != null && ship.Target != null && ship.Target.transform != null)
		{
			Vector3 dir = ship.Target.transform.position - transform.position;
			dir.Normalize();
			
			// falloff
			Gizmos.color = _C_dTarget_for;
			Gizmos.DrawRay(transform.position + dir * currentRange, dir * currentFalloff);
			
			// range
			Gizmos.color = _C_dTarget_ir;
			Gizmos.DrawRay(transform.position, dir * currentRange);
		}
		
	}/*OnDrawGizmosSelected()*/
	
}/*SBWeapon : MonoBehaviour*/
