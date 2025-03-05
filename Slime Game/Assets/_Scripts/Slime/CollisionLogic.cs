using System;
using UnityEngine;

public class CollisionLogic : MonoBehaviour, IGrabbable
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("SoftBodyNode"))
        {
            Debug.Log("Player collided with " + other.gameObject.name);
        }
    }

    public void GrabbableStay()
    {
    }

    public void GrabbableExit()
    {
    }

    public void OnGrab()
    {
        
    }
}
