
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
    private Vector2Int? currentWaypoint;

    private readonly RaycastHit2D[] wallHits = new RaycastHit2D[8];
    private ContactFilter2D wallFilter;

    private CircleCollider2D bodyCollider;


    private void Awake()
    {
        bodyCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        wallFilter = new ContactFilter2D();
        wallFilter.SetLayerMask(wallLayer);
        wallFilter.useTriggers = false;
    }

    public void Initialize(Transform target,WorldGeneration world,GridNavigation navigation)
    {
        this.target = target;
        this.world = world;
        this.navigation = navigation;
    }

    public float CollisionsRadius
    {
        get
        {
            Vector3 scale = bodyCollider.transform.lossyScale;

            return bodyCollider.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
        }
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

    //1 check if enemy can reach player without pathing
    Vector2 toPlayer = targetPosition - currentPosition;

    bool wallBetween =
        toPlayer.sqrMagnitude > 0.0001f &&
        rb.Cast(
            toPlayer.normalized,
            wallFilter,
            wallHits,
            toPlayer.magnitude
        ) > 0;

    Debug.DrawLine(
        currentPosition,
        targetPosition,
        wallBetween ? Color.red : Color.green
    );


    //2 check if current waypoint is finished
    if (currentWaypoint != null &&
        Vector2.Distance(
            currentPosition,
            world.CellToWorld(currentWaypoint.Value)
        ) < 0.1f)
    {
        currentWaypoint = null;
    }

    Vector2 destination = targetPosition;


    //3 chase only when no wall or waypoint is in the way!
    if (!wallBetween && currentWaypoint == null)
    {
        if (Vector2.Distance(currentPosition, targetPosition) <= stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
    else
    {
        //4 follow current or select new
        if (currentWaypoint == null)
        {
            Vector2Int currentCell = world.WorldToCell(transform.position);
            Vector2Int nextCell = navigation.GetNextCell(currentCell);

            if (nextCell == currentCell)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            currentWaypoint = nextCell;
        }

        destination = world.CellToWorld(currentWaypoint.Value);
    }

    //5 check if the destination is blocked - >> Debugging 
    Vector2 toWaypoint = destination - currentPosition;

    bool waypointBlocked =
        toWaypoint.sqrMagnitude > 0.0001f &&
        rb.Cast(
            toWaypoint.normalized,
            wallFilter,
            wallHits,
            toWaypoint.magnitude
        ) > 0;

    Debug.DrawLine(
        currentPosition,
        destination,
        waypointBlocked ? Color.magenta : Color.cyan
    );


    // 6 move to destination
    Vector2 direction = destination - currentPosition;
    rb.linearVelocity = direction.normalized * moveSpeed;
}

}
