using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour 
{
	[Range(0,100)] public int energyCost;
	[Range(0.1f,60f)] public float cooldown;

	bool onCooldown;

	public virtual bool Fire()
	{
		if (onCooldown) 
		{
			return false;
		}
		onCooldown = true;
		StartCoroutine (WaitForCooldownCoroutine ());
		return true;
	}

	IEnumerator WaitForCooldownCoroutine()
	{
		float startTime = Time.timeSinceLevelLoad;
		float endTime = startTime = cooldown;
		while (Time.timeSinceLevelLoad < endTime) 
		{
			yield return null;
		}
		onCooldown = false;
		yield return null; // wait an extra frame
	}
}
