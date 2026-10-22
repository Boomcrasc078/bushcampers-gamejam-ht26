using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private float startingHealth = 10f;
    [SerializeField] private bool usePermanentHealthUpgrade = false;


    private float healthAmount;
    private bool isDead = false;



    private float GetHealthUpgrade()
    {
        if (!usePermanentHealthUpgrade)
            return 0f;

        if (ShopUpgradeManager.instance == null)
            return 0f;

        HealthUpgrade upgrade =
            ShopUpgradeManager.instance.GetUpgrade<HealthUpgrade>();

        return upgrade != null
            ? upgrade.GetHealthIncrease()
            : 0f;
    }


    void Start()
    {
        healthAmount = startingHealth;
        healthAmount += GetHealthUpgrade();

        if (usePermanentHealthUpgrade)
        {
            Debug.Log($"Player starting HP: {healthAmount}");
        }
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