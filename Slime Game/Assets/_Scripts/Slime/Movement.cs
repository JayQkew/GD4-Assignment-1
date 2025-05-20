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
    [HideInInspector] public InputHandler inputHandler;
    private Gamepad _gamepad;

    [Header("Resource")]
    [SerializeReference] private float currAir;

    [SerializeReference] private float maxAir;

    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;

    [SerializeField] private float _moveCostMultiplier = 1;

    [Header("Inflate Settings")] [SerializeField]
    private bool isInflated;

    [SerializeField] private float _inflateTime;
    [SerializeField] private float _maxRadius;
    private float _startRadius;
    [SerializeReference] private Vector2 _inflateShake;
    [SerializeField] private float _maxFrequency;
    private float _startFrequency;

    [Header("Dash Settings")]
    [SerializeField] private float _dashCooldown;

    [SerializeField] private float _dashForceMultiplier;
    [SerializeField] private float _dashCost;

    private void Awake() {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
    }

    private void Start() {
        _gamepad = Gamepad.current;

        _startRadius = _softBody.radius;
        _startFrequency = _softBody.frequency;

        inputHandler.onInflate.AddListener(Inflate);
        inputHandler.onDeflate.AddListener(Deflate);

        inputHandler.onDash.AddListener(Dash);
    }

    private void Update() {
        Move(inputHandler.moveInput);
        if (Grounded()) currAir = maxAir;

        if (currAir < 0) Deflate();

        if (isInflated) _softBody.radius = SetSlimeRadius();
    }

    private void Move(Vector2 dir) {
        Vector2 constrainedDir = new Vector2(dir.x, 0);

        if (isInflated) {
            if (currAir > 0 &&
                dir != Vector2.zero) {
                MoveForce(dir);
                currAir -= _moveCostMultiplier * Time.deltaTime;
            }
            else MoveForce(constrainedDir);
        }
        else {
            MoveForce(constrainedDir);
        }
    }

    private void MoveForce(Vector2 dir) {
        foreach (Rigidbody2D rb in _softBody.nodes_rb) {
            if (rb.transform.position.y >= _softBody.transform.position.y)
                rb.AddForce(dir * (_movementMultiplier * 100 * Time.deltaTime), ForceMode2D.Force);
            else rb.AddForce(dir * (_movementMultiplier * 50 * Time.deltaTime), ForceMode2D.Force);
        }
    }

    private void Inflate() {
        if (currAir > 0) {
            _gamepad.SetMotorSpeeds(_inflateShake.x, _inflateShake.y);

            foreach (Rigidbody2D rb in _softBody.nodes_rb) {
                rb.gravityScale = 0;
            }

            isInflated = true;
            _softBody.radius = SetSlimeRadius();
            _softBody.frequency = _maxFrequency;
        }
    }

    private void Deflate() {
        foreach (Rigidbody2D rb in _softBody.nodes_rb) {
            rb.gravityScale = 1;
        }

        isInflated = false;
        _softBody.radius = _startRadius;
        _softBody.frequency = _startFrequency;
        _gamepad.PauseHaptics();
    }

    private void Dash() {
        if (currAir > 0 && inputHandler.aimInput != Vector2.zero) {
            foreach (Rigidbody2D rb in _softBody.nodes_rb) {
                rb.linearVelocity *= 0.5f;
            }

            foreach (Rigidbody2D rb in _softBody.nodes_rb) {
                rb.AddForce(inputHandler.aimInput * _dashForceMultiplier, ForceMode2D.Impulse);
            }

            currAir -= _dashCost;
        }
    }

    private bool Grounded() {
        foreach (SoftBodyNode node in _softBody.node_scripts) {
            if (node.touchingGround) return true;
        }

        return false;
    }

    private float SetSlimeRadius() {
        float airRatio = currAir / maxAir;
        float radiusDiff = _maxRadius - _startRadius;

        return (airRatio * radiusDiff) + _startRadius;
    }

    public void AirRefill(float amount) {
        currAir += amount;
        if (currAir > maxAir) currAir = maxAir;
    }
}