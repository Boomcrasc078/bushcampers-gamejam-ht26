using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Video;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private float startingHealth = 10f;
    private float healthAmount;
    private HealthUpgrade healthUpgrade;
    [Header("Audio")][SerializeField] private Sound hurtSound;
    [SerializeField] private Sound deathSound;

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
        healthAmount = startingHealth;
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
        if (onDeath != null)
        {
            onDeath.OnDeath();

        }
        Destroy(mainGameObject);
    }

    public void Initialize(float maxHealth)
    {
        healthAmount = maxHealth;
    }
}