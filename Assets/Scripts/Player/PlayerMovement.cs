using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
     

    private PlayerInput playerInput;
    private Rigidbody2D rigidbody2D;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }
    
    void Move()
    {
        Vector2 direction = Vector2.ClampMagnitude(playerInput.GetMoveInput(), 1f);

        rigidbody2D.linearVelocity = direction * moveSpeed;
    }
}
