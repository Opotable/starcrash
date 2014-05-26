using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	private static GameManager gameManager;
	public static GameManager Instance
	{
		get
		{
			return gameManager;
		}
	}
	
	public float gameSpeed = 1f;
	
	public bool isDebug = true;
	
	
	private void Awake()
	{
		gameManager = this;
		
	}/*Awake()*/
	
	
	private void Update()
	{
		ComputeGameManager();
		
	}/*Update()*/
	
	
	private void ComputeGameManager()
	{
		// game speed
		gameSpeed = Mathf.Clamp01(gameSpeed);
//		Time.timeScale = Mathf.Lerp(Time.timeScale, gameSpeed, Time.deltaTime * 30f);
		Time.timeScale = gameSpeed;
		
	}/*ComputeGameManager()*/
	
} /*GameManager : MonoBehaviour*/
