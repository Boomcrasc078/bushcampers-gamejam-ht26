using UnityEngine;

public class RoomIncreaseUpgrade : Upgrade
{
    private float roomAmountMultiplier = 1.5f;

    public float GetRoomIncreaseUpgrade()
    {
        return Mathf.Pow(roomAmountMultiplier, Level);
    }
}