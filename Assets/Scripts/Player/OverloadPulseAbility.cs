
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverloadPulseAbility : MonoBehaviour
{
    [Header("Ability")]
    [SerializeField] private float radius = 5f;
    [SerializeField] private float damage = 9999f;
    [SerializeField] private float invulnerabilityDuration = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Charges")]
    [SerializeField] private int maxCharges = 1;

    private int currentCharges = 0;
    private Health playerHealth;

    public int CurrentCharges => currentCharges;

    private void Awake()
    {
        playerHealth = GetComponentInChildren<Health>();
    }

    private void Update()
    {
        //temp can move later
        if (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame)
        {
            Activate();
        }
    }

    public bool TryCollect()
    {
        if (currentCharges >= maxCharges)
            return false;

        currentCharges++;
        Debug.Log($"overload pulse collected  -> charges: {currentCharges}");

        return true;
    }

    public void Activate()
    {
        if (currentCharges <= 0 || playerHealth == null)
            return;

        currentCharges--;


        playerHealth.GrantInvulnerability(invulnerabilityDuration);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        HashSet<Health> damagedEnemies = new HashSet<Health>();

        foreach (Collider2D hit in hits)
        {
            Health enemyHealth = hit.GetComponentInParent<Health>();

            if (enemyHealth == null || enemyHealth == playerHealth || !damagedEnemies.Add(enemyHealth))
                continue;

            enemyHealth.TakeDamage(damage);
        }

        Debug.Log($" pulse activated, hit {damagedEnemies.Count} enemies.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
