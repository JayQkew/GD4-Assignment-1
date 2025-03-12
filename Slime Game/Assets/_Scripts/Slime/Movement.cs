using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    public InputHandler inputHandler;

    private Vector2 _lastPos;

    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpMultiplier;
    [SerializeField] private bool _canJump;
    [SerializeField] private float displacementThreshold;
    [SerializeField] private float checkTime = 0.5f;
    private float currTime;

    [Header("Spring Settings")] 
    [SerializeField] private float _maxFrequency;
    [SerializeField] private float _midFrequency;
    [SerializeField] private float _minFrequency;

    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
    }

    private void Start()
    {
        _lastPos = transform.position;
        inputHandler.OnJumpPressed.AddListener(Jump);
        inputHandler.OnGrab.AddListener(Grab);
        inputHandler.OnRelease.AddListener(Release);
    }

    private void Update()
    {
        Move(inputHandler.aimInput);
    }

    private void FixedUpdate()
    {
    }

    private void Move(Vector2 dir)
    {
        int xInput = 0;
        if(dir.x > 0) xInput = 1;
        else if(dir.x < 0) xInput = -1;
        Vector2 inputDir = new Vector2(xInput, 0);
        
        Debug.Log(inputDir);
        
        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            if (rb.transform.position.y >= _softBody.transform.position.y)
            {
                rb.AddForce( dir * (_movementMultiplier * 100 * Time.deltaTime), ForceMode2D.Force);
            }
        }
    }

    private void Jump()
    {
        // currTime += Time.deltaTime;
        // if (currTime >= checkTime)
        // {
        //     CanJump();
        //     currTime = 0;
        // }

        if (Grounded())
        {
            foreach (Rigidbody2D rb in _softBody.nodes_rb)
            {
                if (rb.bodyType == RigidbodyType2D.Dynamic)
                {
                    if (rb.transform.position.y >= _softBody.transform.position.y)
                        rb.AddForce(Vector3.up * _jumpMultiplier, ForceMode2D.Impulse);
                    else rb.AddForce(Vector3.up * (_jumpMultiplier * 0.5f), ForceMode2D.Impulse);
                }

                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            foreach (SoftBodyNode node in _softBody.node_scripts)
            {
                node.touchingGrabbable = false;
            }

            _canJump = false;
        }
    }

    private void Grab()
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            node.Grab();
        }
    }

    private void Release()
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            node.Release();
        }
    }

    private bool Grounded()
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            if (node.touchingGround) return true;
        }

        return false;
    }

    private bool CanJump()
    {
        Vector2 displacement = (Vector2)transform.position - _lastPos;

        if (Mathf.Abs(displacement.x) < displacementThreshold && Mathf.Abs(displacement.y) < displacementThreshold)
            _canJump = true;
        else _canJump = false;

        _lastPos = transform.position;

        return _canJump;
    }
}