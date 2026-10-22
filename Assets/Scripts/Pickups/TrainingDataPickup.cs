
using UnityEngine;

[RequireComponent(typeof(PickupMagnet))]
public class TrainingDataPickup : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 1f;

    private float trainingDataAmount;
    private PickupMagnet magnet;

    private void Awake()
    {
        magnet = GetComponent<PickupMagnet>();
    }

    public void SetTrainingDataAmount(float amount)
    {
        trainingDataAmount = amount;
    }

    private void Update()
    {
        Transform playerTransform = magnet.PlayerTransform;

        if (playerTransform == null || TrainingDataManager.instance == null)
            return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance >= pickupDistance)
            return;

        TrainingDataManager.instance.IncreaseTrainingData(trainingDataAmount);
        Destroy(gameObject);
    }
}
