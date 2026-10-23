
using System.Collections;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer warningSprite;
    [SerializeField] private Color warningColor = Color.black;
    [SerializeField] private ParticleSystem attackParticles;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;
    [Header("Animation")]
    [SerializeField] private Animator animator;

    private Transform target;
    private Health targetHealth;
    private PlayerMovement playerMovement;

    private float damage;
    private float attackRange;
    private float windupDuration;
    private float recoveryDuration;
    private float attackCooldown;
    private float knockbackStrength;

    private float nextAttackTime;
    private bool isAttacking;
    private Color originalColor;

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
        playerMovement = player.GetComponent<PlayerMovement>();

        damage = definition.damage;
        attackRange = definition.attackRange;

        windupDuration = definition.attackWindup;
        recoveryDuration = definition.attackRecovery;
        attackCooldown = definition.attackCooldown;
        knockbackStrength = definition.knockbackStrength;
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

        Vector2 direction = target.position - transform.position;

        if (direction.sqrMagnitude > attackRange * attackRange)
            return false;

        return Physics2D.Linecast(
            transform.position,
            target.position,
            wallLayer
        ).collider == null;
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;

        // WIND-UP
        if (warningSprite != null)
            warningSprite.color = warningColor;

        yield return new WaitForSeconds(windupDuration);

        if (warningSprite != null)
            warningSprite.color = originalColor;

        // ATTACK
        PlayAttackParticles();

        if (IsPlayerInRange())
        {
            targetHealth.TakeDamage(damage);

            if (knockbackStrength > 0f && playerMovement != null)
            {
                Vector2 direction =
                    target.position - transform.position;

                if (direction.sqrMagnitude < 0.001f)
                    direction = Vector2.up;

                playerMovement.ApplyKnockback(
                    direction.normalized * knockbackStrength
                );
            }

            targetHealth.GetComponent<IHitReaction>().ReactToHit(new Vector2());
        }

        // RECOVERY
        yield return new WaitForSeconds(recoveryDuration);

        isAttacking = false;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetTrigger("Attack");
    }

    private void PlayAttackParticles()
    {
        if (attackParticles == null)
            return;

        var shape = attackParticles.shape;
        shape.radius = attackRange;

        attackParticles.Play();
    }
}
