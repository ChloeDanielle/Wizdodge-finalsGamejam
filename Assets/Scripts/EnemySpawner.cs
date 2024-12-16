using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Reference to the enemy prefab
    public Transform spawnArea;     // Reference to the spawn area (outer ellipse)
    public Transform enemyBoundary; // Reference to the stopping boundary (inner ellipse)
    public float initialSpawnInterval = 2f; // Initial time between spawns

    public static float currentSpawnInterval; // Static spawn interval to be adjusted dynamically

    private float spawnAreaX;  // Horizontal radius of the spawn area
    private float spawnAreaY;  // Vertical radius of the spawn area
    private float stopAreaX;   // Horizontal radius of the enemy boundary
    private float stopAreaY;   // Vertical radius of the enemy boundary

    private float timer = 0f; // Timer to manually control enemy spawning

    void Start()
    {
        // Initialize spawn interval
        currentSpawnInterval = initialSpawnInterval;

        // Get dimensions of spawn area and stopping area
        spawnAreaX = spawnArea.localScale.x / 2f;
        spawnAreaY = spawnArea.localScale.y / 2f;
        stopAreaX = enemyBoundary.localScale.x / 2f;
        stopAreaY = enemyBoundary.localScale.y / 2f;
    }

    void Update()
    {
        // Manually control the spawn timer
        timer += Time.deltaTime;
        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f; // Reset timer
        }
    }

    void SpawnEnemy()
    {
        // Step 1: Get a random spawn position on the edge of the SpawnArea
        Vector2 spawnPosition = GenerateRandomPositionOnEllipseEdge(spawnAreaX, spawnAreaY);

        // Step 2: Instantiate the enemy at the spawn position
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Step 3: Teleport the enemy instantly to the edge of the EnemyBoundary
        Vector2 stopPosition = GenerateRandomPositionOnEllipseEdge(stopAreaX, stopAreaY);
        enemy.transform.position = stopPosition;
    }

    Vector2 GenerateRandomPositionOnEllipseEdge(float radiusX, float radiusY)
    {
        // Generate a random angle in radians
        float angle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate the position on the ellipse using the parametric equation
        float x = radiusX * Mathf.Cos(angle);
        float y = radiusY * Mathf.Sin(angle);

        return new Vector2(x, y);
    }

    public static void AdjustSpawnInterval(int difficultyLevel)
    {
        // Decrease the spawn interval based on difficulty
        currentSpawnInterval = Mathf.Max(0.5f, 2f - (0.2f * difficultyLevel));
    }
}
