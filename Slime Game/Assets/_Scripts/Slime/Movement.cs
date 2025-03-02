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

    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Start()
    {
        _lastPos = transform.position;
    }

    private void Update()
    {
        if (_inputHandler.movementType == InputHandler.MovementType.Movement1)
        {
            AdjustStiffness(_inputHandler.stiffnessInput);
        }
        else if (_inputHandler.movementType == InputHandler.MovementType.Movement2)
        {
            currTime += Time.deltaTime;
            if (currTime >= checkTime)
            {
                CanJump();
                currTime = 0;
            }

            Jump(_inputHandler.jumpInput);
            AdjustStiffness(_inputHandler.stiffnessInput);
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

    private void Jump(bool jumpInput)
    {
        if (jumpInput && _canJump && Grounded())
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
            
            _canJump = false;
        }
    }

    #endregion

    private bool Grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, _softBody.radius + 0.05f,
            LayerMask.GetMask("Ground"));
    }

    private bool CanJump()
    {
        Vector2 displacement = (Vector2)transform.position - _lastPos;

        if (Mathf.Abs(displacement.x) < displacementThreshold && Mathf.Abs(displacement.y) < displacementThreshold)
            _canJump = true;
        else _canJump = false;

        _lastPos = transform.position;
        Debug.Log($"Displacement: {displacement}, CanJump: {_canJump}");

        return _canJump;
    }
}