using System;
using UnityEngine;

[RequireComponent(typeof(SoftBody))]
public class Movement : MonoBehaviour
{
    private SoftBody _softBody;
    private InputHandler _inputHandler;
    
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    
    [Header("Spring Settings")]
    [SerializeField] private float _maxFrequency;
    [SerializeField] private float _midFrequency;
    [SerializeField] private float _minFrequency;
    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
        _inputHandler = GetComponent<InputHandler>();
    }

    private void Update()
    {
        AdjustStiffness(_inputHandler.stiffnessInput);
    }

    private void FixedUpdate()
    {
        Move(_inputHandler.moveInput);
    }

    public void Move(Vector2 dir)
    {
        foreach (Rigidbody2D rb in _softBody.nodes_rb)
        {
            if (rb.transform.position.y >= _softBody.transform.position.y)
            {
                rb.AddForce(dir * _movementSpeed, ForceMode2D.Force);
            }
        }
    }

    public void AdjustStiffness(int stiffness)
    {
        if (stiffness > 0) _softBody.SetFrequency(_maxFrequency);
        else if (stiffness < 0) _softBody.SetFrequency(_minFrequency);
        else _softBody.SetFrequency(_midFrequency);
    }
    public void AdjustGravity(bool adjust)
    {
        if (adjust)
        {
            foreach (GameObject node in _softBody.nodes)
            {
                Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                rb.gravityScale = 0.5f;
                rb.mass = 0.5f;
            }
            _softBody.frequency = _softBody.maxFrequency;
        }
        else
        {
            foreach (GameObject node in _softBody.nodes)
            {
                Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                rb.mass = 1f;
            }

            _softBody.frequency = _softBody.minFrequency;
        }
    }
}
