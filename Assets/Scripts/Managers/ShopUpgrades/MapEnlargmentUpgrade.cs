using UnityEngine;
public class MapEnlargmentUpgrade : Upgrade
{
    [SerializeField] private float mapSizeMultiplier = 1.3f;

    public float GetMapSizeUpgrade()
    {
        return Mathf.Pow(mapSizeMultiplier, Level);
    }
}