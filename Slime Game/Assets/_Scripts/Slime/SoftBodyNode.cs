using System;
using System.Collections.Generic;
using UnityEngine;

public class SoftBodyNode : MonoBehaviour
{
    public bool touchingGround;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) touchingGround = true;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")) touchingGround = false;
    }
}