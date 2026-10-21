
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IShooting
{
    [Header("Attack")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform aimPivot;

    private Transform target;
    private float attackRange;

    public bool ShouldStopMoving => IsShooting();

    public void Initialize(Transform player, EnemyDefinition definition)
    {
        target = player;
        attackRange = definition.attackRange;
    }

    public bool IsShooting()
    {
        if (target == null)
            return false;

        Vector2 start = transform.position;
        Vector2 end = target.position;

        if ((end - start).sqrMagnitude > attackRange * attackRange)
            return false;

        // walls dont shoot :)
        return Physics2D.Linecast(
            start,
            end,
            wallLayer
        ).collider == null;
    }

    private void Update()
    {
        if (target == null || aimPivot == null)
            return;

        Vector2 direction =
            target.position - aimPivot.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        float angle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        aimPivot.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
