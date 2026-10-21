
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private PlayerInput playerInput;
    private Rigidbody2D rigidbody2D;
    private SpeedUpgrade speedUpgrade;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        speedUpgrade = GetComponent<SpeedUpgrade>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 direction =
            Vector2.ClampMagnitude(playerInput.GetMoveInput(), 1f);

        float speed = moveSpeed * GetSpeedUpgrade();

        rigidbody2D.linearVelocity = direction * speed;
    }

    private float GetSpeedUpgrade()
    {
        if (speedUpgrade != null &&
            ShopUpgradeManager.instance != null &&
            ShopUpgradeManager.instance.ActivatedUpgrades.Contains(speedUpgrade))
        {
            return speedUpgrade.GetUpgradeMultiplier();
        }

        return 1f;
    }
}
