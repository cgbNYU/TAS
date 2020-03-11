using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Run through a Perlin script to find a Y position for the objects in the array
/// </summary>
public class MoveWithPerlin : MonoBehaviour
{

    public GameObject node; //the prefab that we use for nodes
    
    private List<GameObject> perlinObjects; //holds a list of all of the objects in the array

    public int nodeCount; //how many objects should be in the array

    public float objectOffset; //how far apart are the objects

    public float heightScale; //modify the values coming out of the Perlin function to make the objects move higher

    private float _time;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        PerlinMove();
    }

    private void CreateObjects()
    {
        perlinObjects = new List<GameObject>();
        for (int i = 0; i < nodeCount; i++)
        {
            GameObject newNode = Instantiate(node);
            newNode.transform.position = new Vector3(transform.position.x + (i * objectOffset), transform.position.y, transform.position.z);
            perlinObjects.Add(newNode);
        }
    }

    private void PerlinMove()
    {
        for (int i = 0; i < perlinObjects.Count; i++)
        {
            float perlinValue = i + Time.time;
            float yPos = Perlin.Noise(perlinValue) * heightScale;
            
            perlinObjects[i].transform.position = new Vector3(perlinObjects[i].transform.position.x, yPos , perlinObjects[i].transform.position.z);
        }
    }
    
}
