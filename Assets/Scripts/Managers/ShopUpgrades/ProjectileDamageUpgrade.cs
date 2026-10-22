
using UnityEngine;

public class ProjectileDamageUpgrade : Upgrade
{
    [SerializeField] private float damageIncreasePerLevel = 0.15f;

    public float GetDamageMultiplier()
    {
        return 1f + damageIncreasePerLevel * Level;
    }
}
