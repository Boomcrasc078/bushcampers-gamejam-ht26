
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

    [Header("Kill Combo")]
    [SerializeField, Min(1f)] private float comboWindow = 6f;
    [SerializeField, Min(0f)] private float comboBonusPerKill = 0.05f;
    [SerializeField, Min(1f)] private float maxComboMultiplier = 1.5f;
    [SerializeField] private int[] comboTierKillThresholds = { 2, 4, 6, 9, 12, 16, 20, 25, 30, 36 };

    private int comboKills;
    private float lastKillTime = float.NegativeInfinity;

    public int ComboKills => comboKills;
    

    private EnemySpawner enemySpawner;
    private float elapsedTime;
    private float spawnTimer;

    private int enemiesKilled;

    public float ElapsedTime => elapsedTime;
    public int EnemiesKilled => enemiesKilled;

    public float ComboDuration => comboWindow;

    public float ComboTimeRemaining => comboKills > 0 ? Mathf.Max(0f, comboWindow - (Time.time - lastKillTime)) : 0f;

    public float ComboMultiplier
    {
        get
        {
            int tier = 0;

            foreach (int threshold in comboTierKillThresholds)
            {
                if (comboKills < threshold)
                    break;

                tier++;
            }

            return Mathf.Min(maxComboMultiplier, 1f + tier * comboBonusPerKill);
        }
    }

    public int ComboTier
    {
        get
        {
            int tier = 0;

            foreach (int threshold in comboTierKillThresholds)
            {
                if (comboKills < threshold)
                    break;

                tier++;
            }

            return tier;
        }
    }

    public int KillsToNextComboTier
    {
        get
        {
            foreach (int threshold in comboTierKillThresholds)
            {
                if (comboKills < threshold)
                    return threshold - comboKills;
            }

            return 0;
        }
    }


    public void RegisterEnemyKill()
    {
        enemiesKilled++;

        if (Time.time - lastKillTime > comboWindow)
            comboKills = 0;

        comboKills++;
        lastKillTime = Time.time;
    }


    public void Initialize(EnemySpawner spawner)
    {
        enemySpawner = spawner;

        elapsedTime = 0f;
        spawnTimer = 0f;
        enemiesKilled = 0;
        comboKills = 0;
        lastKillTime = float.NegativeInfinity;

        SpawnEnemies();
    }

    private void Update()
    {
        if (comboKills > 0 && Time.time - lastKillTime >= comboWindow)
            comboKills = 0;

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
