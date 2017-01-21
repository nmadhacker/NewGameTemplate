using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveshoot : BaseWeapon {

//	public enum waves {
//		wave1, wave2, wave3,
//	}

	public Transform[] wave;
	[SerializeField] private Transform currentWave;
	private int waveType;
	[SerializeField] private float waveLifeTime = 4.0f;
    public Transform spawnLocation;

    Transform instWave;

	Coroutine createTrailCoroutine;

	void Awake () {
		currentWave = wave [0];
	}

	protected override void StartAnimation ()
	{
		base.StartAnimation ();
		instWave = Instantiate(currentWave, spawnLocation.position, Quaternion.identity);
        instWave.GetComponent<waveScript>().faceRight = GetComponent<Player>().facingRight;
		createTrailCoroutine = StartCoroutine (CreateTrailCoroutine ());
	}

	IEnumerator CreateTrailCoroutine(waveScript instance)
	{
		while (true) 
		{
			instance.createUpdate();
			yield return null;
		}
	}

	protected override void StopAnimation ()
	{
		base.StopAnimation ();
		StopCoroutine (createTrailCoroutine);
		instWave.GetComponent<waveScript>().startMove();
	}

	public override void nextWave()
	{
		waveType++;
		if (waveType >= wave.Length) {
			waveType = 0;
		}
		currentWave = wave[waveType];
	}

	public override void previousWave()
	{
		waveType--;
		if (waveType < 0) {
			waveType = 2;
		}
		currentWave = wave[waveType];
	}

	void countDownWave ()
	{
		waveLifeTime -= Time.deltaTime;
		if (waveLifeTime <= 0) {
			Destroy (this.gameObject);
		}
	}
}