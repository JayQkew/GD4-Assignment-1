using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoftBody : MonoBehaviour
{
    [Range(3, 40)]public int numberOfNodes = 10;
    public float radius = 3;
    public float nodeRadius = 0.001f;
    [SerializeField] private Transform nodeParent;
    
    [Header("Soft Body Qualities")] 
    [Range(0,1)] public float dampingRatio;
    [Range(0.1f,10)]public float frequency;
    
    
    public Sprite nodeSprite;
    public List<GameObject> nodes = new List<GameObject>();
    private readonly List<SpringJoint2D> _springJoints = new List<SpringJoint2D>();

    private PolygonCollider2D _polygonCollider;
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
        ApplySoftBodyQualities();
        UpdateMesh();
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 centerPos = Vector3.zero;
        foreach (GameObject n in nodes)
        {
            centerPos += n.transform.position;
        }
        transform.position = centerPos/nodes.Count;
    }

    private void CreateNodes()
    {
        for (int i = 0; i < numberOfNodes; i++)
        {
            GameObject node = new GameObject("SoftBodyNode");

            Rigidbody2D rb = node.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;

            CircleCollider2D cc = node.AddComponent<CircleCollider2D>();
            cc.radius = nodeRadius;
            
            node.transform.SetParent(nodeParent == null ? transform : nodeParent);
            node.layer = LayerMask.NameToLayer("SoftBodyNodes");
            
            nodes.Add(node);
        }
    }

    private void ArrangeNodes()
    {
        float equalAngles = 360/numberOfNodes;
        float startAngle = 90;

        foreach (var n in nodes)
        {
            float x = Mathf.Cos(startAngle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(startAngle * Mathf.Deg2Rad) * radius;
            n.transform.localPosition = new Vector3(x, y, 0);
            
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

    private void ApplySoftBodyQualities()
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
        
        _polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        if (nodeParent == null)
        {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        else
        {
            _meshFilter = nodeParent.gameObject.AddComponent<MeshFilter>();
            _meshRenderer = nodeParent.gameObject.AddComponent<MeshRenderer>();
        }
        
        _verts = new Vector3[numberOfNodes];
        _uvs = new Vector2[numberOfNodes];
        _tris = new int[(numberOfNodes - 2) * 3];
        _polygonCollider.points = new Vector2[_verts.Length];
        Vector2[] colliderPoints = new Vector2[_verts.Length];

        // Set vertices and UVs
        for (int n = 0; n < numberOfNodes; n++)
        {
            _verts[n] = nodes[n].transform.localPosition;
            _uvs[n] = new Vector2(_verts[n].x / radius, _verts[n].y / radius);
            colliderPoints[n] = nodes[n].transform.position;
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
        _polygonCollider.excludeLayers = LayerMask.GetMask("SoftBodyNodes");
        _polygonCollider.SetPath(0, colliderPoints);
    }
    
    private void UpdateMesh()
    {
        Vector2[] colliderPoints = new Vector2[_verts.Length];
        // Update vertices
        for (int n = 0; n < numberOfNodes; n++)
        {
            _verts[n] = nodes[n].transform.localPosition;
            colliderPoints[n] = transform.InverseTransformPoint(nodes[n].transform.position);
        }

        _polygonCollider.SetPath(0, colliderPoints);
        _mesh.vertices = _verts;
        _mesh.RecalculateNormals();
    }
}
