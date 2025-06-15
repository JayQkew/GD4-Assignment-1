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
    private InputHandler inputHandler;
    private PlayerStats playerStats;

    public bool moveConsumeFuel = true;
    public bool dashConsumeFuel = true;
    public float currFuel;
    [SerializeField] private bool isInflated;

    private void Awake() {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start() {
        currFuel = playerStats.GetStatValue(StatName.Fuel);
        inputHandler.onInflate.AddListener(Inflate);
        inputHandler.onDeflate.AddListener(Deflate);

        inputHandler.onDash.AddListener(Dash);
    }

    private void Update() {
        Move(inputHandler.moveInput);
        if (Grounded()) currFuel = playerStats.GetStatValue(StatName.Fuel);
        if (isInflated) _softBody.currRadius = SetSlimeRadius();
    }

    private void Move(Vector2 dir) {
        Vector2 constrainedDir = new Vector2(dir.x, 0);

        if (isInflated) {
            if (currFuel > 0 && dir != Vector2.zero) {
                MoveForce(dir);
                if(moveConsumeFuel) currFuel -= playerStats.GetStatValue(StatName.MoveCost) * Time.deltaTime;
            }
            // else MoveForce(constrainedDir);
        }
        else {
            MoveForce(constrainedDir);
        }
    }

    private void MoveForce(Vector2 dir) {
        float moveMult = playerStats.GetStatValue(StatName.MoveSpeed);
        foreach (Rigidbody2D rb in _softBody.nodesRb) {
            if (rb.transform.position.y >= _softBody.transform.position.y)
                rb.AddForce(dir * (moveMult * 100 * Time.deltaTime), ForceMode2D.Force);
            else rb.AddForce(dir * (moveMult * 50 * Time.deltaTime), ForceMode2D.Force);
        }
    }

    public void Inflate() {
        foreach (Rigidbody2D rb in _softBody.nodesRb) {
            rb.gravityScale = 0;
        }

        isInflated = true;
        _softBody.currRadius = SetSlimeRadius();
        _softBody.frequency = playerStats.GetStatValue(StatName.MaxFrequency);
    }

    public void Deflate() {
        foreach (Rigidbody2D rb in _softBody.nodesRb) {
            rb.gravityScale = 1;
        }

        isInflated = false;
        _softBody.currRadius = playerStats.GetStatValue(StatName.MinRadius);
        _softBody.frequency = playerStats.GetStatValue(StatName.MinFrequency);
    }

    private void Dash() {
        if (currFuel > 0 && inputHandler.aimInput != Vector2.zero) {
            foreach (Rigidbody2D rb in _softBody.nodesRb) {
                rb.linearVelocity *= 0.25f;
                rb.AddForce(inputHandler.aimInput * playerStats.GetStatValue(StatName.DashForce), ForceMode2D.Impulse);
            }
            if (dashConsumeFuel) currFuel -= playerStats.GetStatValue(StatName.DashCost);
        }
    }

    private bool Grounded() {
        foreach (SoftBodyNode node in _softBody.nodeScripts) {
            if (node.touchingGround) return true;
        }
        return false;
    }

    private float SetSlimeRadius() {
        float minRadius = playerStats.GetStatValue(StatName.MinRadius);
        float radiusDiff = playerStats.GetStatValue(StatName.MaxRadius) - minRadius;
        return radiusDiff + minRadius;
    }

    public void AirRefill(float amount) {
        float maxFuel = playerStats.GetStatValue(StatName.Fuel);
        currFuel += amount;
        if (currFuel > maxFuel) currFuel = maxFuel;
    }
}