using System;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    private InputHandler _inputHandler;

    [Header("Movement Settings")] [SerializeField]
    private float _movementMultiplier;

    [Header("Spring Settings")] [SerializeField]
    private float _maxFrequency;

    [SerializeField] private float _midFrequency;
    [SerializeField] private float _minFrequency;

    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        if (_inputHandler.movementType == InputHandler.MovementType.Movement1)
        {
            AdjustStiffness(_inputHandler.stiffnessInput);
        }
        else if (_inputHandler.movementType == InputHandler.MovementType.Movement2)
        {
            Jump(_inputHandler.jumpInput);
        }
    }

    private void FixedUpdate()
    {
        if (_inputHandler.movementType == InputHandler.MovementType.Movement1)
        {
            Move(_inputHandler.moveInput);
        }
    }

    #region Movement1

    private void Move(Vector2 dir)
    {
        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            if (rb.transform.position.y >= _softBody.transform.position.y)
            {
                rb.AddForce(dir * _movementMultiplier, ForceMode2D.Force);
            }
        }
    }

    private void AdjustStiffness(int stiffness)
    {
        if (stiffness > 0) _softBody.SetFrequency(_maxFrequency);
        else if (stiffness < 0) _softBody.SetFrequency(_minFrequency);
        else _softBody.SetFrequency(_midFrequency);
    }

    #endregion

    #region Movement2

    private void Aim()
    {
    }

    private void Inflate()
    {
    }

    private void Jump(bool jumpInput)
    {
        if (jumpInput && Grounded())
        {
            foreach (Rigidbody2D rb in _softBody.nodes_rb)
            {
                if (rb.transform.position.y >= _softBody.transform.position.y)
                {
                    rb.AddForce(_inputHandler.aimInput * _movementMultiplier, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(_inputHandler.aimInput * (_movementMultiplier * 0.5f), ForceMode2D.Impulse);
                }
            }
        }
    }

    #endregion

    private bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _softBody.radius + 0.05f,
            LayerMask.GetMask("Ground"));
    }
}