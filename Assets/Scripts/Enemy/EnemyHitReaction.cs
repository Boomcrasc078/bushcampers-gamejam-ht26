
using System.Collections;
using UnityEngine;

public class EnemyHitReaction : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 1f;
    [SerializeField] private float knockbackDuration = 0.2f;

    [Header("Hit Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Rigidbody2D rb;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private EnemyMovement enemyMovement;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void ReactToHit(Vector2 direction, float multiplier = 1f)
    {
        Debug.Log($"Enemy: {name} | Rigidbody: {(rb == null ? "MISSING" : rb.bodyType.ToString())} | Knockback Force: {knockbackForce * multiplier}");


        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.linearVelocity = Vector2.zero;

            if (enemyMovement != null)
                enemyMovement.PauseForKnockback(knockbackDuration);

            rb.AddForce(direction.normalized * knockbackForce * multiplier, ForceMode2D.Impulse);
        }



        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(Flash());
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = hitColor;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}
