
using UnityEngine;

public class KnockbackUpgrade : Upgrade
{
    [SerializeField] private float increasePerLevel = 0.25f;

    public float GetKnockbackMultiplier()
    {
        return 1f + increasePerLevel * Level;
    }
}
