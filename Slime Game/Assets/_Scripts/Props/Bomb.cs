using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private bool triggered = false;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionForce;
    [SerializeField] private float countDown;
    [SerializeField] private Material _flashMaterial;
    private Material personal_flashMaterial;
    private float currentTime;
    private float initialCountDown;
    private Rigidbody2D rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        initialCountDown = countDown;
        personal_flashMaterial = new Material(_flashMaterial);
        personal_flashMaterial.name = "personal_flash";
        personal_flashMaterial.SetFloat("_GlowRadius", 0);
        GetComponentInChildren<SpriteRenderer>().material = personal_flashMaterial;
    }

    private void Update()
    {
        if (triggered)
        {
            countDown -= Time.deltaTime;
            
            // flash speed increases as countdown decreases
            float flashSpeed = Mathf.Lerp(0.5f, 10f, 1 - (countDown / initialCountDown));

            float glowStrength = Mathf.Abs(Mathf.Sin((initialCountDown - countDown) * flashSpeed));

            // Apply glow effect
            personal_flashMaterial.SetFloat("_GlowRadius", glowStrength * 1.5f);
        }
        if (countDown <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }
    
    public void Explode()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero, 0);
        foreach (RaycastHit2D h in hit)
        {
            Rigidbody2D rb = h.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(explosionForce * dir, ForceMode2D.Impulse);
                Debug.Log(rb.gameObject.name);
            }
        }
        
        Debug.Log("Exploded");
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("Platform"))
        {
            if (!triggered)
            {
                triggered = true;
            }
        }
    }
}
