using UnityEngine;

public class SpecialDropRateUpgrade : Upgrade
{
    [SerializeField] private float chanceIncreasePerLevel = 0.15f;

    public float GetChanceMultiplier()
    {
        return 1f + chanceIncreasePerLevel * Level;
    }
}
