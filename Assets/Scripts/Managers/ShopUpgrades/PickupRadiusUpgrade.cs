
using UnityEngine;

public class PickupRadiusUpgrade : Upgrade
{
    [SerializeField] private float radiusIncreasePerLevel = 0.2f;

    public float GetRadiusMultiplier()
    {
        return 1f + radiusIncreasePerLevel * Level;
    }
}
