using System.Linq;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 300f;
    private PlayerInput playerInput;
    private Rigidbody2D rigidbody2D;
    private SpeedUpgrade speedUpgrade;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        speedUpgrade = GetComponent<SpeedUpgrade>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float speed = baseSpeed;

        speed *= GetSpeedUpgrade();

        rigidbody2D.AddForce(playerInput.GetMoveInput() * Time.deltaTime * speed);
    }

    float GetSpeedUpgrade()
    {
        if (speedUpgrade != null)
        {
            if (ShopUpgradeManager.instance.ActivatedUpgrades.Contains(speedUpgrade))
            {
                return speedUpgrade.GetUpgradeMultiplier();
            }
        }
        return 1f;
    }
}
