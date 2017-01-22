using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public float chargeSpeed;


    public Vector3 lookdir;

    CharState state;


    public Transform wave;
    public Transform disWave;
    public Transform shootLoaction;


    public float fadeSpeed;


    Animator animator;

    int hits = 0;

    public Transform chargingForm;


    Coroutine attackC;
	// Use this for initialization
	void Start () {

        animator = GetComponent<Animator>();

        state = CharState.Starting;

        lookdir = Vector3.left;

        //StartCoroutine(Charge());


        attackC = StartCoroutine(AttackLoop());
        
        
        
        //StartCoroutine(fadeOut());
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void StateCkeck()
    {

    }


    IEnumerator StartSecuence()
    {
        yield return new WaitForSeconds(2);
    }


    IEnumerator Charge()
    {
        //Coroutine charge = StartCoroutine(ChargeCoroutine());

        yield return new WaitForSeconds(6);
        chargingForm.gameObject.SetActive(true);
        //StopCoroutine(charge);

    }
    IEnumerator ChargeCoroutine()
    {
        //bool charge = true;
        while (true)
        {
            transform.Translate(lookdir * chargeSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Attack(Transform t)
    {
        animator.SetTrigger("attack");
        Transform currentWave;
        currentWave = Instantiate(t, shootLoaction.position, Quaternion.identity);

        if(lookdir == Vector3.left)
        {
            currentWave.GetComponent<waveScript>().faceRight = false;

        }

        Coroutine c = StartCoroutine(CreateTrailCoroutine(currentWave.GetComponent<waveScript>()));

        yield return new WaitForSeconds(5);
        currentWave.GetComponent<waveScript>().startMove();
        StopCoroutine(c);
        //StartCoroutine(Attack());
    }

    IEnumerator CreateTrailCoroutine(waveScript instance)
    {
        while (true)
        {
            instance.createUpdate();
            yield return null;
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            StartCoroutine(Attack(wave));
            yield return new WaitForSeconds(7);
        }
    }

    IEnumerator fadeOut()
    {
        SpriteRenderer sprite;
        sprite = GetComponent<SpriteRenderer>();

        float a = sprite.color.a;
        //print(a);
        while (a > 0.01)
        {
            //print(a);
            a = sprite.color.a;
            a = Mathf.Lerp(a, 0, Time.deltaTime * fadeSpeed);
            sprite.color = new Color(255,255,255, a);
            yield return null;
        }

        sprite.color = new Color(255, 255, 255, 0);

    }

    IEnumerator fadeIn()
    {
        SpriteRenderer sprite;
        sprite = GetComponent<SpriteRenderer>();

        float a = sprite.color.a;
        //print(a);
        while (a < 0.9)
        {
            //print(a);
            a = sprite.color.a;
            a = Mathf.Lerp(a, 1, Time.deltaTime * fadeSpeed);
            sprite.color = new Color(255, 255, 255, a);
            yield return null;
        }

        sprite.color = new Color(255, 255, 255, 1);

    }

    public IEnumerator hitEffect()
    {

        

        SpriteRenderer sprite;
        sprite = GetComponent<SpriteRenderer>();
        float a = 0;
        sprite.color = new Color(255, 255, 255, a);
        yield return new WaitForSeconds(0.1f);
        a = 1;
        sprite.color = new Color(255, 255, 255, a);
        yield return new WaitForSeconds(0.2f);
        a = 0;
        sprite.color = new Color(255, 255, 255, a);
        yield return new WaitForSeconds(0.1f);
        a = 1;
        sprite.color = new Color(255, 255, 255, a);
        yield return new WaitForSeconds(0.2f);
        a = 0;
        sprite.color = new Color(255, 255, 255, a);
        yield return new WaitForSeconds(0.1f);
        a = 1;
        sprite.color = new Color(255, 255, 255, a);
        //yield return new WaitForSeconds(0.2f);



        StopCoroutine(attackC);

        EnemyWave[] ew = FindObjectsOfType<EnemyWave>();

        foreach(EnemyWave w in ew)
        {
            Destroy(w.gameObject);
        }


        StartCoroutine(Attack(disWave));



        StartCoroutine(fadeOut());
        StartCoroutine(Charge());

        yield return new WaitForSeconds(10);
        StartCoroutine(fadeIn());

        attackC = StartCoroutine(AttackLoop());


        chargingForm.GetComponent<ChargeScript>().reset();

    }

    enum CharState
    {
        Starting, BottomRightAtk, TopLeftAtk, TopRightAtk, BottomLeftAtk
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag  == "Wave")
        {
            print("bla");
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wave")
        {
            print("outch");
            
        }
    }

    
    
}
