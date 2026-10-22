
using UnityEngine;

public class PiercingUpgrade : Upgrade
{
    [SerializeField] private int additionalPiercesPerLevel = 1;

    public int GetAdditionalPierces()
    {
        return Level * additionalPiercesPerLevel;
    }
}
