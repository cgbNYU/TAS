using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeQuad : MonoBehaviour
{
    private MeshRenderer _myMR;
    private MeshFilter _myMF; //the component that stores the reference to the mesh

    private Mesh _myMesh;
    

    private Vector3[] _verts;
    private int[] _tris;
    private Vector3[] _norms;
    private Vector2[] _uvs;
    
    private Material _mat;
    
    void Start()
    {
        _myMR = gameObject.AddComponent<MeshRenderer>();
        _myMF = gameObject.AddComponent<MeshFilter>();
        
        _myMesh = new Mesh(); //meshes are objects so you have to create it before manipulating it
        _myMesh.name = "SteveTheTriangle";
        
        MakeVerts();
        MakeTris();
        MakeNorms();
        MakeUVs();
        
        PassDataToMesh();
        PopulateComponents();
        GenerateMaterial();
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            _uvs[i].x += Time.deltaTime;
            //_verts[i].z = Random.Range(0f, 1f);
            _norms[i] = Random.insideUnitSphere;
        }

        /*for (int i = 0; i < _tris.Length; i++)
        {
            _tris[i] = Random.Range(0, _verts.Length);
        }*/

        _myMesh.uv = _uvs;
        _myMesh.vertices = _verts;
        _myMesh.triangles = _tris;
        _myMesh.normals = _norms;
    }

    void MakeVerts()
    {
        _verts = new Vector3[4];
        
        _verts[0] = new Vector3(-1, -1, 1);
        _verts[1] = new Vector3(1, -1, -1);
        _verts[2] = new Vector3(-1, 1, 0);
        _verts[3] = new Vector3(1, 1, 0);
    }

    void MakeTris()
    {
        _tris = new int[6];
        _tris[0] = 0;
        _tris[1] = 2;
        _tris[2] = 1;
        _tris[3] = 2;
        _tris[4] = 3;
        _tris[5] = 1;
    }
    
    void MakeNorms()
    {
        _norms = new Vector3[4];

        _norms[0] = Vector3.Cross(
            Vector3.Normalize(_verts[2] - _verts[0]),
            Vector3.Normalize(_verts[1] - _verts[0]));

        _norms[1] = Vector3.Cross(
            Vector3.Normalize(_verts[0] - _verts[1]),
            Vector3.Normalize(_verts[2] - _verts[1]));
        
        _norms[2] = Vector3.Cross(
            Vector3.Normalize(_verts[1] - _verts[2]),
            Vector3.Normalize(_verts[0] - _verts[2]));
        
        _norms[3] = Vector3.Cross(
            Vector3.Normalize(_verts[1] - _verts[3]),
            Vector3.Normalize(_verts[2] - _verts[3]));
    }

    void MakeUVs()
    {
        _uvs = new Vector2[4];
        _uvs[0] = new Vector2(0, 0);
        _uvs[1] = new Vector2(1, 0);
        _uvs[2] = new Vector2(0, 1);
        _uvs[3] = new Vector2(1, 1);
    }

    void PassDataToMesh()
    {
        _myMesh.vertices = _verts;
        _myMesh.triangles = _tris;
        _myMesh.normals = _norms;
        _myMesh.uv = _uvs;
    }

    void PopulateComponents()
    {
        _myMF.mesh = _myMesh;
    }

    void GenerateMaterial()
    {
        _mat = new Material(Shader.Find("Standard"));
        _myMR.material = _mat;
    }

    void SmoothNormals()
    {
        _norms = new Vector3[4];
        _norms[0] = Vector3.Normalize(new Vector3(0, -1, -1));
    }
}
