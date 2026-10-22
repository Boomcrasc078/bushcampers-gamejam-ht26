using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public int Level => level;
    private int level = 0;

    public void Levelup()
    {
        level++;
    }

    public void Levelup(int amount)
    {
        level += amount;
    }

    public void SetLevel(int newLevel)
    {
        level = Mathf.Max(0, newLevel);
    }

}