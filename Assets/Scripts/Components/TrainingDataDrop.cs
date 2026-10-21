using UnityEngine;

public class TrainingDataDrop : MonoBehaviour, IOnDeath
{
    [SerializeField] private float traningDataDropAmount = 128f;
    [SerializeField] private GameObject trainingDataPickupPrefab;
    public void OnDeath()
    {
        DropTrainingData();
    }

    public void DropTrainingData()
    {
        var spawned = Instantiate(trainingDataPickupPrefab, transform.position, new Quaternion());
        var trainingDataPickup = spawned.GetComponent<TrainingDataPickup>();
        trainingDataPickup.SetTrainingDataAmount(traningDataDropAmount);
    }
}