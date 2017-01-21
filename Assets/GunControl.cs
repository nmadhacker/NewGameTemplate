﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour 
{
	public BaseWeapon currentWeapon;
	[SerializeField] Animator m_Anim;
	[SerializeField] float fireStartDelay;

	public bool IsFiring { get; private set;}

	public void Update()
	{
		if (Input.GetButtonDown ("Fire1") && currentWeapon.Fire()) 
		{
			IsFiring = true;
		}
		if (IsFiring && Input.GetButtonUp ("Fire1")) 
		{
			m_Anim.SetBool ("Fire", false);
			IsFiring = false;
			currentWeapon.Stop ();
		}
	}

	IEnumerator TriggerFireAnimation()
	{
		yield return new WaitForSeconds (fireStartDelay);
		m_Anim.SetBool ("Fire",true);
		yield break;
	}
}
