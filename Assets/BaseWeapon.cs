using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour 
{
	[Range(0,100)] public int energyCost;
	[Range(0.1f,60f)] public float cooldown;

	bool onCooldown;
	public bool IsFiring{ get; private set; }

	public virtual bool Fire()
	{
		if (onCooldown || IsFiring) 
		{
			return false;
		}
		IsFiring = true;
		return IsFiring;
	}

	public virtual bool Stop()
	{
		if (!IsFiring) 
		{
			return false;
		}

		onCooldown = true;
		IsFiring = false;
		StartCoroutine (WaitForCooldownCoroutine ());
		return IsFiring;
	}

	IEnumerator WaitForCooldownCoroutine()
	{
		yield return new WaitForSeconds (cooldown);
		onCooldown = false;
		yield return null; // wait an extra frame
	}
}
