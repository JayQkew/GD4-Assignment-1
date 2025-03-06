using System;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable;
    public GameObject grabbableObject;
    public Rigidbody2D rb;
    private bool grabbing;

    public void Grab(bool grab)
    {
        // Vector3 dif = transform.position - grabbableObject.transform.position;
        // transform.position = grabbableObject.transform.position + dif;

        if (grab) rb.bodyType = touchingGrabbable ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        else rb.bodyType = RigidbodyType2D.Dynamic;
        grabbing = grab;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            touchingGrabbable = true;
            grabbableObject = other.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            
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