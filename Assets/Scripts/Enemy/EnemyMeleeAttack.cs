
using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float windupDuration = 0.45f;
    [SerializeField] private float recoveryDuration = 0.6f;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer warningSprite;
    [SerializeField]
    private Color warningColor =
        new Color(1f, 0.4f, 0.4f);

    private Transform target;
    private Health targetHealth;
    private float damage;
    private float attackRange;


    private bool isAttacking;
    private float nextAttackTime;
    private Color originalColor;
    


    // Used by EnemyMovement to decide when to stop.
    public bool ShouldStopMoving =>
        isAttacking || IsPlayerInRange();

    private void Awake()
    {
        if (warningSprite != null)
            originalColor = warningSprite.color;
    }

    public void Initialize(Transform player, EnemyDefinition definition)
    {
        target = player;
        targetHealth = player.GetComponentInChildren<Health>();

        damage = definition.damage;
        attackRange = definition.attackRange;
    }

    private void Update()
    {
        if (isAttacking || Time.time < nextAttackTime)
            return;

        if (IsPlayerInRange())
            StartCoroutine(PerformAttack());
    }

    private bool IsPlayerInRange()
    {
        if (target == null || targetHealth == null)
            return false;

        float distance = Vector2.Distance(
            transform.position,
            target.position
        );

        if (distance > attackRange)
            return false;

        // walls!
        return Physics2D.Linecast(
            transform.position,
            target.position,
            wallLayer
        ).collider == null;
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // show that hes attacking :)
        if (warningSprite != null)
            warningSprite.color = warningColor;

        yield return new WaitForSeconds(windupDuration);

        // normal
        if (warningSprite != null)
            warningSprite.color = originalColor;

        // player can dodge now, was impossible before :))))))
        if (IsPlayerInRange())
            targetHealth.TakeDamage(damage);

        // enemy waits and stands still so you can "escape". 
        yield return new WaitForSeconds(recoveryDuration);

        isAttacking = false;
        nextAttackTime = Time.time + attackCooldown;
    }
}
