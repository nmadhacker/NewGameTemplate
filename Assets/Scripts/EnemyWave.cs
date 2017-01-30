using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour {


    public bool disapear;
    public float disapearTime;
    Player player;

    waveScript w;

    public int hits = 0;
	// Use this for initialization
	void Start () {

        w = GetComponent<waveScript>();
		
	}
	
	// Update is called once per frame
	void Update () {
        if (disapear)
        {
            disapearTime-=Time.deltaTime;
            w.wLength -= Time.deltaTime/2;

            if(disapearTime <= 0)
            {
                Destroy(gameObject);
            }
        }
		
	}

    void setPlayer(Player p)
    {
        player = p;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().doDamage(1);
        }
            print(hits++);
    }
}
