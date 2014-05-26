using UnityEngine;
using System.Collections;

public class SimpleRotateCamera : MonoBehaviour
{
	public float sensibility = 100f;
	
	public float zoomSensibility = 50f;
	
	public Transform PivotX;
	
	public Transform MainCamera;
	
	private void LateUpdate()
	{
		if(Input.GetMouseButton(0))
		{
			Screen.showCursor = false;
			
			transform.Rotate(Vector3.up * sensibility * Time.deltaTime * Input.GetAxis("Mouse X"), Space.World);
			
			if(PivotX != null)
			{
				PivotX.Rotate(Vector3.left * Time.deltaTime * sensibility * Input.GetAxis("Mouse Y"), Space.Self);
			}
		}
		else
		{
			Screen.showCursor = true;
		}
		
		MainCamera.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel") * zoomSensibility, Space.Self);
		
	}/*LateUpdate()*/
	
} /*SimpleRotateCamera : MonoBehaviour*/
