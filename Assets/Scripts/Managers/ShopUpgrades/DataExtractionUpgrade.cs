
using UnityEngine;

public class DataExtractionUpgrade : Upgrade
{
    [SerializeField] private float dataIncreasePerLevel = 0.15f;

    public float GetDataMultiplier()
    {
        return 1f + dataIncreasePerLevel * Level;
    }
}
