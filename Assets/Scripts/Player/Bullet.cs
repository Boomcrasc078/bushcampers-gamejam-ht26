using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 1f;
    private int remainingHits = 1;
    private float knockbackMultiplier = 1f;

    private HashSet<Health> hitEnemies = new HashSet<Health>();

    public void Initialize(float damage, int additionalPierces = 0, float knockbackMultiplier = 1f)
    {
        this.damage = damage;
        this.knockbackMultiplier = knockbackMultiplier;
        remainingHits = 1 + Mathf.Max(0, additionalPierces);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger)
            return;

        Health hitHealth = other.GetComponentInParent<Health>();

        if (hitHealth != null)
        {
            // Already hit
            if (!hitEnemies.Add(hitHealth))
                return;

            EnemyHitReaction hitReaction = hitHealth.GetComponent<EnemyHitReaction>();

            if (hitReaction != null && gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
            {
                Vector2 knockbackDirection = transform.up;
                Debug.Log($"Bullet knockback multiplier: {knockbackMultiplier}");
                hitReaction.ReactToHit(knockbackDirection, knockbackMultiplier);
            }

            hitHealth.TakeDamage(damage);
            remainingHits--;


            if (remainingHits <= 0)
                Destroy(gameObject);

            return;
        }

        Destroy(gameObject);
    }

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }
}
