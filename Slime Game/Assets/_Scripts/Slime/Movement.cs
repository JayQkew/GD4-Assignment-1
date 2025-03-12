using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    public InputHandler inputHandler;


    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpMultiplier;

    [Header("Dash Settings")] 
    [SerializeField] private float _dashMultiplier;

    [Header("Inflate Settings")]
    [SerializeField] private float _inflateTime;
    
    [SerializeField] private float _maxRadius;
    [SerializeField] private float _startRadius;
    
    [SerializeField] private float _startFrequency;
    [SerializeField] private float _maxFrequency;

    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
    }

    private void Start()
    {
        _startRadius = _softBody.radius;
        _startFrequency = _softBody.frequency;
        
        inputHandler.OnJump.AddListener(Jump);
        
        inputHandler.OnGrab.AddListener(Grab);
        inputHandler.OnRelease.AddListener(Release);
        
        inputHandler.OnInflate.AddListener(Inflate);
        inputHandler.OnDeflate.AddListener(Deflate);
    }

    private void Update()
    {
        Move(inputHandler.moveInput);
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

    private void Inflate()
    {
        _softBody.radius = _maxRadius;
        _softBody.frequency = _maxFrequency;
    }

    private void Deflate()
    {
        StartCoroutine(deflate());
        _softBody.frequency = _startFrequency;
    }

    private IEnumerator deflate()
    {
        float elapsedTime = 0f;
        float startRadius = _softBody.radius; // Store the current radius

        while (elapsedTime < _inflateTime)
        {
            _softBody.radius = Mathf.Lerp(startRadius, _startRadius, elapsedTime / _inflateTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _softBody.radius = _startRadius; // Ensure it reaches the exact final value
    }
    
    private bool Grounded()
    {
        foreach (SoftBodyNode node in _softBody.node_scripts)
        {
            if (node.touchingGround) return true;
        }

        return false;
    }
}