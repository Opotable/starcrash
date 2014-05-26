using UnityEngine;
using System.Collections;

public class SBScore : MonoBehaviour
{
	// vars
	// motor : per meter
	public const float PP_METER = 0.03f;
	// weapon : per hit
	public const float PP_HIT = 0.1f;
	// weapon : per kill
	public const float PP_KILL = 50f;
	// health : per % healed
	public const float PP_HEALED = 0.2f;
	
	private float Score = 0f;
	public float SCORE
	{
		get
		{
			return Score;
		}
	}
	
	public void Add(float addedValue)
	{
		Score += addedValue;
		
	}/*Add()*/
	
} /*SBScore : MonoBehaviour*/
