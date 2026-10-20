using UnityEngine;

public class Upgrade
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

}