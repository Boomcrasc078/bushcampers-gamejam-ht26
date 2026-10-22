using UnityEngine;

public class TrainingDataDrop : MonoBehaviour, IOnDeath
{

    [SerializeField] private GameObject trainingDataPickupPrefab;

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
    }

    public void DropTrainingData()
    {
        var spawned = Instantiate(trainingDataPickupPrefab, transform.position, new Quaternion());
        var trainingDataPickup = spawned.GetComponent<TrainingDataPickup>();

        trainingDataPickup.SetTrainingDataAmount(trainingDataDropAmount);
    }
}