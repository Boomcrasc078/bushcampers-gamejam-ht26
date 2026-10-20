using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldGeneration worldGeneration;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CameraMovement cameraMovement;

    private void Start()
    {
        worldGeneration.GenerateWorld();

        if (worldGeneration.CurrentWorld == null)
        {
            Debug.LogError("World Generation failed. Cannot spawn player");
            return;
        }

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3 spawnPosition = worldGeneration.SpawnWorldPosition;

        GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

        cameraMovement.SetTarget(player.transform);
    }
}
