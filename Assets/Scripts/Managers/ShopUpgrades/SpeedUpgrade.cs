
using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    [SerializeField] private float speedIncreasePerLevel = 0.07f;

    public float GetUpgradeMultiplier()
    {
        return 1f + (speedIncreasePerLevel * Level);
    }
}
