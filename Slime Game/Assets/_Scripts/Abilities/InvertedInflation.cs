using System;
using UnityEngine;
using UnityEngine.Rendering;

public class InvertedInflation : MonoBehaviour
{
    // make the player stay inflated
    // when trigger is pressed, the player shrinks
    private Movement playerMovement;
    private void Awake() {
        //find the player Input Handler
        playerMovement = transform.parent.parent.GetComponentInChildren<Movement>();
        Debug.Log(playerMovement ? "Found Player Movement" : "Couldn't Find Player Movement");

        InputHandler inputHandler = transform.parent.GetComponentInParent<InputHandler>();
        Debug.Log(inputHandler ? "Found Input Handler" : "Couldn't Find Input Handler");
        
        inputHandler.onInflate.RemoveListener(playerMovement.Inflate);
        inputHandler.onInflate.AddListener(playerMovement.Deflate);
        
        inputHandler.onDeflate.RemoveListener(playerMovement.Deflate);
        inputHandler.onDeflate.AddListener(playerMovement.Inflate);
        
        playerMovement.moveConsumeFuel = false;
    }
}
