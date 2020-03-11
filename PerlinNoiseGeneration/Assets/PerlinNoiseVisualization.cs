using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseVisualization : MonoBehaviour
{

    public GameObject marker;

    
    public int startingVal;
    public int nodeCount;
    
    public float widthScale;
    
    public float heightScale;

    private List<GameObject> _nodes;
    private int _listRef = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _nodes = new List<GameObject>();
        for (float i = startingVal; i <= startingVal + nodeCount; i += .2f)
        {
            for (float j = startingVal; j <= startingVal + nodeCount; j += .2f)
            {
                GameObject tempMarker = Instantiate(marker);

                float xPos = i * widthScale;
                float zPos = j * widthScale;
                float yPos = Perlin.Noise(i, j) * heightScale;

                float xMask = i + 545.24f;
                float zMask = j + 545.24f;
                float yMask = Perlin.Noise(xMask/5, zMask/5);

                float xOct = i + 25.87f;
                float zOct = j + 25.87f;

                float yOct = Perlin.Noise(xOct / 3, zOct / 3);
            
                Vector3 markerPos = new Vector3(xPos, (yPos * yMask) + yOct * 5, zPos);

                tempMarker.transform.position = markerPos;
                _nodes.Add(tempMarker);
            }
            
        }
    }

    void Update()
    {
        for (float i = startingVal; i <= startingVal + nodeCount; i += .2f)
        {
            for (float j = startingVal; j <= startingVal + nodeCount; j += .2f)
            {
                float timedI = i + Time.time;
                float timedJ = j + Time.time;

                float yPos = Perlin.Noise(timedI, timedJ) * heightScale;
                
                float xMask = timedI + 545.24f;
                float zMask = timedJ + 545.24f;
                float yMask = Perlin.Noise(xMask/5, zMask/5);

                float xOct = timedI + 25.87f;
                float zOct = timedJ + 25.87f;

                float yOct = Perlin.Noise(xOct / 3, zOct / 3);
                
                Vector3 markerPos = new Vector3(_nodes[_listRef].transform.position.x, (yPos * yMask) + yOct * 5, _nodes[_listRef].transform.position.z);
                _nodes[_listRef].transform.position = markerPos;

                _listRef++;
            }
        }

        _listRef = 0;
    }

}
