using Unity.VisualScripting;
using UnityEngine;

public class TrainingDataPickup : MonoBehaviour
{
    [SerializeField] private float pullDistance = 3f;

    [SerializeField] private float pullSpeed = 3f;
    [SerializeField] private float pickupDistance = 1f;
    private float trainingDataAmount;
    private Transform playerTransform;
    public void SetTrainingDataAmount(float amount)
    {
        trainingDataAmount = amount;
    }

    void Awake()
    {
        var player = FindAnyObjectByType<PlayerMovement>();

        playerTransform = player != null ? player.transform : null;
    }

    void Update()
    {
        Loop();
    }

    void Loop()
    {
        if (playerTransform == null)
        {
            return;
        }

        var distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance < pullDistance)
        {
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, pullSpeed * Time.deltaTime / Vector2.Distance(transform.position, playerTransform.position));
        }

        if (distance < pickupDistance)
        {
            TrainingDataManager.instance.IncreaseTrainingData(trainingDataAmount);
            Destroy(gameObject);
        }
    }

}