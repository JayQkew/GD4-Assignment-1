using System;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CircleCollider2D))]
public class Refill : MonoBehaviour
{
    public float refillAmount;
    
    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            collision.gameObject.GetComponent<Movement>().AirRefill(refillAmount);
            gameObject.SetActive(false);
        }
    }
}