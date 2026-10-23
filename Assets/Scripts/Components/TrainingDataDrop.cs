
using UnityEngine;

public class TrainingDataDrop : MonoBehaviour, IOnDeath
{
    [SerializeField] private GameObject trainingDataPickupPrefab;

    [Header("Special Drops")]
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField, Range(0f, 1f)] private float healthDropChance = 0.06f;

    [SerializeField] private GameObject overloadPulsePickupPrefab;
    [SerializeField, Range(0f, 1f)] private float overloadPulseDropChance = 0.01f;

    [Header("Drop Scatter")]
    [SerializeField, Min(0f)] private float dropScatterRadius = 0.45f;

    private float trainingDataDropAmount;
    private RunManager runManager;
    private DataExtractionUpgrade dataExtractionUpgrade;
    private SpecialDropRateUpgrade specialDropRateUpgrade;

    private void Awake()
    {
        runManager = FindAnyObjectByType<RunManager>();

        if (ShopUpgradeManager.instance != null)
        {
            dataExtractionUpgrade = ShopUpgradeManager.instance.GetUpgrade<DataExtractionUpgrade>();
            specialDropRateUpgrade = ShopUpgradeManager.instance.GetUpgrade<SpecialDropRateUpgrade>();
        }
    }

    public void Initialize(float amount)
    {
        trainingDataDropAmount = Mathf.Max(0f, amount);
    }

    public void OnDeath()
    {
        if (runManager != null)
            runManager.RegisterEnemyKill();

        float chanceMultiplier = specialDropRateUpgrade != null ? specialDropRateUpgrade.GetChanceMultiplier() : 1f;

        bool dropHealth = healthPickupPrefab != null && Random.value < Mathf.Clamp01(healthDropChance * chanceMultiplier);
        bool dropPulse = overloadPulsePickupPrefab != null && Random.value < Mathf.Clamp01(overloadPulseDropChance * chanceMultiplier);

        int dropCount = 1 + (dropHealth ? 1 : 0) + (dropPulse ? 1 : 0);
        float randomAngle = Random.Range(0f, 360f);
        int dropIndex = 0;

        DropTrainingData(GetDropPosition(dropIndex++, dropCount, randomAngle));

        if (dropHealth)
            Instantiate(healthPickupPrefab, GetDropPosition(dropIndex++, dropCount, randomAngle), Quaternion.identity);

        if (dropPulse)
            Instantiate(overloadPulsePickupPrefab, GetDropPosition(dropIndex++, dropCount, randomAngle), Quaternion.identity);
    }

    public void DropTrainingData()
    {
        DropTrainingData(transform.position);
    }

    private void DropTrainingData(Vector3 position)
    {
        float dataMultiplier = dataExtractionUpgrade != null ? dataExtractionUpgrade.GetDataMultiplier() : 1f;
        float comboMultiplier = runManager != null ? runManager.ComboMultiplier : 1f;
        float finalAmount = Mathf.Round(trainingDataDropAmount * dataMultiplier * comboMultiplier);

        GameObject spawned = Instantiate(trainingDataPickupPrefab, position, Quaternion.identity);
        TrainingDataPickup pickup = spawned.GetComponent<TrainingDataPickup>();
        pickup.SetTrainingDataAmount(finalAmount);
    }

    private Vector3 GetDropPosition(int index, int total, float randomAngle)
    {
        float angle = (randomAngle + 360f * index / total) * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * dropScatterRadius;
        return transform.position + offset;
    }
}
