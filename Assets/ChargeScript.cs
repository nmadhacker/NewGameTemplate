using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeScript : MonoBehaviour {

    Vector3 startPos;

	// Use this for initialization
	void Start () {

        startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void reset()
    {
        transform.position = startPos;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().doDamage(1);
        }
    }
}
