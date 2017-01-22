using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteOnContactWithWave : ExecuteOnContact 
{
	[SerializeField] int expectedPoints;

	protected override bool IsValid (GameObject obj)
	{
		if (!base.IsValid (obj)) 
		{
			return false;
		}

		var waveScript = obj.GetComponentInChildren<waveScript> ();
		if (waveScript == null || waveScript.Points < expectedPoints) 
		{
			return false;
		}
		return true;
	}
}
