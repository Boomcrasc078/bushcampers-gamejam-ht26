
using UnityEngine;

[RequireComponent(typeof(PickupMagnet))]
public class TrainingDataPickup : MonoBehaviour
{
    [SerializeField] private float pickupDistance = 1f;
    [SerializeField, Min(1f)] private float lifetime = 30f;
    [Header("Audio")]
    [SerializeField] Sound pickupSound;
    private float trainingDataAmount;
    private PickupMagnet magnet;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

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
        AudioManager.instance.PlaySound(pickupSound, AudioManager.instance.gameObject);
        Destroy(gameObject);
    }

}
