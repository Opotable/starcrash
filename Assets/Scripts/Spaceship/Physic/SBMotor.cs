using UnityEngine;
using System.Collections;

public struct MotorParams
{
	public float Speed;
	public float Acceleration;
	public float AngularSpeed;
	public float SecurityDistance;
	
	public void Setup(float speed, float acceleration, float angularSpeed, float securityDistance)
	{
		Speed = speed;
		Acceleration = acceleration;
		AngularSpeed = angularSpeed;
		SecurityDistance = securityDistance;
		
	}/*Setup()*/
	
}/*MotorParams*/

public class SBMotor : SpaceshipBehaviour
{
	// pour les truc de collisions
	public const float SHIP_SIZE_WARNING = 12f;
	public const float SHIP_SIZE_TOOLATE = 2f;
	
	// notre vaisseau
	public SBShip ship;
	
	// MVT
	
	// vitesse
	public Vector2 MvtSpeedMinMax = new Vector2(6f, 10f);
	private float currentSpeed = 0f;
	private float rSpeed = 0;
	
	// distance de securité lorsque l'on s'éloigne des ennemis
	public Vector2 SecurityDistMinMax = new Vector2(35f, 55f);
	private float currentSecuDist = 0f;
	
	// ACC
	
	// acceleration
	public Vector2 MvtAccMinMax = new Vector2(0.01f, 0.1f);
	private float currentAcc = 0f;
	
	// vitesse de rotation
	public Vector2 AngleSpeedMinMax = new Vector2(35f, 55f);
	private float currentAngleSpeed = 0f;
	
	// ratio vitesse/anglTarget
	// (permet de "freiner" si jamais notre cible est trop derrière nous)
	public AnimationCurve CurveAglTweakSpeed = AnimationCurve.Linear(0f, 0f, 1f, 1f);
	private float rAglTargetTweak = 1f;
	
	// quelques variables qui se font recalculées souvents.
	private Vector3 targetDir = Vector3.zero;
	private float targetDist = 0f;
	private float forwardDist = 0f;
	
	// var donnant un léger coté aléatoire lorsque deux vaisseaux s'esquive (sinon impact >90%)
	private Vector3 rngDir = Vector3.zero;
	
	// skins mvt
	public GameObject[] PrefabsSkinMvt;
	
	// skin acc
	public GameObject[] PrefabsSkinAcc;
	
	/*
	private void Start()
	{
		Rng();
		
		SetSkin();
		
	}/*Start()*/
	
	/*
	public void Setup()
	{
//		Rng();
		
//		SetSkin();
		
	}/*Setup()*/
	
	
	public float Rng()
	{
		float rngScore = 0f;
		
		currentSpeed = Random.Range(MvtSpeedMinMax.x, MvtSpeedMinMax.y);
		currentAcc = Random.Range(MvtAccMinMax.x, MvtAccMinMax.y);
		currentAngleSpeed = Random.Range(AngleSpeedMinMax.x, AngleSpeedMinMax.y) * Mathf.PI / 180.0f;
		currentSecuDist = Random.Range(SecurityDistMinMax.x, SecurityDistMinMax.y);
		
		// rngDir seed (better dodge when close-up).
		rngDir = Random.insideUnitSphere;
		
		// compute score
		rngScore += GetRatioModuleScoreMvt();
		rngScore += GetRatioModuleScoreAcc();
		rngScore *= 0.5f;
		
		return rngScore;
		
	}/*Rng()*/
	
	
	public void SetSkin()
	{
		if(PrefabsSkinCollection.Instance != null && PrefabsSkinCollection.Instance.MotorMvt != null)
		{
			// nb skin
//			int nbSkinMvt = Mathf.FloorToInt(GetRatioModuleScoreMvt() * SBShip.NB_PARTS_PER_MODULE);
				
			// set motorMvt
			float rMvt = GetRatioModuleScoreMvt();
			GameObject goSkin = PrefabsSkinCollection.Instance.MotorMvt.Get(rMvt);
			
			if(goSkin != null)
			{
				goSkin = (GameObject)Instantiate(goSkin);
				goSkin.transform.parent = ship.skinRoot;
				goSkin.transform.localPosition = Random.onUnitSphere * SBShip.BODY_RADIUS;
				goSkin.transform.LookAt(goSkin.transform.parent.TransformPoint(goSkin.transform.localPosition * 2f));
				goSkin.transform.localScale = Vector3.one;
				
				// Add script to motor
				SkinMotor sMotor = goSkin.AddComponent<SkinMotor>();
				sMotor.SetShip(ship.transform);
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
		
	}/*SetSkin()*/
	
	
	public override void Process()
	{
		
		
	}/*Process()*/
	
	
	private void Update()
	{
		ComputeMotor();
		
	}/*Update()*/
	
	
	private void AddScore(float distance)
	{
		if(ship.Score != null)
		{
			ship.Score.Add(distance * SBScore.PP_METER);
		}
		
	}/*AddScore()*/
	
	
	private void ComputeMotor()
	{
		if(ship != null)
		{
			// compute vars
			if(ship.Target == null) targetDist = -1f;
			else targetDist = Vector3.Distance(transform.position, ship.Target.transform.position);
			
			switch(ship.GetHealthState())
			{
				
			case eHealthState.NORMAL:
			{
				AttackMove();
				
			}break;
				
			case eHealthState.CRITICAL:
			{
				FleeingMove();
				
			}break;
				
			default:break;
				
			}//switch()
		}
		
	}/*ComputeMotor()*/
	
	
	private void AttackMove()
	{
		// target ? targetDist < 0 if ship.Target == null
		if(targetDist > 0f)
		{
			// Add Speed
			rSpeed += Time.deltaTime * currentAcc;
			
			// Clamp speed
			rSpeed = Mathf.Clamp01(rSpeed);
			
			// Setup target
			if(targetDist < SHIP_SIZE_TOOLATE)
			{
				// hurt our comrade
				ship.Target.Health.Hurt(ship.Health.HEALTH * 0.35f);
				
				// A BOMB !
				ship.Explode();
				
				return;
			}
			else if(targetDist < SHIP_SIZE_WARNING)
			{
				// move away and toward depending on ours directions,
				// or we are gonna crash !
				
				float awayToward = (targetDist - SHIP_SIZE_TOOLATE) / SHIP_SIZE_WARNING;
				
				awayToward = Mathf.Clamp01(awayToward);
				
				targetDir = Vector3.Lerp(transform.position - ship.Target.transform.position + rngDir, ship.Target.transform.position - transform.position, awayToward);
			}
			else
			{
				// okay
				targetDir = ship.Target.transform.position - ship.Target.transform.forward * 2f - transform.position;
			}
			
			// rotate to target
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, currentAngleSpeed * Time.deltaTime, 0f));
			
			// compute speed tweak
			ComputeAglSpeedTweak(ref rAglTargetTweak);
			
			// translate forward
			forwardDist = rSpeed * currentSpeed * rAglTargetTweak * Time.deltaTime;
			transform.Translate(Vector3.forward * forwardDist, Space.Self);
			
			// add score
			AddScore(forwardDist);
		}
		else
		{
			// slow speed
			rSpeed = Mathf.Lerp(rSpeed, 0.1f, Time.deltaTime);
			
			// rotate in circle
			targetDir = new Vector3(Mathf.Sin(Time.time), 0f, Mathf.Cos(Time.time + 1f));
			
			// rotate
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, currentAngleSpeed * Time.deltaTime, 0f));
			
			// translate
			forwardDist = rSpeed * currentSpeed * Time.deltaTime;
			transform.Translate(Vector3.forward * forwardDist, Space.Self);
			
			// add score
			AddScore(forwardDist);
		}
		
	}/*AttackMove()*/
	
	
	private void ComputeAglSpeedTweak(ref float aglTweak)
	{
		if(ship != null && ship.Target != null)
		{
			Vector3 dir = ship.Target.transform.position - transform.position;
			
			dir.Normalize();
			
			aglTweak = Vector3.Dot(transform.forward, dir) * 0.5f + 0.5f;
			
			aglTweak = CurveAglTweakSpeed.Evaluate(aglTweak);
		}
		
	}/*ComputeAglSpeedTweak()*/
	
	
	private void FleeingMove()
	{
		// target ?
		if(ship.Target != null && targetDist < 0f)
		{
			// Add Speed
			rSpeed += Time.deltaTime * currentAcc;
			
			// Clamp speed
			rSpeed = Mathf.Clamp01(rSpeed);
			
			// safe distance ?
			if(targetDist > currentSecuDist)
			{
				// move toward mothership while revolve around foe
				targetDir = ship.MOTHER.transform.position - transform.position;
				
				// rotate toward mothership
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, currentAngleSpeed * Time.deltaTime, 0f));
			}
			else
			{
				// move away from target + a little bit of mothership
				targetDir = Vector3.Lerp(ship.Target.transform.position, ship.MOTHER.transform.position, 0.15f) - transform.position;
				
				// rotate away from target ! and a bit of mothership
				transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, -currentAngleSpeed * Time.deltaTime, 0f));
			}
			
			// compute angle tweak speed
			ComputeAglSpeedTweak(ref rAglTargetTweak);
			
			// translate forward
			forwardDist = rSpeed * currentSpeed * rAglTargetTweak * Time.deltaTime;
			transform.Translate(Vector3.forward * forwardDist, Space.Self);
			
			// add score
			AddScore(forwardDist);
		}
		else
		{
			// Add Speed
			rSpeed += Time.deltaTime * currentAcc;
			
			// Clamp speed
			rSpeed = Mathf.Clamp01(rSpeed);
			
			// Go near base
			targetDir = ship.MOTHER.transform.position - transform.position;
			
			// rotate
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, currentAngleSpeed * Time.deltaTime, 0f));
			
			// translate
			forwardDist = rSpeed * currentSpeed * Time.deltaTime;
			transform.Translate(Vector3.forward * forwardDist, Space.Self);
			
			// add score
			AddScore(forwardDist);
		}
		
	}/*FleeingMove()*/
	
	
	// calcul la "valeur" de l'ensemble des modules
	// afin de savoir combien de skin on place.
	private float GetRatioModuleScoreMvt()
	{
		float ratio = 0f;
		
		ratio += Mathf.InverseLerp(MvtSpeedMinMax.x, MvtSpeedMinMax.y, currentSpeed);
		
		ratio += Mathf.InverseLerp(AngleSpeedMinMax.x, AngleSpeedMinMax.y, currentAngleSpeed);
		
		ratio *= 0.5f;
		
		return ratio;
		
	}/*GetRatioModuleScoreMvt()*/
	
	// pareil que dessus mais pour l'acceleration
	private float GetRatioModuleScoreAcc()
	{
		float ratio = 0f;
		
		ratio += Mathf.InverseLerp(MvtAccMinMax.x, MvtAccMinMax.y, currentAcc);
		
		ratio += Mathf.InverseLerp(SecurityDistMinMax.x, SecurityDistMinMax.y, currentSecuDist);
		
		ratio *= 0.5f;
		
		return ratio;
		
	}/*GetRatioModuleScoreMvt()*/
	
	
	private void OnDrawGizmos()
	{
		if(GameManager.Instance != null && GameManager.Instance.isDebug == false) return;
		
		if(ship != null && ship.Target != null)
		{
			Vector3 targetDir = ship.Target.transform.position - transform.position;
			targetDir.Normalize();
			
			float dir = Vector3.Dot(transform.forward, targetDir) * 0.5f + 0.5f;
			
			Gizmos.color = Color.Lerp(Color.red, Color.green, dir);
			
			Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
		}
		
	}/*OnDrawGizmos()*/
	
}/*SBMotor : MonoBehaviour*/
