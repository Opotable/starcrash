using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerLocator : MonoBehaviour
{
	// Singleton
	private static ManagerLocator managerLocator;
	public static ManagerLocator Instance
	{
		get
		{
			return managerLocator;
		}
	}
	
	// ++=========++
	// || 3D GRID ||
	// ++=========++
	public const int CELL_SIZE = 10;
	public const int NB_CELL = 10;
	private float MATRIX_OFFSET = 0f;
	private HashSet<Locator>[][][] MATRIX;
	
	
	private void Awake()
	{
		MATRIX_OFFSET = (float)CELL_SIZE;
		MATRIX_OFFSET *= (float)NB_CELL;
		MATRIX_OFFSET *= 0.5f;
		
		managerLocator = this;
		
		Init();
		
	}/*Start()*/
	
	
	private void Init()
	{
		// première dimension
		MATRIX = new HashSet<Locator>[NB_CELL][][];
		
		for(int i = 0; i < NB_CELL; i++)
		{
			// deuxième dimension
			MATRIX[i] = new HashSet<Locator>[NB_CELL][];
			
			for(int j = 0; j < NB_CELL; j++)
			{
				// troisième dimension
				MATRIX[i][j] = new HashSet<Locator>[NB_CELL];
				
				// liste des vaisseaux
				for(int k = 0; k < NB_CELL; k++)
				{
					MATRIX[i][j][k] = new HashSet<Locator>();
					
				}//for()
				
			}//for()
			
		}//for()
		
	}/*Init()*/
	
	
	private IEnumerator Process()
	{
		while(true)
		{
			// première dimension
			for(int i = 0; i < NB_CELL; i++)
			{
				// deuxième dimension
				for(int j = 0; j < NB_CELL; j++)
				{
					// troisième dimension
					for(int k = 0; k < NB_CELL; k++)
					{
						HashSet<Locator> HSL = MATRIX[i][j][k];
						
						if(HSL != null)
						{
							foreach(Locator loc in HSL)
							{
								if(loc.Refresh())
								{
									int x = 0, y = 0, z = 0;
									
									MatrixPos(loc.transform.position, ref x, ref y, ref z);
									
									loc.SetPosition(x, y, z);
								}
							}
						}
						
					}//for()
					
				}//for()
				
				yield return null;
				
			}//for()
			
			yield return null;
			
			Debug.Log("Ping Loc !");
			
		}//while()
		
	}/*Process()*/
	
	
	public void Add(Locator loc)
	{
		
		
	}/*Add()*/
	
	
	private void MatrixPos(Vector3 worldPos, ref int x, ref int y, ref int z)
	{
		x = WorldToMatrix(worldPos.x);
		y = WorldToMatrix(worldPos.y);
		z = WorldToMatrix(worldPos.z);
		
	}/*MatrixPos()*/
	
	
	private int WorldToMatrix(float val)
	{
		// offset
		val += MATRIX_OFFSET;
		
		// scale down to cell size
		val /= (float)CELL_SIZE;
		
		// round up/down
		val = Mathf.Round(val);
		
		// set back to world space
		val *= (float)CELL_SIZE;
		
		return Mathf.RoundToInt(val);
		
	}/*WorldToMatrix()*/
	
}/*ManagerLocator : MonoBehaviour*/
