using System;
using System.Collections.Generic;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGround;
    public bool touchingSurface;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) touchingGround = true;
        if (other.gameObject.CompareTag("Platform")) touchingSurface = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) touchingGround = false;
        if (other.gameObject.CompareTag("Platform")) touchingSurface = false;
    }
}