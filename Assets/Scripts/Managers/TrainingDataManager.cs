using UnityEngine;

public class TrainingDataManager : MonoBehaviour
{
    public static TrainingDataManager instance;
    private float trainingData;
    public float TrainingDataUnformatted => trainingData;
    public string TrainingDataFormatted => TrainingDataDisplay.FormatTrainingData(trainingData);

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void IncreaseTrainingData(float amount)
    {
        trainingData += amount;
    }

    public void DecreaseTrainingData(float amount)
    {
        trainingData -= amount;
        if (trainingData < 0)
        {
            trainingData = 0;
        }
    }

    public bool HasEnoughTrainingData(float amount)
    {
        return trainingData >= amount;
    }
}