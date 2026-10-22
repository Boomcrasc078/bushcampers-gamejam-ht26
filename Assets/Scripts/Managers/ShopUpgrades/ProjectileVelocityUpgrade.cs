
using UnityEngine;

public class ProjectileVelocityUpgrade : Upgrade
{
    [SerializeField] private float speedIncreasePerLevel = 0.15f;

    public float GetSpeedMultiplier()
    {
        return 1f + speedIncreasePerLevel * Level;
    }
}
