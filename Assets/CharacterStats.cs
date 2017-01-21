using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : DisposableSingleton<CharacterStats>
{
	[Range(0,100)]  public float Health;
	[Range(0,100)] public float Energy;
	[Range(0.1f,100f)] public float EnergyRegenRate;

	public bool UpdateHealth(float delta)
	{
		if ((delta > 0 && Health >= 100) || (delta < 0 && Health <= 0)) 
		{
			return false;
		}

		Health += delta; 

		if (Health < 0) { Health = 0; }
		if (Health > 100) { Health = 100; }

		return true;
	}

	public bool UpdateEnergy(float delta)
	{
		if ((delta > 0 && Energy >= 100) || (delta < 0 && Energy <= 0)) 
		{
			return false;
		}

		Energy += delta; 

		if (Energy < 0) { Energy = 0; }
		if (Energy > 100) { Energy = 100; }

		return true;
	}

	void Update () 
	{
		if (!GunControl.Instance.IsFiring && Energy < 100) {
			EnergyRegen ();
		}

	}

	void EnergyRegen()
	{
		Energy += Time.deltaTime * EnergyRegenRate;
		if (Energy > 100) { Energy = 100; }
	}
}
