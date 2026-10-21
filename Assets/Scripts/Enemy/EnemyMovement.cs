
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.7f;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private Transform target;
    private WorldGeneration world;
    private GridNavigation navigation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(
        Transform target,
        WorldGeneration world,
        GridNavigation navigation)
    {
        this.target = target;
        this.world = world;
        this.navigation = navigation;
    }

    private void FixedUpdate()
    {
        if (target == null || world == null || navigation == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = target.position;


        Vector2 toPlayer = targetPosition - currentPosition;

        RaycastHit2D wallHit = Physics2D.CircleCast(
            currentPosition,
            0.5f,
            toPlayer.normalized,
            toPlayer.magnitude,
            wallLayer
        );

        bool wallBetween = wallHit.collider != null;

        Debug.DrawLine(
            currentPosition,
            targetPosition,
            wallBetween ? Color.red : Color.green
        );


        Vector2 destination = targetPosition;

        if (!wallBetween)
        {
            if (Vector2.Distance(currentPosition, targetPosition) <= stoppingDistance)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }
        else
        {
            Vector2Int currentCell = world.WorldToCell(transform.position);
            Vector2Int nextCell = navigation.GetNextCell(currentCell);

            if (nextCell == currentCell)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            destination = world.CellToWorld(nextCell);
        }
        Debug.DrawLine(currentPosition, destination, Color.cyan);

        Vector2 direction = destination - currentPosition;

        rb.linearVelocity = direction.normalized * moveSpeed;
    }
}
