using UnityEngine;
using UnityEngine.Video;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;
    private float healthAmount = 10f;
    private HealthUpgrade healthUpgrade;

    void Awake()
    {
        healthUpgrade = GetComponent<HealthUpgrade>();
    }

    float GetHealthUpgrade()
    {
        if (healthUpgrade != null)
        {
            return healthUpgrade.GetHealthIncrease();
        }
        return 0f;
    }

    void Start()
    {
        healthAmount += GetHealthUpgrade();
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        if (healthAmount <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        var onDeath = GetComponent<IOnDeath>();
        onDeath.OnDeath();
        Destroy(mainGameObject);
    }
}