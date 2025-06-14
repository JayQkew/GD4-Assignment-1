using UnityEngine;
using UnityEngine.Serialization;

public class Goals : MonoBehaviour
{
    [FormerlySerializedAs("team")] 
    public int scoringTeam;
    public GameObject pufferfishBurst;
    public AudioSource goalBurst;
    public AudioSource goalPing;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Ball")) {
            PointManager.Instance.Score(scoringTeam);
            GameObject burst = Instantiate(pufferfishBurst, transform, false);
            burst.transform.localPosition = Vector3.zero;
            goalBurst.Play();
            goalPing.Play();
        }
    }
}