using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldGeneration worldGeneration;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private CameraMovement cameraMovement;

    private Transform playerTransform;
    private GridNavigation navigation;
    private Vector2Int lastPlayerCell;

    private void Start()
    {
        worldGeneration.GenerateWorld();

        if (worldGeneration.CurrentWorld == null)
        {
            Debug.LogError("World generation failed!");
            return;
        }

        SpawnPlayer();
        InitializeNavigation();
        SpawnTestEnemy();
    }

    private void Update()
    {
        if (playerTransform == null || navigation == null)
            return;

        Vector2Int currentCell = worldGeneration.WorldToCell(
            playerTransform.position
        );

        if (currentCell == lastPlayerCell)
            return;

        lastPlayerCell = currentCell;
        navigation.BuildDistanceField(currentCell);
    }


    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, worldGeneration.SpawnWorldPosition, Quaternion.identity);

        playerTransform = player.transform;
        cameraMovement.SetTarget(playerTransform);
    }

    private void SpawnTestEnemy()
    {
        if (worldGeneration.RewardSpawnCells.Count == 0)
        {
            Debug.LogWarning("No avaliable test spwan locations");
            return;
        }

        Vector2Int spawnCell = worldGeneration.RewardSpawnCells[0];

        Vector3 spawnPosition = worldGeneration.CellToWorld(spawnCell);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if(movement != null)
        {
            movement.Initialize(playerTransform, worldGeneration, navigation);
        }

    }

    private void InitializeNavigation()
    {
        navigation = new GridNavigation(worldGeneration.CurrentWorld);

        Vector2Int playerCell = worldGeneration.WorldToCell(playerTransform.position);

        navigation.BuildDistanceField(playerCell);

        lastPlayerCell = playerCell;
    }
}
