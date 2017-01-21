using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : DisposableSingleton<CharacterStats>
{
	[Range(0,10)]  public float Health;
	[Range(0,100)] public float WeaponEnergy;

	public void UpdateHealth(float delta)
	{
		
	}
}
