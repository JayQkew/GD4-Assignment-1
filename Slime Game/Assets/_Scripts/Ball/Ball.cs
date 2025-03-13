using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Instance {get; private set;}

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public void Respawn() => transform.position = spawnPoint.position;

}
