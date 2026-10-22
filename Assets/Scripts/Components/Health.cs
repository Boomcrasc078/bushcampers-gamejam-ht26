using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Video;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private float startingHealth = 10f;

    private float healthAmount;
    private HealthUpgrade healthUpgrade;
    private bool isDead = false;

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
        if (isDead)
            return;

        healthAmount -= damage;

        if (healthAmount <= 0)
        {
            healthAmount = 0f;
            isDead = true;
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
        isDead = false;
        startingHealth = maxHealth;
        healthAmount = startingHealth + GetHealthUpgrade();
    }
}