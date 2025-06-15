using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private CircleCollider2D col;

    private void Awake() {
        col = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D other) {
        col.isTrigger = false;
        Debug.Log("Projectile not trigger anymore");
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
