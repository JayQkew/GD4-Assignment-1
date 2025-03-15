using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ExplosiveTypes explosiveType;
    private Explosive currentExplosive;
    private Rigidbody2D rb;
    
    [SerializeField] private Explosive[] explosives = new Explosive[3];

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        currentExplosive = explosives[(int)explosiveType];
        Debug.Log((int)explosiveType);
        SetupBomb();
    }

    private void SetupBomb()
    {
        rb.sharedMaterial = currentExplosive.physicsMaterial;
        if (explosiveType == ExplosiveTypes.Mine) rb.gravityScale = 0;
    }

    private void Update()
    {
        if (currentExplosive.triggered) currentExplosive.CountDown();
        if (currentExplosive.countDown <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }
    
    public void Explode()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, currentExplosive.explosionRadius, Vector2.zero, 0);
        foreach (RaycastHit2D h in hit)
        {
            Rigidbody2D rb = h.collider.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(currentExplosive.explosionForce * dir, ForceMode2D.Impulse);
                Debug.Log(rb.gameObject.name);
            }
        }
        
        Debug.Log("Exploded");
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Ground") && !other.gameObject.CompareTag("Platform"))
        {
            if (!currentExplosive.triggered)
            {
                Debug.Log("Bomb ticking");
                currentExplosive.triggered = true;
            }
        }
    }

    private enum ExplosiveTypes
    {
        Normal = 0,
        Mine = 1,
        Bouncy = 2
    }
}
