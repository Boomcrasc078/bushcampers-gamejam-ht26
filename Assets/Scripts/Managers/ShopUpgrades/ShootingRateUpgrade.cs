using UnityEngine;

public class ShootingRateUpgrade : Upgrade
{
    [SerializeField] private float shootingRateMultiplier = 1.1f;

    public float GetShootingRateUpgrade()
    {
        return Mathf.Pow(shootingRateMultiplier, Level);
    }

}