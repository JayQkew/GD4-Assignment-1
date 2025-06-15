using System;
using UnityEngine;

public class Gills : MonoBehaviour
{
    private Movement playerMovement;

    private void Awake() {
        playerMovement = transform.parent.parent.GetComponentInChildren<Movement>();
        playerMovement.moveConsumeFuel = false;
    }
}
