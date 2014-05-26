using UnityEngine;
using System.Collections;

public class SkinBody : MonoBehaviour
{
	private Quaternion fixeRot;
//	private Quaternion addRot;
	
	private void Awake()
	{
		fixeRot = Random.rotation;
		
//		addRot = Quaternion.Euler(Random.rotation.eulerAngles * 0.15f);
		
	}/*Awake()*/
	
	private void Update()
	{
//		fixeRot = Quaternion.RotateTowards(fixeRot, Quaternion.Euler(fixeRot.eulerAngles + addRot.eulerAngles), Time.deltaTime);
		
		transform.rotation = fixeRot;
		
	}/*Update()*/
	
}/*SkinBody : MonoBehaviour*/
