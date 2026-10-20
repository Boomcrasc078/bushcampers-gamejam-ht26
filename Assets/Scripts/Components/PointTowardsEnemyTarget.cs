using System.Collections;
using UnityEngine;

public class PointTowardsEnemyTarget : MonoBehaviour
{
    [SerializeField] private float ticksPerSecond = 1f;
    private Transform target;

    private void Start()
    {
        StartCoroutine(Ticker());
    }

    private IEnumerator Ticker()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / Mathf.Max(ticksPerSecond, 0.01f));

            var enemyTarget = FindAnyObjectByType<EnemyTarget>();
            target = enemyTarget != null ? enemyTarget.transform : null;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector2 direction = target.position - transform.position;
        if (direction.sqrMagnitude < 0.0001f)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}