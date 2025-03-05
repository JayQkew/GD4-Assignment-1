using System;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable;
    public GameObject grabbableObject;
    public Rigidbody2D rb;

    public void Grab(bool grab)
    {
        if (touchingGrabbable && grab)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
        else if (!grab)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
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
            touchingGrabbable = false;
            grabbableObject = null;
        }
    }
}

public interface IGrabbable
{
    void GrabbableStay();

    void GrabbableExit();

    void OnGrab();
}