using UnityEngine;

public class RunManager : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private EnemyDefinition basicEnemy;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int startingSpawnCount = 3;
    [SerializeField] private int maxAliveEnemies = 100;


    [Header("Difficukty")]
    [SerializeField] private float difficultyIncreaseInterval = 30f;
    [SerializeField] private int extraEnemiesPerIncrease = 1;

    private EnemySpawner enemySpawner;
    private float elapsedTime;
    private float spawnTimer;

    public void Initialize(EnemySpawner spawner)
    {
        enemySpawner = spawner;

        elapsedTime = 0f;
        spawnTimer = 0f;

        SpawnEnemies();
    }

    private void Update()
    {
        if (enemySpawner == null)
            return;

        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer < spawnInterval)
            return;

        spawnTimer = 0f;
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (basicEnemy == null || difficultyIncreaseInterval <= 0f)
            return;

        int difficultyLevel = Mathf.FloorToInt(elapsedTime / difficultyIncreaseInterval);

        int spawnCount = startingSpawnCount + difficultyLevel * extraEnemiesPerIncrease;

        //check pop

        int aliveEnemies = FindObjectsByType<EnemyMovement>().Length;

        int avaliableSlots = maxAliveEnemies - aliveEnemies;
        spawnCount = Mathf.Min(spawnCount, avaliableSlots);

        if (spawnCount > 0)
            enemySpawner.Spawn(basicEnemy, spawnCount);
    }
}
