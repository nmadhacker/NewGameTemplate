using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBehaviour : MonoBehaviour {


    EnemyScript enemy;

	// Use this for initialization
	void Start () {
        enemy = GetComponent<EnemyScript>();



        enemy.chargeSpeed = 0;
        
		
	}
	
	// Update is called once per frame
	void Update () {

        enemy.lookdir = Vector3.right;
    }



    public void StartAttack()
    {
        enemy.chargeSpeed = 10;
    }
}
