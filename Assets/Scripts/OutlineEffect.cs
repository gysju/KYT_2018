using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour {

    public float Tickness = 2.0f;

    [SerializeField] private Material _outlineMaterial = null;
    private Vector3[] _initVertices;

    private MeshFilter _meshFilter;

    private MeshRenderer _outlineMeshRenderer;
    private MeshFilter _outlineMeshFilter;

    private Mesh _outlineMesh;

    void Start()
    {
        _meshFilter = gameObject.GetComponent<MeshFilter>();

        GameObject OutGameObject = new GameObject("outline");
        OutGameObject.transform.parent = transform;
        OutGameObject.transform.localPosition = Vector3.zero;

        _outlineMesh = Instantiate(_meshFilter.mesh) as Mesh;

        _outlineMeshFilter = OutGameObject.AddComponent<MeshFilter>();
        _outlineMeshFilter.mesh = _outlineMesh;

        _outlineMeshRenderer = OutGameObject.AddComponent<MeshRenderer>();
        _outlineMeshRenderer.material = _outlineMaterial;
        _outlineMesh.RecalculateNormals();

        _initVertices = _outlineMesh.vertices;

        GenerateOutlineMesh(Tickness);
    }

    private void Update()
    {
        GenerateOutlineMesh(Tickness);
    }

    void GenerateOutlineMesh(float tickness)
    {
        Vector3[] NewVertices = new Vector3[_outlineMesh.vertexCount];
        for (int i = 0; i < _outlineMesh.vertexCount; ++i)
        {
            NewVertices[i] = _initVertices[i] - _outlineMesh.normals[i] * tickness;
        }
        _outlineMesh.vertices = NewVertices;
        _outlineMesh.RecalculateBounds();
    }
}
