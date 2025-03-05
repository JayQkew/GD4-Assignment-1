using System;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGrabbable = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<IGrabbable>() != null)
        {
            Debug.Log(other.gameObject.name);
        }
    }
}

public interface IGrabbable
{
    void GrabbableStay();
    
    void GrabbableExit();
    
    void OnGrab();
}
