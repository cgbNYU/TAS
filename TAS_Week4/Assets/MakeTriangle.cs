using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTriangle : MonoBehaviour
{
    private MeshRenderer _myMR;
    private MeshFilter _myMF; //the component that stores the reference to the mesh

    private Mesh _myMesh;
    

    private Vector3[] _verts;
    private int[] _tris;
    private Vector3[] _norms;

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
        
        PassDataToMesh();
        PopulateComponents();
        GenerateMaterial();
    }

    void MakeVerts()
    {
        _verts = new Vector3[3];
        
        _verts[0] = new Vector3(-1, -1, 0);
        _verts[1] = new Vector3(1, -1, 0);
        _verts[2] = new Vector3(-1, 1, 0);
    }

    void MakeTris()
    {
        _tris = new int[3];
        _tris[0] = 0;
        _tris[1] = 2;
        _tris[2] = 1;
    }
    
    void MakeNorms()
    {
        _norms = new Vector3[3];

        _norms[0] = Vector3.Cross(
            Vector3.Normalize(_verts[2] - _verts[0]),
            Vector3.Normalize(_verts[1] - _verts[0]));

        _norms[1] = Vector3.Cross(
            Vector3.Normalize(_verts[0] - _verts[1]),
            Vector3.Normalize(_verts[2] - _verts[1]));
        
        _norms[2] = Vector3.Cross(
            Vector3.Normalize(_verts[1] - _verts[2]),
            Vector3.Normalize(_verts[0] - _verts[2]));
    }

    void PassDataToMesh()
    {
        _myMesh.vertices = _verts;
        _myMesh.triangles = _tris;
        _myMesh.normals = _norms;
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

    
}
