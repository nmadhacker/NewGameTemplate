﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseManager : Singleton <WwiseManager> {

	void Awake () {
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
