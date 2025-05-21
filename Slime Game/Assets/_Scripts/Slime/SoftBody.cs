using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//Title: How I Wont the GMTK Game Jam
//Author: JimmyGameDev
//Date: Jan 18, 2025
//Availablity: https://www.youtube.com/watch?v=y1D4DiZhSIo
//This video describes how he made the soft body, I inferred from this video to create this portable softbody script. 
public class SoftBody : MonoBehaviour
{
    private PlayerStats playerStats;

    [Range(3, 40), SerializeField] private int numberOfNodes;
    private float oldRadius;
    public float currRadius;
    private readonly float nodeRadius = 0.01f;
    [SerializeField] private Transform nodeParent;

    [Header("Soft Body Qualities")]
    [Range(0, 1), SerializeField] private float dampingRatio;
    [Range(0.1f, 10)] public float frequency;
    [SerializeField] private float radiusChangeSpeed;

    private List<GameObject> nodes;
    [HideInInspector] public List<Rigidbody2D> nodesRb;
    [HideInInspector] public List<SoftBodyNode> nodeScripts;
    private readonly List<SpringJoint2D> _springJoints;
    private bool setSprintDistance;
    private int frameCount;
    private readonly List<float> springJointsStartDistance;
    private PolygonCollider2D _polygonCollider;
    private Mesh _mesh;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    public Material meshMaterial;
    private Vector3[] _verts;
    private Vector2[] _uvs;
    private int[] _tris;

    private void Awake() {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start() {
        oldRadius = playerStats.GetStatValue(StatName.MinRadius);
        currRadius = oldRadius;
        CreateNodes();
        ArrangeNodes();
        ConnectNodes();
        CreateMesh();
    }

    private void Update() {
        ApplySoftBodyQualities();
        UpdateMesh();
        UpdatePosition();
        UpdateRadius();

        if (!setSprintDistance && frameCount == 2) {
            GetSpringDistances();
            setSprintDistance = true;
        }

        if (frameCount < 2) frameCount++;
    }

    private void FixedUpdate() {
        UpdateCollider();
    }

    private void OnEnable() {
        nodeParent.gameObject.SetActive(true);
    }

    private void OnDisable() {
        nodeParent.gameObject.SetActive(false);
    }

    /// <summary>
    /// updates the center position of all the nodes
    /// </summary>
    private void UpdatePosition() {
        Vector3 centerPos = new Vector3(0, 0, -21f);
        foreach (GameObject n in nodes) {
            centerPos += new Vector3(n.transform.position.x, n.transform.position.y, 0);
        }

        transform.position = centerPos / nodes.Count;
    }

    /// <summary>
    /// updates the radius to keep up with player controls
    /// </summary>
    private void UpdateRadius() {
        if (!Mathf.Approximately(currRadius, oldRadius)) {
            float radiusFactor = Mathf.Lerp(oldRadius, currRadius, Time.deltaTime * radiusChangeSpeed);

            for (int i = 0; i < _springJoints.Count; i++) {
                _springJoints[i].distance = springJointsStartDistance[i] * (1 + radiusFactor);
            }

            oldRadius = radiusFactor; // Update oldRadius for consistency
        }
    }

    #region Creating Soft Body

    /// <summary>
    /// creates soft body nodes and adds all components and attributes
    /// </summary>
    private void CreateNodes() {
        for (int i = 0; i < numberOfNodes; i++) {
            GameObject node = new GameObject("SoftBodyNode");
            node.tag = "SoftBodyNode";

            SoftBodyNode nodeScript = node.AddComponent<SoftBodyNode>();
            nodeScripts.Add(nodeScript);

            Rigidbody2D rb = node.AddComponent<Rigidbody2D>();
            rb.freezeRotation = true;
            nodeScript.rb = rb;
            nodesRb.Add(rb);

            CircleCollider2D cc = node.AddComponent<CircleCollider2D>();
            cc.radius = nodeRadius;

            //exclude this polygon collider (flip the colliders between the two slimes)
            int excludeLayer = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
            cc.excludeLayers = excludeLayer;
            node.layer = LayerMask.LayerToName(gameObject.layer) == "SoftBody1"
                ? LayerMask.NameToLayer("SoftBody2")
                : LayerMask.NameToLayer("SoftBody1");

            node.transform.SetParent(nodeParent == null ? transform : nodeParent);

            nodes.Add(node);
        }
    }

    /// <summary>
    /// arranges the soft body nodes in a circular shape
    /// </summary>
    private void ArrangeNodes() {
        float equalAngles = 360f / numberOfNodes;
        float startAngle = 90;

        foreach (var n in nodes) {
            float x = Mathf.Cos(startAngle * Mathf.Deg2Rad) * currRadius;
            float y = Mathf.Sin(startAngle * Mathf.Deg2Rad) * currRadius;
            n.transform.localPosition = new Vector3(x, y, 1);
            startAngle += equalAngles;
        }
    }

    /// <summary>
    /// connects all the nodes with a spring joint
    /// </summary>
    private void ConnectNodes() {
        for (int i = 0; i < nodes.Count - 1; i++) {
            for (int j = i + 1; j < nodes.Count; j++) {
                SpringJoint2D sprintJoint = nodes[i].AddComponent<SpringJoint2D>();
                sprintJoint.enableCollision = true;
                sprintJoint.connectedBody = nodes[j].GetComponent<Rigidbody2D>();
                sprintJoint.frequency = frequency;

                _springJoints.Add(sprintJoint);
            }
        }
    }

    /// <summary>
    /// get the current distances of the spring joints
    /// </summary>
    private void GetSpringDistances() {
        foreach (var spring in _springJoints) {
            springJointsStartDistance.Add(spring.distance);
        }
    }

    /// <summary>
    /// make all spring joints have the same dampingRatio and frequency
    /// </summary>
    private void ApplySoftBodyQualities() {
        foreach (var springJoint in _springJoints) {
            springJoint.dampingRatio = dampingRatio;
            springJoint.frequency = frequency;
        }
    }

    #endregion

    #region Soft Body Mesh

    //Title: Creating a Mesh
    //Author: Jasper Flick
    //Date: 2021-10-30
    //Availability: https://catlikecoding.com/unity/tutorials/procedural-meshes/creating-a-mesh/
    //Learnt the concepts here, managed to modify it to suit the slimes constant changing shape
    private void CreateMesh() {
        _mesh = new Mesh();
        _mesh.name = "SoftBodyMesh";

        _polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        if (nodeParent == null) {
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }
        else {
            _meshFilter = nodeParent.gameObject.AddComponent<MeshFilter>();
            _meshRenderer = nodeParent.GetComponent<MeshRenderer>();
        }

        _verts = new Vector3[numberOfNodes];
        _uvs = new Vector2[numberOfNodes];
        _tris = new int[(numberOfNodes - 2) * 3];
        _polygonCollider.points = new Vector2[_verts.Length];
        Vector2[] colliderPoints = new Vector2[_verts.Length];

        for (int n = 0; n < numberOfNodes; n++) {
            _verts[n] = nodes[n].transform.localPosition;
            _uvs[n] = new Vector2(_verts[n].x / currRadius, _verts[n].y / currRadius);
            colliderPoints[n] = nodes[n].transform.position;
        }

        for (int i = 0; i < numberOfNodes - 2; i++) {
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
        _polygonCollider.SetPath(0, colliderPoints);
    }

    private void UpdateMesh() {
        Vector2[] colliderPoints = new Vector2[_verts.Length];
        // Update vertices
        for (int n = 0; n < numberOfNodes; n++) {
            _verts[n] = nodes[n].transform.localPosition;
        }

        _mesh.vertices = _verts;
        _mesh.RecalculateNormals();
    }

    private void UpdateCollider() {
        Vector2[] colliderPoints = new Vector2[_verts.Length];
        for (int n = 0; n < numberOfNodes; n++) {
            colliderPoints[n] = transform.InverseTransformPoint(nodes[n].transform.position);
        }

        _polygonCollider.SetPath(0, colliderPoints);
    }

    #endregion

    /// <summary>
    /// moves the soft body to the given position
    /// </summary>
    /// <param name="newPosition"></param>
    public void MoveSoftBody(Vector2 newPosition) {
        Vector2[] dif = new Vector2[nodes.Count];
        for (int i = 0; i < nodes.Count; i++) {
            dif[i] = nodes[i].transform.position - transform.position;
            nodes[i].transform.position = newPosition + dif[i];
        }
    }

    /// <summary>
    /// adds a force to the soft body, similar to rigidbody forces
    /// </summary>
    public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse) {
        foreach (Rigidbody2D rb in nodesRb) {
            rb.AddForce(force, forceMode);
        }
    }
}