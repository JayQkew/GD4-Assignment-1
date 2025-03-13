using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    public InputHandler inputHandler;
    private Gamepad _gamepad;
    
    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;
    
    [Header("Inflate Settings")] [SerializeField]
    private float _inflateTime;
    [SerializeField] private float _maxRadius;
    private float _startRadius;
    [SerializeReference] private Vector2 _inflateShake;
    [SerializeField] private float _maxFrequency;
    private float _startFrequency;

    [Header("Dash Settings")] 
    [SerializeField] private bool _canDash;
    [SerializeField] private float _dashCooldown;
    [SerializeField] private float _dashForceMultiplier;
    private float _currDashCooldown;
    
    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
    }

    private void Start()
    {
        _gamepad = Gamepad.current;
        
        _startRadius = _softBody.radius;
        _startFrequency = _softBody.frequency;

        _currDashCooldown = 0;

        inputHandler.OnGrab.AddListener(Grab);
        inputHandler.OnRelease.AddListener(Release);

        inputHandler.OnInflate.AddListener(Inflate);
        inputHandler.OnDeflate.AddListener(Deflate);
        
        inputHandler.OnDash.AddListener(Dash);
    }

    private void Update()
    {
        Move(inputHandler.moveInput);
        DashCoolDown();
    }

    private void Move(Vector2 dir)
    {
        int xInput = 0;
        if (dir.x > 0) xInput = 1;
        else if (dir.x < 0) xInput = -1;
        Vector2 inputDir = new Vector2(xInput, 0);

        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            if (rb.transform.position.y >= _softBody.transform.position.y)
            {
                rb.AddForce(dir * (_movementMultiplier * 100 * Time.deltaTime), ForceMode2D.Force);
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
        _gamepad.SetMotorSpeeds(_inflateShake.x, _inflateShake.y);
        
        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            rb.gravityScale = 0;
        }  
        
        _softBody.radius = _maxRadius;
        _softBody.frequency = _maxFrequency;
    }

    private void Deflate()
    {
        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            rb.gravityScale = 1;
        }  
        
        _softBody.radius = _startRadius;
        _softBody.frequency = _startFrequency;
        _gamepad.PauseHaptics();
    }

    private void Dash()
    {
        if (_canDash)
        {
            foreach (Rigidbody2D rb in _softBody.nodes_rb)
            {
                rb.linearVelocity *= 0.5f;
            }   
            foreach (Rigidbody2D rb in _softBody.nodes_rb)
            {
                rb.AddForce(inputHandler.aimInput * _dashForceMultiplier, ForceMode2D.Impulse);
            }

            _canDash = false;
        }
    }

    private void DashCoolDown()
    {
        _currDashCooldown += Time.deltaTime;
        if (_currDashCooldown >= _dashCooldown)
        {
            _canDash = true;
            _currDashCooldown = 0;
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
}