using UnityEngine;

public class TrainingDataDrop : MonoBehaviour, IOnDeath
{
    [SerializeField] private GameObject trainingDataPickupPrefab;

    [Header("Special Drops")]
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField, Range(0f, 1f)] private float healthDropChance = 0.06f;

    [SerializeField] private GameObject overloadPulsePickupPrefab;
    [SerializeField, Range(0f, 1f)] private float overloadPulseDropChance = 0.01f;

    private float trainingDataDropAmount;

    private RunManager runManager;

    private void Awake()
    {
        runManager = FindAnyObjectByType<RunManager>();
    }

    public void Initialize(float amount)
    {
        trainingDataDropAmount = Mathf.Max(0f, amount);
    }

    public void OnDeath()
    {
        if(runManager != null)
        {
            runManager.RegisterEnemyKill();
        }
        DropTrainingData();
        TrySpawnPickup(healthPickupPrefab, healthDropChance);
        TrySpawnPickup(overloadPulsePickupPrefab, overloadPulseDropChance);
    }

    public void DropTrainingData()
    {
        var spawned = Instantiate(trainingDataPickupPrefab, transform.position, new Quaternion());
        var trainingDataPickup = spawned.GetComponent<TrainingDataPickup>();

        trainingDataPickup.SetTrainingDataAmount(trainingDataDropAmount);
    }

    private void TrySpawnPickup(GameObject prefab, float dropChance)
    {
        if (prefab == null || Random.value >= dropChance)
            return;

        Instantiate(prefab, transform.position, Quaternion.identity);
    }

}