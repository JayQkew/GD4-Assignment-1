using System;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable;
    public GameObject grabbableObject;
    public Rigidbody2D rb;

    public void Grab(bool grab, Movement movement)
    {
        // Vector3 dif = transform.position - grabbableObject.transform.position;
        // transform.position = grabbableObject.transform.position + dif;

        if (grab)
        {
            if (touchingGrabbable)
            {
                movement.isBigSlime = grabbableObject.CompareTag("Player");
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            else  rb.bodyType = RigidbodyType2D.Dynamic;
            
            if(rb.bodyType == RigidbodyType2D.Kinematic) rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            movement.isBigSlime = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            touchingGrabbable = true;
            grabbableObject = other.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                touchingGrabbable = false;
                grabbableObject = null;
            }
        }
    }
}

public interface IGrabbable
{
    void GrabbableStay();

    void GrabbableExit();

    void OnGrab();
}