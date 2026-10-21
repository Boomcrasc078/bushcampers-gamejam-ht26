using UnityEngine;

public class HealthUpgrade : Upgrade {
    [SerializeField]float healthIncrease = 1f;

    public float GetHealthIncrease()
    {
        return healthIncrease * Level;
    }
}