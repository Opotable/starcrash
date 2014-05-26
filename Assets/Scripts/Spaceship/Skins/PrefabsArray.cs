using UnityEngine;
using System.Collections;

[System.Serializable]
public class PrefabsArray
{
	public GameObject[] Prefabs;
	
	public GameObject Get(int index)
	{
		if(index < 0 || Prefabs == null || index > Prefabs.Length - 1) return null;
		
		return Prefabs[index];
		
	}/*Get()*/
	
	public GameObject Get(float ratio)
	{
		ratio = Mathf.Clamp01(ratio);
		
		if(Prefabs != null)
		{
			ratio *= (float)Prefabs.Length;
		}
		
		return Get(Mathf.FloorToInt(ratio));
		
	}/*Get()*/
	
}/*PrefabsArray : MonoBehaviour*/
