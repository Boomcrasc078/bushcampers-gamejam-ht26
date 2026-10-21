
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDistance = 6f;
    [SerializeField] private float maxSpawnDistance = 15f;
    [SerializeField] private float spawnPadding = 0.2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Camera Spawning")]
    [SerializeField] private Camera spawnCamera;
    [SerializeField, Range(0f, 0.3f)]
    private float offscreenPadding = 0.5f;

    private Transform player;
    private WorldGeneration world;
    private GridNavigation navigation;

    public void Initialize(
        Transform player,
        WorldGeneration world,
        GridNavigation navigation)
    {
        this.player = player;
        this.world = world;
        this.navigation = navigation;
    }

    public int Spawn(EnemyDefinition enemyType, int count)
    {
        if (count <= 0 || enemyType == null || enemyType.prefab == null || player == null || world == null || navigation == null)
            return 0;

        if (minSpawnDistance >= maxSpawnDistance)
        {
            Debug.LogError("Invalid spawn distances.");
            return 0;
        }

        CircleCollider2D bodyCollider = enemyType.prefab.GetComponent<CircleCollider2D>();

        if (bodyCollider == null)
        {
            Debug.LogError("Enemy prefab no col");
            return 0;
        }

        Vector3 scale = bodyCollider.transform.lossyScale;

        float radius = bodyCollider.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));

        List<Vector2> spawnedPositions = new List<Vector2>();

        int spawned = 0;
        int maxAttempts = count * 50;

        for (int attempt = 0; attempt < maxAttempts && spawned < count; attempt++)
        {

            // choose random pos around player - > will change later to off screen?
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

            Vector2 direction = new Vector2(Mathf.Cos(angle),Mathf.Sin(angle));

            Vector2 candidatePosition = (Vector2)player.position + direction * distance;

            //convert to cell
            Vector2Int cell = world.WorldToCell(candidatePosition);

            if (!navigation.IsWalkable(cell) || navigation.GetDistance(cell) < 0)
                continue;

            Vector3 spawnPosition = world.CellToWorld(cell);
            Vector2 spawnPosition2D = spawnPosition;

            //pref offscreen spawn
            bool allowOnScreenFallBack = attempt >= maxAttempts * 0.8f;

            if (!allowOnScreenFallBack && IsInsideCamera(spawnPosition))
                continue;

            float distanceToPlayer = Vector2.Distance(spawnPosition2D,player.position);

            if (distanceToPlayer < minSpawnDistance || distanceToPlayer > maxSpawnDistance)
                continue;

            // dont spawn inside others
            if (Physics2D.OverlapCircle(spawnPosition2D,radius + spawnPadding,enemyLayer) != null)
                continue;

            bool tooClose = false;

            foreach (Vector2 previous in spawnedPositions)
            {
                if (Vector2.Distance(previous, spawnPosition2D)
                    < radius * 2f + spawnPadding)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            // spawn the enemy
            GameObject enemy = Instantiate(enemyType.prefab, spawnPosition, Quaternion.identity);

            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

            if (movement == null)
            {
                Debug.LogError("missing script on prefab");
                Destroy(enemy);
                break;
            }

            movement.Initialize(player, world, navigation, enemyType);

            Health health = enemy.GetComponentInChildren<Health>();

            if(health != null)
            {
                health.Initialize(enemyType.maxHealth);
            }
            else
            {
                Debug.LogWarning($"Enemy {enemyType.enemyName} has no health component");
            }

            EnemyMeleeAttack meleeattack = enemy.GetComponent<EnemyMeleeAttack>();

            if(meleeattack != null)
            {
                meleeattack.Initialize(player, enemyType.damage);
            }


            spawnedPositions.Add(spawnPosition2D);
            spawned++;
        }

        return spawned;
    }

    private bool IsInsideCamera(Vector3 position)
    {
        Vector3 viewport = spawnCamera.WorldToViewportPoint(position);

        return viewport.z > 0f && viewport.x >= -offscreenPadding && viewport.x <= 1f + offscreenPadding && viewport.y >= -offscreenPadding && viewport.y <= 1f + offscreenPadding;
    }
}
