
using UnityEngine;

public class PowerupChanceUpgrade : Upgrade
{
    [SerializeField] private float chanceIncreasePerLevel = 0.05f;

    public float GetChanceBonus()
    {
        return chanceIncreasePerLevel * Level;
    }
}
