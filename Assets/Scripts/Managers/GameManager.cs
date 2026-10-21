using System;
using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldGeneration worldGeneration;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private RunManager runManager;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private EnemyDefinition basicEnemy;

    private Transform playerTransform;
    private GridNavigation navigation;
    private Vector2Int lastPlayerCell;



    private void Start()
    {
        //gen world
        worldGeneration.GenerateWorld();

        if (worldGeneration.CurrentWorld == null)
        {
            Debug.LogError("world gen failed");
            return;
        }

        SpawnPlayer();

        InitializeNavigation();

        if (navigation == null)
        {
            Debug.LogError("nav init failed");
            return;
        }

        // start enemy spawn
        enemySpawner.Initialize(
            playerTransform,
            worldGeneration,
            navigation
        );

        // start game loop
        runManager.Initialize(enemySpawner);
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

        Debug.Log($"Player cell: {currentCell}, " +
          $"Walkable for enemy: {navigation.IsWalkable(currentCell)}");

        navigation.BuildDistanceField(currentCell);
    }


    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, worldGeneration.SpawnWorldPosition, Quaternion.identity);

        playerTransform = player.transform;
        cameraMovement.SetTarget(playerTransform);
    }

   
    private void InitializeNavigation()
    {
        CircleCollider2D enemyCollider = basicEnemy.prefab.GetComponent<CircleCollider2D>();

        if(enemyCollider == null)
        {
            Debug.LogError("no collider on prefab");
            return;
        }

        Vector3 scale = enemyCollider.transform.lossyScale;

        float radius = enemyCollider.radius * Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y));

        navigation = new GridNavigation(worldGeneration, radius, wallLayer);

        Vector2Int playerCell = worldGeneration.WorldToCell(playerTransform.position);

        lastPlayerCell = playerCell;

        navigation.BuildDistanceField(playerCell);
    }
}
