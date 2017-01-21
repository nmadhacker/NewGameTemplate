using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class waveScript : MonoBehaviour {

    LineRenderer lineRenderer;

    public int size;

    public float wLength;
    public float wWidth;


    public float res;


    public float shootSpeed;
    float shootTimer;
    // Use this for initialization

    bool canMove = false;

    bool deleting = false;

    int offset;
    

	void Start () {
        lineRenderer = GetComponent<LineRenderer>();

        //lineRenderer.numPositions = (int)(size * res);

        //GenerateRoundWave();
        //generateCollider();


        offset = 0;
    }
	
    void FixedUpdate()
    {
        //lineRenderer.numPositions = (int) (size * res);
        
        
    }

	// Update is called once per frame
	void Update () {

        if (canMove)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootSpeed / res)
            {
                waveMove();
                shootTimer -= (shootSpeed / res);
            }
        }

        if (deleting)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootSpeed / res)
            {
                deleteWave();
                shootTimer -= (shootSpeed / res);
            }
        }

    }

    public void createUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootSpeed / res){
            //size++;

            lineRenderer.numPositions++;

            //lineRenderer.numPositions = (int)(size * res);

            shootTimer -= shootSpeed / res;
            GenerateRoundWave();
            generateCollider();
        }

    }

    public void startMove()
    {
        canMove = true;
    }



    void waveMove()
    {
        offset++;
        Vector3 pos = Vector3.zero;

        int npos = lineRenderer.numPositions;

        for (int i = 0; i < npos; i++)
        {
            if(i < (npos - 1))
            {
                pos = lineRenderer.GetPosition(i + 1);
                lineRenderer.SetPosition(i, pos);
            }
            else
            {
                
                pos = new Vector3( ( ( (i+offset) / res ) * wWidth) , Mathf.Cos((i+offset)/ res) * wLength, 0);

                //print(pos);
                lineRenderer.SetPosition(i, pos);
            }

            
        }

        lineRenderer.SetPosition(npos-1, pos);

        generateCollider();
    }

    void generateCollider()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();

        int n = lineRenderer.numPositions;
        Vector3[] positions = new Vector3[n];
        lineRenderer.GetPositions(positions);

        

        //print(n);
        //print(positions);

        Vector2[] positions2 = new Vector2[n];

        for (int i = 0; i < positions.Length; i++)
        {
            positions2[i] = positions[i];
        }


        edgeCollider.points = positions2;
    }

    void GenerateRoundWave()
    {
        Vector3 pos;
        int npos = lineRenderer.numPositions;
        for (int i = 0; i < npos; i++)
        {
            pos = new Vector3((i/res)*wWidth, Mathf.Cos(i/res) *wLength, 0);

            lineRenderer.SetPosition(i, pos);
        }
        
    }
    Vector3 getNextpos()
    {
        return Vector3.zero;
    }

    void GenerateSquareWave(){
        int xcount = 3;
        int ycount = 1;

        float x = 0;
        float y = -wLength;



        Vector3 newPos;

        for (int i = 0; i < size; i++)
        {
            xcount--;
            ycount--;

            if (xcount == 0){
                xcount = 3;
                x+= wWidth;
            }


            if(ycount == 0)
            {
                newPos = new Vector3(x, 0, 0);
                ycount = 3;
                y = y*-1;
            }
            else
            {
                newPos = new Vector3(x, y, 0);
            }
            

            lineRenderer.SetPosition(i, newPos);
            
            if(i > 0)
            {
                Vector3 prevPoss = lineRenderer.GetPosition(i - 1);


            }

        }
    }

    void deleteWave()
    {

        Vector3 pos = Vector3.zero;

        int npos = lineRenderer.numPositions;

        

        for (int i = 0; i < npos; i++)
        {
            if (i < (npos - 1))
            {
                pos = lineRenderer.GetPosition(i + 1);
                lineRenderer.SetPosition(i, pos);
            }
            else
            {

                pos = new Vector3((((i + offset) / res) * wWidth), Mathf.Cos((i + offset) / res) * wLength, 0);

                //print(pos);
                lineRenderer.SetPosition(i, pos);
            }


        }

        lineRenderer.numPositions = npos - 1;

        if(npos-1 == 0)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        print("wavehit");
        if(collision.collider.tag != "Player")
        {
            canMove = false;
            deleting = true;
        }
    }
}
