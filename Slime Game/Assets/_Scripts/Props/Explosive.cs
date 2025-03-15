using System;
using UnityEngine;

[Serializable]
public class Explosive
{
    public bool triggered = false;
    public float explosionRadius;
    public float explosionForce;
    public float countDown;
    public PhysicsMaterial2D physicsMaterial;
    private float currentTime;

    public float CountDown()
    {
        return countDown -= Time.deltaTime;
    }


}
