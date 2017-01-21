using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : DisposableSingleton<GameManager> 
{
	private GameObject characterInstance;
	private Vector3 currentCheckPoint;

	public void SetCharacter(GameObject character)
	{
		characterInstance = character;
		currentCheckPoint = characterInstance.transform.position;
	}

	public void SetCheckPoint(Vector3 point) { currentCheckPoint = point; }

	public void RestartGame ()
	{
		characterInstance.transform.position = currentCheckPoint;
	}

	public void EndGame()
	{
		
	}
}
