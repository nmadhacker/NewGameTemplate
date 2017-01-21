using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnContact : MonoBehaviour 
{
	[SerializeField] UnityEvent eventToTrigger;
	[SerializeField] string tagtoCheck;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag.Equals (tagtoCheck)) 
		{
			eventToTrigger.Invoke ();
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == tagtoCheck) 
		{
			eventToTrigger.Invoke ();
		}
	}
}
