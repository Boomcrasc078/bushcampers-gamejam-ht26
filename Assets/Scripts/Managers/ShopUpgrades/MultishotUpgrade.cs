
using UnityEngine;

public class MultishotUpgrade : Upgrade
{
    [SerializeField] private int additionalProjectilesPerLevel = 1;

    public int GetAdditionalProjectiles()
    {
        return Level * additionalProjectilesPerLevel;
    }
}
