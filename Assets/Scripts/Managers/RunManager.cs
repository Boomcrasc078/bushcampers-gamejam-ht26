
using System;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    [Serializable]
    public class EnemySpawnEntry
    {
        public EnemyDefinition enemy;
        [Min(0f)] public float unlockTime;
    }

    [Header("Enemy Types")]
    [SerializeField] private EnemySpawnEntry[] enemyTypes;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int startingSpawnCount = 3;
    [SerializeField] private int maxAliveEnemies = 100;

    [Header("Difficulty")]
    [SerializeField] private float difficultyIncreaseInterval = 30f;
    [SerializeField] private int extraEnemiesPerIncrease = 1;

    private EnemySpawner enemySpawner;
    private float elapsedTime;
    private float spawnTimer;

    private int enemiesKilled;

    public float ElapsedTime => elapsedTime;
    public int EnemiesKilled => enemiesKilled;

    public void RegisterEnemyKill()
    {
        enemiesKilled++;
    }


    public void Initialize(EnemySpawner spawner)
    {
        enemySpawner = spawner;

        elapsedTime = 0f;
        spawnTimer = 0f;
        enemiesKilled = 0;

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
        if (enemyTypes == null || enemyTypes.Length == 0)
            return;

        // increase spawn count
        int difficultyLevel = Mathf.FloorToInt(
            elapsedTime / Mathf.Max(1f, difficultyIncreaseInterval)
        );

        int spawnCount = startingSpawnCount +
            difficultyLevel * extraEnemiesPerIncrease;

     
        //check if too many enemies :P, SHOULD THIS BE CHANGABLE WITH UPGRADES????????
        int aliveEnemies = FindObjectsByType<EnemyMovement>().Length;

        int availableSlots = maxAliveEnemies - aliveEnemies;
        spawnCount = Mathf.Min(spawnCount, availableSlots);

        for (int i = 0; i < spawnCount; i++)
        {
            EnemyDefinition selectedEnemy = ChooseEnemy();

            if (selectedEnemy == null)
                break;

            enemySpawner.Spawn(selectedEnemy, 1);
        }
    }

    private EnemyDefinition ChooseEnemy()
    {
        float totalWeight = 0f;


        //calc combine weight of enmey
        foreach (EnemySpawnEntry entry in enemyTypes)
        {
            if (entry.enemy == null)
                continue;

            if (elapsedTime < entry.unlockTime)
                continue;

            totalWeight += Mathf.Max(0f, entry.enemy.spawnWeight);
        }

        if (totalWeight <= 0f)
            return null;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        //select enemy based on weight  -> FATTY
        foreach (EnemySpawnEntry entry in enemyTypes)
        {
            if (entry.enemy == null)
                continue;

            if (elapsedTime < entry.unlockTime)
                continue;

            float weight = Mathf.Max(0f, entry.enemy.spawnWeight);

            if (weight <= 0f)
                continue;

            randomValue -= weight;

            if (randomValue <= 0f)
                return entry.enemy;
        }

        return null;
    }
}
