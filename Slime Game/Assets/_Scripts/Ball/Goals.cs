using System;
using Unity.VisualScripting;
using UnityEngine;

public class Goals : MonoBehaviour
{
    public Teams team;
    public GameObject pufferfishBurst;
    public AudioSource goalBurst;
    public AudioSource goalPing;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            PointManager.Instance.UpdateScore(team);
            PointManager.Instance.RespawnBall();
            PointManager.Instance.CheckScores(team);
            GameObject burst=Instantiate(pufferfishBurst, transform, false);
            burst.transform.localPosition = Vector3.zero;
            goalBurst.Play();
            goalPing.Play();

        }
    }
}