using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnContact : MonoBehaviour 
{
	[SerializeField] UnityEvent eventToTrigger;
	[SerializeField] string tagtoCheck;

	protected virtual bool IsValid(GameObject obj)
	{
		return obj.tag.Equals (tagtoCheck);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (IsValid(collider.gameObject)) 
		{
			eventToTrigger.Invoke ();

			print ("triggerExe");
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (IsValid(collision.gameObject)) 
		{
			eventToTrigger.Invoke ();
		}
	}
}
