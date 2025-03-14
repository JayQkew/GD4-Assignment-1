using System;
using Unity.VisualScripting;
using UnityEngine;

public class Goals : MonoBehaviour
{
    public Teams team;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            PointManager.Instance.UpdateScore(team);
            PointManager.Instance.RespawnBall();
        }
    }
}