using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProductCalculator : MonoBehaviour
{
    [Header("Inputs for Cross Product")]
    public Vector3 InputVectorA;
    public Vector3 InputVectorB;
    
    [Header("Output of Calculation")]
    public Vector3 OutPutVectorC;

    // Update is called once per frame
    void Update()
    {
        Vector3 a = InputVectorA.normalized;
        Vector3 b = InputVectorB.normalized;

        OutPutVectorC = Vector3.Cross(a, b);
        
        Debug.DrawLine(Vector3.zero, a, Color.red);
        Debug.DrawLine(Vector3.zero, b, Color.green);
        Debug.DrawLine(Vector3.zero, OutPutVectorC, Color.blue);
    }
}
