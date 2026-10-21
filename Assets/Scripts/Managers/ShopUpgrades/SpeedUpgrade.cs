using UnityEngine;

public class SpeedUpgrade : Upgrade
{
    [SerializeField]float speedMultiplier = 1.2f;

    public float GetUpgradeMultiplier()
    {
        return Mathf.Pow(speedMultiplier, Level);
    }
}