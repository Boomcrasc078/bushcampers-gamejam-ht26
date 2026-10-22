
using UnityEngine;

public class MapEnlargmentUpgrade : Upgrade
{
    [SerializeField] private float mapIncreasePerLevel = 0.1f;

    public float GetMapSizeUpgrade()
    {
        return 1f + mapIncreasePerLevel * Level;
    }
}
