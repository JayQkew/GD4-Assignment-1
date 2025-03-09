using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    private InputHandler _inputHandler;

    private Vector2 _lastPos;

    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;

    [SerializeField] private bool _canJump;
    [SerializeField] private float displacementThreshold;
    [SerializeField] private float checkTime = 0.5f;
    private float currTime;

    [Header("Spring Settings")] [SerializeField]
    private float _maxFrequency;

    [SerializeField] private float _midFrequency;
    [SerializeField] private float _minFrequency;

    [Header("Jump Settings")] 
    public bool isBigSlime;

    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        _inputHandler = transform.parent.GetComponent<InputHandler>();
    }

    private void Start()
    {
        _lastPos = transform.position;
    }

    private void Update()
    {
        Jump(_inputHandler.jumpInput);
        Grab(_inputHandler.grabInput);
    }

    private void Jump(bool jumpInput)
    {
        currTime += Time.deltaTime;
        if (currTime >= checkTime)
        {
            CanJump();
            currTime = 0;
        }

        if (jumpInput && _canJump && Grounded())
        {
            foreach (Rigidbody2D rb in _softBody.nodes_rb)
            {
                if (rb.bodyType == RigidbodyType2D.Dynamic)
                {
                    if (rb.transform.position.y >= _softBody.transform.position.y)
                        rb.AddForce(_inputHandler.aimInput * _movementMultiplier, ForceMode2D.Impulse);
                    else rb.AddForce(_inputHandler.aimInput * (_movementMultiplier * 0.5f), ForceMode2D.Impulse);
                }

                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            foreach (SoftBodyNode node in _softBody.node_scripts)
            {
                node.touchingGrabbable = false;
                node.grabbableObject = null;
            }

            _canJump = false;
        }
    }

    private void Grab(bool grabInput)
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            node.Grab(grabInput && !_inputHandler.jumpInput, this);
        }
    }

    private bool Grounded()
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            if (node.touchingGrabbable) return true;
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