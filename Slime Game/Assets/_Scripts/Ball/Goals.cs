using System;
using UnityEngine;

public class Goals : MonoBehaviour
{
    public bool isPlayer1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            PointManager.Instance.UpdateScore(isPlayer1 ? 0 : 1);
            Ball.Instance.Respawn();
        }
    }
}
