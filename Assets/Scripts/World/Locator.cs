using UnityEngine;
using System.Collections;

public class Locator : MonoBehaviour
{
	public const float REFRESH_FREQUENCY = 1f;
	
	// valeur indiquant à la matrice si nous devons etre repositioné
	private float refreshPosition = -1f;
	
	// position dans la MATRIX
	public int mX = 0;
	public int mY = 0;
	public int mZ = 0;
	
	private void Start()
	{
		// find managerLocator
		
	}/*Start()*/
	
	
	public bool Refresh()
	{
		if(refreshPosition < 0f)
		{
			//yup !
			refreshPosition = REFRESH_FREQUENCY;
			
			return true;
		}
		else
		{
			refreshPosition -= Time.deltaTime;
			
			return false;
		}
		
	}/*Refresh()*/
	
	
	public void SetPosition(int x, int y, int z)
	{
		mX = x;
		mY = y;
		mZ = z;
		
	}/*SetPosition()*/
	
}/*Locator : MonoBehaviour*/
