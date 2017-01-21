using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveshoot : MonoBehaviour {

    public Transform platform;

    public Transform spawnLocation;

    Transform instWave;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            instWave = Instantiate(platform, spawnLocation.position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            instWave.GetComponent<waveScript>().createUpdate();
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            instWave.GetComponent<waveScript>().startMove();
        }
		
	}
}
