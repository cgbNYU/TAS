using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinEnemyMove : MonoBehaviour
{

    public float towardsSpeed;

    public float moveScale;

    private TrailRenderer _tr;

    private float _startY;

    private void Start()
    {
        _tr = GetComponent<TrailRenderer>();
        _tr.material = GetComponentInChildren<MeshRenderer>().material;

        _startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * towardsSpeed;

        float yPos = Perlin.Noise(Time.time) * moveScale;
        
        transform.position = new Vector3(transform.position.x, yPos + _startY, transform.position.z);
    }
}
