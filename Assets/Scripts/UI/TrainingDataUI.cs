using UnityEngine;
using TMPro;

public class TrainingDataUI : MonoBehaviour
{
    [SerializeField] private TMP_Text trainingDataText;


    // Update is called once per frame
    void Update()
    {
        trainingDataText.text = TrainingDataManager.instance.TrainingDataFormatted;
    }
}
