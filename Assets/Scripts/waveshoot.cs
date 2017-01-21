using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveshoot : BaseWeapon {

    public Transform platform;

    public Transform spawnLocation;

    Transform instWave;

	Coroutine createTrailCoroutine;

	protected override void StartAnimation ()
	{
		base.StartAnimation ();
		instWave = Instantiate(platform, spawnLocation.position, Quaternion.identity);
		createTrailCoroutine = StartCoroutine (CreateTrailCoroutine ());
	}

	IEnumerator CreateTrailCoroutine()
	{
		while (true) 
		{
			instWave.GetComponent<waveScript>().createUpdate();
			yield return null;
		}
	}

	protected override void StopAnimation ()
	{
		base.StopAnimation ();
		StopCoroutine (createTrailCoroutine);
		instWave.GetComponent<waveScript>().startMove();
	}
}
