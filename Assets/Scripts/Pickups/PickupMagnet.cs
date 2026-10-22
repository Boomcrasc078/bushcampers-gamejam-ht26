
using UnityEngine;

public class PickupMagnet : MonoBehaviour
{
    [Header("Attraction")]
    [SerializeField] private float pullDistance = 3f;
    [SerializeField] private float pullSpeed = 3f;

    private Transform playerTransform;
    private Rigidbody2D rb;
    private PickupRadiusUpgrade radiusUpgrade;

    public Transform PlayerTransform => playerTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (ShopUpgradeManager.instance != null)
        {
            radiusUpgrade = ShopUpgradeManager.instance.GetUpgrade<PickupRadiusUpgrade>();
        }
    }

    private void Update()
    {
        // The player may be spawned after the pickup.
        if (playerTransform == null)
        {
            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();

            if (player != null)
                playerTransform = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
            return;

        float multiplier = radiusUpgrade != null ? radiusUpgrade.GetRadiusMultiplier() : 1f;
        float effectivePullDistance = pullDistance * multiplier;

        Vector2 currentPosition = rb != null ? rb.position : (Vector2)transform.position;
        Vector2 targetPosition = playerTransform.position;

        if (Vector2.Distance(currentPosition, targetPosition) >= effectivePullDistance)
            return;

        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, targetPosition, pullSpeed * Time.fixedDeltaTime);

        if (rb != null)
            rb.MovePosition(nextPosition);
        else
            transform.position = new Vector3(nextPosition.x, nextPosition.y, transform.position.z);
    }
}
