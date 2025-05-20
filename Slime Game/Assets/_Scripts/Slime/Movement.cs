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

    [SerializeReference] private float currFuel;
    [SerializeField] private bool isInflated;

    private void Awake() {
        _softBody = GetComponent<SoftBody>();
        inputHandler = transform.parent.GetComponent<InputHandler>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start() {
        currFuel = playerStats.GetStat(StatName.Fuel);
        inputHandler.onInflate.AddListener(Inflate);
        inputHandler.onDeflate.AddListener(Deflate);

        inputHandler.onDash.AddListener(Dash);
    }

    private void Update() {
        Move(inputHandler.moveInput);
        if (Grounded()) currFuel = playerStats.GetStat(StatName.Fuel);
        if (isInflated) _softBody.currRadius = SetSlimeRadius();
    }

    private void Move(Vector2 dir) {
        Vector2 constrainedDir = new Vector2(dir.x, 0);

        if (isInflated) {
            if (currFuel > 0 && dir != Vector2.zero) {
                MoveForce(dir);
                currFuel -= playerStats.GetStat(StatName.MoveCost) * Time.deltaTime;
            }
            else MoveForce(constrainedDir);
        }
        else {
            MoveForce(constrainedDir);
        }
    }

    private void MoveForce(Vector2 dir) {
        float moveMult = playerStats.GetStat(StatName.MoveSpeed);
        foreach (Rigidbody2D rb in _softBody.nodes_rb) {
            if (rb.transform.position.y >= _softBody.transform.position.y)
                rb.AddForce(dir * (moveMult * 100 * Time.deltaTime), ForceMode2D.Force);
            else rb.AddForce(dir * (moveMult * 50 * Time.deltaTime), ForceMode2D.Force);
        }
    }

    private void Inflate() {
        foreach (Rigidbody2D rb in _softBody.nodes_rb) {
            rb.gravityScale = 0;
        }

        isInflated = true;
        _softBody.currRadius = SetSlimeRadius();
        _softBody.frequency = playerStats.GetStat(StatName.MaxFrequency);
    }

    private void Deflate() {
        foreach (Rigidbody2D rb in _softBody.nodes_rb) {
            rb.gravityScale = 1;
        }

        isInflated = false;
        _softBody.currRadius = playerStats.GetStat(StatName.MinRadius);
        _softBody.frequency = playerStats.GetStat(StatName.MinFrequency);
    }

    private void Dash() {
        if (currFuel > 0 && inputHandler.aimInput != Vector2.zero) {
            foreach (Rigidbody2D rb in _softBody.nodes_rb) {
                rb.linearVelocity *= 0.25f;
                rb.AddForce(inputHandler.aimInput * playerStats.GetStat(StatName.DashForce), ForceMode2D.Impulse);
            }
            currFuel -= playerStats.GetStat(StatName.DashCost);
        }
    }

    private bool Grounded() {
        foreach (SoftBodyNode node in _softBody.node_scripts) {
            if (node.touchingGround) return true;
        }
        return false;
    }

    private float SetSlimeRadius() {
        float minRadius = playerStats.GetStat(StatName.MinRadius);
        float radiusDiff = playerStats.GetStat(StatName.MaxRadius) - minRadius;
        return radiusDiff + minRadius;
    }

    public void AirRefill(float amount) {
        float maxFuel = playerStats.GetStat(StatName.Fuel);
        currFuel += amount;
        if (currFuel > maxFuel) currFuel = maxFuel;
    }
}