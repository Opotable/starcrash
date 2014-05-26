using UnityEngine;
using System.Collections;

public class PivotLight : MonoBehaviour
{
	public float speedRotation = 1f;
	
	private void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime * speedRotation, Space.Self);
		
	}/*Update*/
	
}/*PivotLight*/