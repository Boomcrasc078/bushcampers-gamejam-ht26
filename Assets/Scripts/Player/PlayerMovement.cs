using System.Linq;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 300f;
    private PlayerInput playerInput;
    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float speed = baseSpeed;

        var speedUpgrade = (SpeedUpgrade)ShopUpgradeManager.instance.ActivatedUpgrades.FirstOrDefault(upgrade => upgrade is SpeedUpgrade);

        if (speedUpgrade != null)
        {
            speed *= speedUpgrade.GetUpgradeMultiplier();
        }

        rigidbody2D.AddForce(playerInput.GetMoveInput() * Time.deltaTime * speed);
    }
}
