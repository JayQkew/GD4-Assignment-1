using System;
using Unity.VisualScripting;
using UnityEngine;

public class Whirlpool : MonoBehaviour
{
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private float innerRadius;
    [SerializeField] private float force;

    private void OnTriggerStay2D(Collider2D other) {
        float distance = Vector2.Distance(transform.position, other.transform.position);
        if (distance >= innerRadius) {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            other.transform.GetComponent<Rigidbody2D>().AddForce(dir * force);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
