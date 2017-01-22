using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{

    waveScript w;
    // Use this for initialization
    void Start()
    {
        w = GetComponent<waveScript>();
        w.lineRenderer.numPositions = (int)(w.size * w.res);

    }

    // Update is called once per frame
    void Update()
    {
        w.GenerateRoundWave();
    }

    public void updatehealth(int h)
    {
        w.size = h;
        w.lineRenderer.numPositions = (int)(w.size * w.res);
    }

}
