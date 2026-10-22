
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float knockbackRecovery = 25f;

    private Vector2 knockbackVelocity;

    private PlayerInput playerInput;
    private Rigidbody2D rigidbody2D;
    private SpeedUpgrade speedUpgrade;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        speedUpgrade = ShopUpgradeManager.instance != null ? ShopUpgradeManager.instance.GetUpgrade<SpeedUpgrade>() : null;
        Debug.Log($"Speed Level: {speedUpgrade?.Level}, Multiplier: {GetSpeedUpgrade()}");
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
        if (speedUpgrade == null)
            return 1f;

        return speedUpgrade.GetUpgradeMultiplier();
    }



    public void ApplyKnockback(Vector2 velocity)
    {
        knockbackVelocity = Vector2.ClampMagnitude(
            knockbackVelocity + velocity,
            12f
        );
    }

}
