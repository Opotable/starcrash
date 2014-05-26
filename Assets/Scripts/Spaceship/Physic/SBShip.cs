using UnityEngine;
using System.Collections;

public struct ShipParams
{
	public HealthParams health;
	public MotorParams motor;
	public WeaponParams weapon;
	
}/*ShipParams*/

public class SBShip : SpaceshipBehaviour
{
	public const float BODY_RADIUS = 1.15f;
	
	public const int NB_PARTS_PER_MODULE = 3;
	
	// moteur
	public SBMotor Motor;
	
	// arme
	public SBWeapon Weapon;
	
	// pv
	public SBHealth Health;
	
	// score
	public SBScore Score;
	
	private Mothership mother;
	public Mothership MOTHER
	{
		get
		{
			return mother;
		}
	}
	
	// notre cible
	private SBShip shipTarget;
	public SBShip Target
	{
		get
		{
			return shipTarget;
		}
	}
	
	// notre skin
	public Transform skinRoot;
	
	private bool informNoTarget = false;
	
	
	private void Start()
	{
		Setup();
		
	}/*Start()*/
	
	
	public void Setup()
	{
//		Rng();
		
		SetSkin();
		
		if(Motor != null)
		{
			Motor.Rng();
			Motor.SetSkin();
		}
		
		if(Health != null)
		{
			Health.Rng();
			Health.SetSkin();
		}
		
		if(Weapon != null)
		{
			Weapon.Rng();
			Weapon.SetSkin();
		}
		
	}/*Rng()*/
	
	/*
	private void Rng()
	{
	
		
	}/*Rng()*/
	
	
	private void SetSkin()
	{
		if(PrefabsSkinCollection.Instance != null)
		{
			GameObject goSkin = PrefabsSkinCollection.Instance.Body.Get(Random.value);
			
			if(goSkin != null)
			{
				goSkin = (GameObject)Instantiate(goSkin);
				goSkin.transform.parent = transform;
				goSkin.transform.localPosition = Vector3.zero;
				
				// Add script
				goSkin.AddComponent<SkinBody>();
				
				// root for other component of the ship
				skinRoot = goSkin.transform;
			}
		}
		
	}/*SetSkin()*/
	
	
	public void Link(Mothership mother)
	{
		this.mother = mother;
		
	}/*Link()*/
	
	
	private void Update()
	{
		ComputeSBShip();
		
	}/*Update()*/
	
	
	public override void Process()
	{
		
		
	}/*Process()*/
	
	
	private void ComputeSBShip()
	{
		// ça va changer !
		// envoie une requète afin de demander un dueliste adverse
		if(shipTarget == null && informNoTarget == false && mother != null)
		{
			mother.InformWaiting(this);
			informNoTarget = true;
		}
		
	}/*ComputeSBShip()*/
	
	
	public void SetTarget(SBShip newShipTarget)
	{
		shipTarget = newShipTarget;
		informNoTarget = false;
		
	}/*SetTarget()*/
	
	
	public bool HitTarget(float dmg)
	{
		if(shipTarget != null) return shipTarget.Health.Hurt(dmg);
		else return false;
		
	}/*HitTarget()*/
	
	
	public void Destroy()
	{
		// report back
		if(mother != null) //TODO 
		
		// unlink from mothership
		if(mother != null) mother.Unlink(this);
		
		// destroy
		Destroy(gameObject);
		
	}/*Destroy()*/
	
	
	public void Explode()
	{
		Health.Death();
		
		this.Destroy();
		
	}/*Explode()*/
	
	
	public eHealthState GetHealthState()
	{
		if(Health != null)
		{
			return Health.HEALTH_STATE;
		}
		else
		{
			return eHealthState.NORMAL;
		}
		
	}/*GetHealthState()*/
	
}/*SBShip : MonoBehaviour*/
