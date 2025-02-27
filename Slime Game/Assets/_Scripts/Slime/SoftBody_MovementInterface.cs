using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(SoftBody))]
public class SoftBody_MovementInterface : MonoBehaviour
{
    private SoftBody _softBody;
    
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    private void Awake()
    {
        _softBody = GetComponent<SoftBody>();
    }

    public void Move(Vector2 dir)
    {
        foreach (GameObject node in _softBody.nodes)
        {
            if (dir.y > 0)
            {
                if (node.transform.position.y >= transform.position.y)
                {
                    Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                    rb.AddForce(dir * _movementSpeed, ForceMode2D.Force);
                }
            }

            if (dir.y < 0)
            {
                if (node.transform.position.y <= transform.position.y)
                {
                    Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                    rb.AddForce(dir * _movementSpeed, ForceMode2D.Force);
                }
            }

            if (dir.x < 0)
            {
                if (node.transform.position.x >= transform.position.x)
                {
                    Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                    rb.AddForce(dir * _movementSpeed, ForceMode2D.Force);
                }
            }

            if (dir.x > 0)
            {
                if (node.transform.position.x >= transform.position.x)
                {
                    Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                    rb.AddForce(dir * _movementSpeed, ForceMode2D.Force);
                }
            }
        }
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
            _softBody.frequency = 5f;
        }
        else
        {
            foreach (GameObject node in _softBody.nodes)
            {
                Rigidbody2D rb = node.GetComponent<Rigidbody2D>();
                rb.gravityScale = 1f;
                rb.mass = 1f;
            }

            _softBody.frequency = 1f;
        }
    }
}
