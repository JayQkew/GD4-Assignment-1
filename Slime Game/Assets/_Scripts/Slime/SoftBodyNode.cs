using System;
using System.Collections.Generic;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable;
    public bool touchingGround = false;
    public Rigidbody2D rb;

    public void Grab()
    {
        if (touchingGrabbable) rb.bodyType = RigidbodyType2D.Kinematic;
        if (rb.bodyType == RigidbodyType2D.Kinematic) rb.linearVelocity = Vector2.zero;
    }

    public void Release()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null) touchingGrabbable = true;

        if (other.gameObject.CompareTag("Ground")) touchingGround = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic) touchingGrabbable = false;
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic) touchingGround = false;
        }
    }
}

public interface IGrabbable
{
    void GrabbableStay();

    void GrabbableExit();

    void OnGrab();
}