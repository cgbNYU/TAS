using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitwiseOperatorCalculator : MonoBehaviour
{

    public int a;
    public int b;
    private int c;

    public int outPutC;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        outPutC = a & b;
    }
}
