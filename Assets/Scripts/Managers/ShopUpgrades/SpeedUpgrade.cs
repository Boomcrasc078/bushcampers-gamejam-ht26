using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    float speedMultiplier;
    public SpeedUpgrade(float speedMultiplier)
    {
        this.speedMultiplier = speedMultiplier;
    }

    public float GetUpgradeMultiplier()
    {
        return Mathf.Pow(speedMultiplier, Level);
    }
}