
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 0.7f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Swarm movement")]
    [SerializeField] private LayerMask enemylayer;
    [SerializeField] private float seperationStrength = 0.8f;
    [SerializeField] private float seperationPadding = 0.4f;

    [Header("Knockback")]
    private float knockbackEndTime;



    private Rigidbody2D rb;
    private Transform target;
    private WorldGeneration world;
    private GridNavigation navigation;
    private Vector2Int? currentWaypoint;

    private readonly RaycastHit2D[] wallHits = new RaycastHit2D[8];
    private ContactFilter2D wallFilter;

    private readonly Collider2D[] nearbyEnemies = new Collider2D[32];
    private ContactFilter2D enemyFilter;

    private CircleCollider2D bodyCollider;

    private EnemyMeleeAttack meleeAttack;
    private EnemyRangedAttack rangedAttack;




    private void Awake()
    {
        bodyCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        wallFilter = new ContactFilter2D();
        wallFilter.SetLayerMask(wallLayer);
        wallFilter.useTriggers = false;

        enemyFilter = new ContactFilter2D();
        enemyFilter.SetLayerMask(enemylayer);
        enemyFilter.useTriggers = false;

        meleeAttack = GetComponent<EnemyMeleeAttack>();
        rangedAttack = GetComponent<EnemyRangedAttack>();

    }

    public void Initialize(Transform target,WorldGeneration world,GridNavigation navigation, EnemyDefinition definition)
    {
        this.target = target;
        this.world = world;
        this.navigation = navigation;

        moveSpeed = definition.moveSpeed;
    }

    public float CollisionsRadius
    {
        get
        {
            Vector3 scale = bodyCollider.transform.lossyScale;

            return bodyCollider.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
        }
    }
    

    // YO THIS SHIT GETTING MESSY FUCKING HELL AHAHHHHHHHHHHH 
    private void FixedUpdate()
    {
        if (Time.time < knockbackEndTime)
            return;

        if (target == null || world == null || navigation == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        bool stopForMelee =
            meleeAttack != null && meleeAttack.ShouldStopMoving;

        bool stopForRanged =
            rangedAttack != null && rangedAttack.ShouldStopMoving;

        if (stopForMelee || stopForRanged)
        {
            currentWaypoint = null;
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
            ) < 0.3f)
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
                    // Try to recover if we're outside the valid navigation field.
                    Vector2Int? recovery = navigation.GetDistance(currentCell) < 0
                        ? FindRecoveryCell(currentPosition)
                        : null;

                    if (!recovery.HasValue)
                    {
                        rb.linearVelocity = Vector2.zero;
                        return;
                    }

                    nextCell = recovery.Value;
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


        // 6 calc direction to destination
        Vector2 desiredDirection =
            (destination - currentPosition).normalized;

        // seperation from nearby
        Vector2 seperation = CalculateSeperation(
            currentPosition,
            desiredDirection
        );

        //Combine 

        Vector2 sidewaysSeparation = seperation -
            desiredDirection * Vector2.Dot(seperation, desiredDirection);

        Vector2 combinedDirection = desiredDirection + sidewaysSeparation;


        Vector2 finalDirection =
            combinedDirection.sqrMagnitude > 0.0001f
                ? combinedDirection.normalized
                : desiredDirection;

        // check so doesnt go into wall
        float checkDistance = moveSpeed * Time.fixedDeltaTime + 0.05f;

        bool steeringBlocked =
            rb.Cast(
                finalDirection,
                wallFilter,
                wallHits,
                checkDistance
            ) > 0;

        if (steeringBlocked)
        {
            finalDirection = desiredDirection;
        }

        // 7 apply
        rb.linearVelocity = finalDirection * moveSpeed;

    }

    private Vector2Int? FindRecoveryCell(Vector2 position)
    {
        Vector2Int origin = world.WorldToCell(position);

        Vector2Int? bestCell = null;
        float bestDistance = float.MaxValue;

        for(int x = -3; x <= 3; x++)
        {
            for (int y = -3; y <= 3; y++)
            {
                Vector2Int candidate = origin + new Vector2Int(x, y);

                if (!navigation.IsWalkable(candidate) || navigation.GetDistance(candidate) < 0)
                    continue;

                Vector2 destination = world.CellToWorld(candidate);
                Vector2 direction = destination - position;

                float distance = direction.sqrMagnitude;

                if (distance >= bestDistance)
                    continue;

                //check with col
                if (distance > 0.001f && rb.Cast(direction.normalized, wallFilter, wallHits, direction.magnitude) > 0)
                    continue;

                bestCell = candidate;
                bestDistance = distance;

            }
        }
        return bestCell;
    }

    private Vector2 CalculateSeperation(Vector2 position, Vector2 desiredDirection)
    {
        float ownRadius = CollisionsRadius;
        float searchRadius = ownRadius * 3f + seperationPadding;

        int count = Physics2D.OverlapCircle(position, searchRadius, enemyFilter, nearbyEnemies);

        Vector2 separation = Vector2.zero;

        for(int i = 0; i < count; i++)
        {
            Collider2D other = nearbyEnemies[i];

            //ignore self
            if (other == null || other.attachedRigidbody == rb)
                continue;

            Vector2 away = position - (Vector2)other.bounds.center;
            float distance = away.magnitude;

            if (distance < 0.001f)
                continue;

            float otherRadius = Mathf.Max(other.bounds.extents.x, other.bounds.extents.y);

            float desiredDistance = ownRadius + otherRadius + seperationPadding;

            if (distance >= desiredDistance)
                continue;

            float strength = 1f - distance / desiredDistance;

            //push away 
            separation += away.normalized * strength;

        }

        return Vector2.ClampMagnitude(separation, 1f) * seperationStrength;
    }

    public void PauseForKnockback(float duration)
    {
        knockbackEndTime = Mathf.Max(knockbackEndTime, Time.time + duration);
    }

}
