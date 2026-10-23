using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject mainGameObject;
    [SerializeField] private float startingHealth = 10f;
    [SerializeField] private bool usePermanentHealthUpgrade = false;


    [Header("Audio")]
    [SerializeField] private Sound hurtSound;
    [SerializeField] private Sound deathSound;
    [Header("Animation")]
    [SerializeField] private Animator animator;


    private float healthAmount;
    private bool isDead = false;
    private bool isInvulnerable = false;
    private Coroutine invulnerabilityCoroutine;

    public float CurrentHealth => healthAmount;
    public float MaxHealth => startingHealth + GetHealthUpgrade();

    public bool Heal(float amount)
    {
        if (isDead || amount <= 0f || healthAmount >= MaxHealth)
            return false;

        healthAmount = Mathf.Min(healthAmount + amount, MaxHealth);
        return true;
    }


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
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvulnerable)
            return;

        healthAmount -= damage;

        if (healthAmount <= 0f)
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

        if (animator != null)
        {
            animator.SetTrigger("Death");
        }

        Destroy(mainGameObject);
    }

    public void Initialize(float maxHealth)
    {
        isDead = false;
        startingHealth = maxHealth;
        healthAmount = startingHealth + GetHealthUpgrade();
    }


    public void GrantInvulnerability(float duration)
    {
        if (isDead)
            return;

        if (invulnerabilityCoroutine != null)
            StopCoroutine(invulnerabilityCoroutine);

        invulnerabilityCoroutine =
            StartCoroutine(InvulnerabilityRoutine(duration));
    }

    private IEnumerator InvulnerabilityRoutine(float duration)
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(duration);

        isInvulnerable = false;
        invulnerabilityCoroutine = null;
    }


}
