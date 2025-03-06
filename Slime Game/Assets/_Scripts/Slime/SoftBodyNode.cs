using System;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable;
    public GameObject grabbableObject;
    public Rigidbody2D rb;

    public void Grab(bool grab)
    {
        // if (touchingGrabbable && grab)
        // {
        //     rb.bodyType = RigidbodyType2D.Static;
        //     // This morphs the player into the shape of the sureface
        //     // Vector3 dif = transform.position - grabbableObject.transform.position;
        //     // transform.position = grabbableObject.transform.position + dif;
        //     
        //     
        // }
        // else if (!grab)
        // {
        //     rb.bodyType = RigidbodyType2D.Dynamic;
        // }

        if (grab) rb.bodyType = touchingGrabbable ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        else rb.bodyType = RigidbodyType2D.Dynamic;
        
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
        if (other.gameObject.GetComponent<IGrabbable>() != null && rb.bodyType != RigidbodyType2D.Static)
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