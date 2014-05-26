using UnityEngine;
using System.Collections;

public struct HealthParams
{
	public float Health;
	public float HealthRegen;
	public float DelayHealthRegen;
}

public enum eHealthState
{
	NORMAL = 0,
	CRITICAL = 1
}

public class SBHealth : SpaceshipBehaviour
{
	private eHealthState healthState = eHealthState.NORMAL;
	public eHealthState HEALTH_STATE
	{
		get
		{
			return healthState;
		}
	}
	
	// notre vaisseau
	public SBShip ship;
	
	// pv
	public Vector2 HealthMinMax = new Vector2(90f, 110f);
	private float currentHealth = 0f;
	private float rHealth = 1f;
	public float HEALTH
	{
		get
		{
			return rHealth * currentHealth;
		}
	}
	
	public float RATIO_HEALTH
	{
		get
		{
			return Mathf.InverseLerp(HealthMinMax.x, HealthMinMax.y, currentHealth);
		}
	}
	
	// pv regen
	public Vector2 HealthRegenMinMax = new Vector2(8f, 12f);
	private float currentHealthRegen = 0f;
	
	public float RATIO_HEALTH_REGEN
	{
		get
		{
			return Mathf.InverseLerp(HealthRegenMinMax.x, HealthRegenMinMax.y, currentHealthRegen);
		}
	}
	
	// regen pv delay
	public Vector2 DelayHealthRegenMinMax = new Vector2(2f, 4f);
	private float currentDelayHealthRegen = 0f;
	private float rDelayHealthRegen = 1f;
	
	public float RATIO_HEALTH_REGEN_DELAY
	{
		get
		{
			return Mathf.InverseLerp(DelayHealthRegenMinMax.x, DelayHealthRegenMinMax.y, currentDelayHealthRegen);
		}
	}
	
	public int NB_SKIN_HEALTH
	{
		get
		{
			if(PrefabsSkinCollection.Instance != null)
				return PrefabsSkinCollection.Instance.HealthLength;
			else
				return 0;
		}
	}
	
	// Fx death
	public GameObject prefabDeath;
	
	// Fx damaged
	public GameObject prefabDamagedFX;
	private GameObject dmgFX;
	
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
		currentHealth = Random.Range(HealthMinMax.x, HealthMinMax.y);
		currentHealthRegen = Random.Range(HealthRegenMinMax.x, HealthRegenMinMax.y);
		currentDelayHealthRegen = Random.Range(DelayHealthRegenMinMax.x, DelayHealthRegenMinMax.y);
		
	}/*Rng()*/
	
	
	public void SetSkin()
	{
		if(PrefabsSkinCollection.Instance != null && PrefabsSkinCollection.Instance.Health != null)
		{
			 // skin : health
			float rScore = Mathf.InverseLerp(HealthMinMax.x, HealthMinMax.y, currentHealth);
			
			// 1/3
			GameObject goSkin = PrefabsSkinCollection.Instance.Health.Get(Random.value * rScore);
			
			if(goSkin != null)
			{
				goSkin = (GameObject)Instantiate(goSkin);
				goSkin.transform.parent = ship.skinRoot;
				goSkin.transform.localPosition = Random.onUnitSphere * SBShip.BODY_RADIUS;
				goSkin.transform.LookAt(goSkin.transform.parent.TransformPoint(goSkin.transform.localPosition * 2f));
				
				// add script
//				goSkin.AddComponent<SkinHealth>();
			}
				
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
		
		// set skin damaged
		if(prefabDamagedFX != null)
		{
			dmgFX = (GameObject)Instantiate(prefabDamagedFX);
			dmgFX.transform.parent = ship.skinRoot;
			dmgFX.transform.localPosition = Vector3.zero;
			dmgFX.transform.localRotation = Quaternion.identity;
			dmgFX.transform.localScale = Vector3.one;
		}
		
	}/*SetSkin()*/
	
	
	private void Update()
	{
		RegenHealth();
		
	}/*Update()*/
	
	public override void Process()
	{
		
		
	}/*Process()*/
	
	
	private void RegenHealth()
	{
		// mothership bonus ?
		if(ship.MOTHER != null && Vector3.Distance(transform.position, ship.MOTHER.transform.position) < Mothership.SAFE_ZONE)
		{
			rHealth += Time.deltaTime / currentHealthRegen / currentHealth * Mothership.REGEN_AURA;
		}
		else if(rDelayHealthRegen > 1f)
		{
			// heal
			rHealth += Time.deltaTime / currentHealthRegen / currentHealth;
			
			// clamp
			rHealth = Mathf.Min(rHealth, 1f);
		}
		else
		{
			// wait
			rDelayHealthRegen += Time.deltaTime / currentDelayHealthRegen;
		}
		
		if(rHealth < 0.45f)
		{
			// health critical !
			healthState = eHealthState.CRITICAL;
			
			// fx ?
			if(dmgFX != null) dmgFX.SetActive(true);
		}
		else if (rHealth > 0.65f)
		{
			healthState = eHealthState.NORMAL;
			
			// fx ?
			if(dmgFX != null) dmgFX.SetActive(false);
		}
		
	}/*RegenHealth()*/
	
	
	public bool Hurt(float damage)
	{
		// remove hp
		rHealth -= damage / currentHealth;
		
		// kill health regen
		rDelayHealthRegen = 0f;
		
		// dead ?
		if(rHealth < 0f)
		{
			Death();
			
			return true;
		}
		else
		{
			return false;
		}
		
	}/*Hurt()*/
	
	
	public void Death()
	{
		rHealth = -1f;
		
		// add death animation
		if(prefabDeath != null)
		{
			GameObject goDeathFX = (GameObject)Instantiate(prefabDeath, transform.position, Random.rotation);
			
			Destroy(goDeathFX, 2f);
		}
		
		// call main
		if(ship != null) ship.Destroy();
		
	}/*Death()*/
	
}/*SBHealth : MonoBehaviour*/
