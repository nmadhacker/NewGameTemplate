using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour 
{
	[Range(0,100)] public int energyCost;
	[Range(0.1f, 10f)] public float minFireTime;
	[Range(0.1f,60f)] public float cooldown;

	Coroutine consumeEnergyCoroutine = null;

	bool onCooldown;
	public bool IsFiring{ get; private set; }
	private float fireStartTime;

	public virtual bool Fire()
	{
		if (onCooldown || IsFiring) 
		{
			return false;
		}
		IsFiring = true;
		fireStartTime = Time.timeSinceLevelLoad;
		consumeEnergyCoroutine = StartCoroutine (ConsumeEnergyCoroutine ());
		StartAnimation ();
		return IsFiring;
	}

	protected virtual void StartAnimation() { }

	protected virtual void StopAnimation() { }

	public virtual bool Stop()
	{
		if (!IsFiring) 
		{
			return false;
		}

		float fireTime = Time.timeSinceLevelLoad - fireStartTime;
		if (fireTime < minFireTime) 
		{
			float remainingTime = minFireTime - fireTime;
			CharacterStats.Instance.UpdateEnergy (-energyCost * remainingTime);
			StartCoroutine(WaitAndStopAnimation (remainingTime));
		} 
		else 
		{
			StopAnimation ();
		}

		onCooldown = true;
		IsFiring = false;
		StopCoroutine (consumeEnergyCoroutine);
		StartCoroutine (WaitForCooldownCoroutine ());
		return true;
	}

	IEnumerator WaitAndStopAnimation(float time)
	{
		yield return new WaitForSeconds (time);
		StopAnimation ();
	}

	IEnumerator ConsumeEnergyCoroutine()
	{
		while (IsFiring) 
		{
			if (!CharacterStats.Instance.UpdateEnergy (-energyCost * Time.deltaTime)) 
			{
				Stop ();
				break;
			}

			yield return null;
		}
	}

	IEnumerator WaitForCooldownCoroutine()
	{
		yield return new WaitForSeconds (cooldown);
		onCooldown = false;
		yield return null; // wait an extra frame
	}

	public virtual void nextWave (){
	}
	public virtual void previousWave(){
	}
}
