using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour {

    EnemyScript Enemy;

	// Use this for initialization
	void Start () {
        Enemy = GetComponentInParent<EnemyScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        print("NEWHIT!!!!");
        StartCoroutine(Enemy.hitEffect());
    }
}
