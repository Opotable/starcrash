using UnityEngine;
using System.Collections;

public class PrefabsSkinCollection : MonoBehaviour
{
	// Singleton
	private static PrefabsSkinCollection prefabSCol;
	public static PrefabsSkinCollection Instance
	{
		get
		{
			return prefabSCol;
		}
	}
	
	private void Awake()
	{
		prefabSCol = this;
		
	}/*Awake()*/
	
	// ship : body
	public PrefabsArray Body;
	
	// link
	public PrefabsArray Link;
	
	// motor
	public PrefabsArray MotorMvt;
	public PrefabsArray MotorAcc;
	
	// motor trail
	public PrefabsArray MotorTrail;
	
	// health
	public PrefabsArray Health;
	
	public int HealthLength
	{
		get
		{
			int nbSkin = 0;
			
			if(Health != null)
				if(Health.Prefabs != null)
					nbSkin = Health.Prefabs.Length;
			
			return nbSkin;
		}
	}
	
	// weapon
	public PrefabsArray Weapon;
	
	public int WeaponLength
	{
		get
		{
			int nbSkin = 0;
			
			if(Weapon != null)
				if(Weapon.Prefabs != null)
					nbSkin = Weapon.Prefabs.Length;
			
			return nbSkin;
		}
	}
	
}/*PrefabsSkinCollection : MonoBehaviour*/
