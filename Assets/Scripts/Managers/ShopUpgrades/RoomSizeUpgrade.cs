
using UnityEngine;

public class RoomSizeUpgrade : Upgrade
{
    [SerializeField] private float roomSizeIncreasePerLevel = 0.08f;

    public float GetRoomSizeMultiplier()
    {
        return 1f + roomSizeIncreasePerLevel * Level;
    }
}
