using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneDetector : MonoBehaviour 
{
	void Awake()
	{
		GameManager.Instance.SetCharacter (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("DeathZone"))
		{
			GameManager.Instance.RestartGame ();
		}
	}
}
