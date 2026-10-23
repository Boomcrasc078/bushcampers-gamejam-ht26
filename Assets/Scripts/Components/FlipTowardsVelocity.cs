using UnityEngine;

public class FlipTowardsVelocity : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogError($"No SpriteRenderer found for {nameof(FlipTowardsVelocity)} on {name}.", this);
    }

    private void Update()
    {
        if (spriteRenderer == null)
            return;

        float horizontalVelocity = rigidbody2D.linearVelocity.x;
        if (Mathf.Abs(horizontalVelocity) > 0.01f)
            spriteRenderer.flipX = horizontalVelocity < 0f;
    }
}
