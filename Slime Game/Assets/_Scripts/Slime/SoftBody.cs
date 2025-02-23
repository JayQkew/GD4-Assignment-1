using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    [Range(3, 40)]public int numberOfNodes = 10;
    public int radius = 3;
    public float nodeRadius = 0.001f;
    
    [Header("Soft Body Qualities")] 
    [Range(0,1)] public float dampingRatio;
    [Range(0.01,10)]public float frequency;
    
    
    public Sprite nodeSprite;
    public List<GameObject> nodes = new List<GameObject>();
    private readonly List<SpringJoint2D> _springJoints = new List<SpringJoint2D>();

    private Mesh _mesh;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    public Material meshMaterial;
    private Vector3[] _verts;
    private Vector2[] _uvs;
    private int[] _tris;

    private void Start()
    {
        CreateNodes();
        ArrangeNodes();
        ConnectNodes();
        CreateMesh();
    }

    private void Update()
    {
        // UpdatePosition();
        ApplySoftBodyQualityes();
        UpdateMesh();
        // ShapePolygonCollider();
    }

    private void UpdatePosition()
    {
        Vector3 totalPosition = Vector3.zero;
        foreach (var node in nodes)
        {
            totalPosition += node.transform.position;
        }
        
        transform.position = totalPosition/nodes.Count;
    }

    private void CreateNodes()
    {
        for (int i = 0; i < numberOfNodes; i++)
        {
            GameObject node = new GameObject("SoftBodyNode");

            Rigidbody2D rb = node.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            
            // SpriteRenderer sr = node.AddComponent<SpriteRenderer>();
            // sr.sprite = nodeSprite;

            CircleCollider2D cc = node.AddComponent<CircleCollider2D>();
            cc.radius = nodeRadius;
            
            node.transform.SetParent(transform);
            
            nodes.Add(node);
        }
    }

    private void ArrangeNodes()
    {
        float equalAngles = 360/numberOfNodes;
        float startAngle = 90;

        for (int n = 0; n < nodes.Count; n++)
        {
            float x = Mathf.Cos(startAngle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(startAngle * Mathf.Deg2Rad) * radius;
            nodes[n].transform.position = new Vector3(x, y, 0);
            
            startAngle += equalAngles;
        }
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            for (int j = i; j < nodes.Count; j++)
            {
                SpringJoint2D sprintJoint = nodes[i].AddComponent<SpringJoint2D>();
                sprintJoint.enableCollision = true;
                sprintJoint.connectedBody = nodes[j].GetComponent<Rigidbody2D>();
                
                _springJoints.Add(sprintJoint);
            }
        }
    }

    private void ApplySoftBodyQualityes()
    {
        foreach (var springJoint in _springJoints)
        {
            springJoint.dampingRatio = dampingRatio;
            springJoint.frequency = frequency;
        }
    }

    private void CreateMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "SoftBodyMesh";
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        
        _verts = new Vector3[numberOfNodes];
        _uvs = new Vector2[numberOfNodes];
        _tris = new int[(numberOfNodes - 2) * 3];

        // Set vertices and UVs
        for (int n = 0; n < numberOfNodes; n++)
        {
            _verts[n] = nodes[n].transform.position;
            _uvs[n] = new Vector2(_verts[n].x / radius, _verts[n].y / radius);
        }

        // Set triangles
        for (int i = 0; i < numberOfNodes - 2; i++)
        {
            _tris[i * 3] = 0;
            _tris[i * 3 + 1] = i + 1;
            _tris[i * 3 + 2] = i + 2;
        }

        _mesh.vertices = _verts;
        _mesh.uv = _uvs;
        _mesh.triangles = _tris;
        _mesh.RecalculateNormals();

        _meshRenderer.material = meshMaterial;
        _meshFilter.mesh = _mesh;
    }
    
    private void UpdateMesh()
    {
        // Update vertices
        for (int n = 0; n < numberOfNodes; n++)
        {
            _verts[n] = nodes[n].transform.position;
        }

        _mesh.vertices = _verts;
        _mesh.RecalculateNormals();
    }
}
