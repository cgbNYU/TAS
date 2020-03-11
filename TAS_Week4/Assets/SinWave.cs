using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWave : MonoBehaviour
{
    public float Speed;
    
    [Range(0, 2 * Mathf.PI)] public float theta = 0;

    public float[] r;

    public QuadMaker QM;

    private int i;
    
    // Start is called before the first frame update
    void Start()
    {
        QM = GameObject.Find("QuadMaker").GetComponent<QuadMaker>();
    }

    // Update is called once per frame
    void Update()
    {

        if (i == QM._verts.Length - 1)
        {
            i = 0;
        }
        float radius = QM._verts[i].x;
        QM._verts[i] = PointOnCircle(Time.deltaTime * Speed, r[i] * Mathf.Sin(Time.time));

        i++;

        //QM.MakeMesh();
    }

    public Vector3 PointOnCircle(float angle, float radius)
    {
        return new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);
    }
}
