using UnityEngine;
using System.Collections;

public class SkinHealth : MonoBehaviour
{
	private const float rotationSpeedMax = 15f;
	private float rot = 0f;
	
	private void Awake()
	{
		rot = Random.Range(-rotationSpeedMax, rotationSpeedMax);
		
	}/*Awake()*/
	
	
	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * rot, Space.Self);
		
	}/*Update()*/
	
}/*SkinHealth : MonoBehaviour*/
