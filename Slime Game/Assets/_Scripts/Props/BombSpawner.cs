using UnityEngine;
using Random = UnityEngine.Random;

public class BombSpawner : MonoBehaviour
{
    public SpawnerType spawnerType;
    [Header("Bombs")]
    public GameObject bombPrefab;
    [SerializeField] private int liveBombs;
    [SerializeField] private int maxLiveBombs;
    [SerializeField] private float spawnForce;
    
    [Header("Timer")] 
    [SerializeField] private float currTime;
    [SerializeField] private float spawnTime;
    

    private void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= spawnTime)
        {
            SpawnBomb();
            currTime = 0;
        }
    }

    private void SpawnBomb()
    {
        Vector2 dir = Vector2.zero;
        switch (spawnerType)
        {
            case SpawnerType.Radial:
                //get random direction 360°
                float randAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                dir = new Vector2(Mathf.Cos(randAngle), Mathf.Sin(randAngle));
                break;
            case SpawnerType.Directional:
                break;
            case SpawnerType.Cone:
                break;
        }
        
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.GetComponent<Rigidbody2D>().AddForce(dir * spawnForce, ForceMode2D.Impulse);
    }

    public enum SpawnerType
    {
        Radial,
        Directional,
        Cone
    }
}
