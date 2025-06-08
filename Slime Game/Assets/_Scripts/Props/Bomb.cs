using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private bool triggered = false;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private float countDownTime;
    private Material personal_flashMaterial;
    private float countDown;
    private float currentTime;
    public GameObject bombExplodeSound;
    public GameObject bombParticles;

    private void Start() {
        countDown = countDownTime;
        personal_flashMaterial = new Material(_flashMaterial);
        personal_flashMaterial.name = "personal_flash";
        personal_flashMaterial.SetFloat("_GlowRadius", 0);
        GetComponentInChildren<SpriteRenderer>().material = personal_flashMaterial;
        gameObject.SetActive(false);
    }

    private void Update() {
        if (triggered) {
            countDown -= Time.deltaTime;
            Flash();
        }

        if (countDown <= 0) {
            Explode();
            gameObject.SetActive(false);
        }
    }

    private void Flash() {
        // flash speed increases as countdown decreases
        float flashSpeed = Mathf.Lerp(0.5f, 10f, 1 - (countDown / countDownTime));
        float glowStrength = Mathf.Abs(Mathf.Sin((countDownTime - countDown) * flashSpeed));
        personal_flashMaterial.SetFloat("_GlowRadius", glowStrength * 1.5f);
    }

    private void Explode() {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero, 0);
        foreach (RaycastHit2D h in hit) {
            Rigidbody2D rb = h.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null) {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(explosionForce * dir, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDisable() {
        triggered = false;
        countDown = countDownTime;
        personal_flashMaterial.SetFloat("_GlowRadius", 0);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        var b = Instantiate(bombParticles);
        b.transform.position = transform.position;
        Instantiate(bombExplodeSound);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("Platform")) {
            if (!triggered) {
                triggered = true;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}