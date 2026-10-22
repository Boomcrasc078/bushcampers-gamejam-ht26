using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;


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

    [Header("UI")]
    [SerializeField] private GameObject runSummaryPanel;
    [SerializeField] private GameObject shopPanel;

    [Header("Run Statistics UI")]
    [SerializeField] private TMP_Text survivalTimeText;
    [SerializeField] private TMP_Text enemiesKilledText;
    [SerializeField] private TMP_Text trainingDataEarnedText;

    private Transform playerTransform;
    private GridNavigation navigation;
    private Vector2Int lastPlayerCell;
    private bool runEnded = false;
    private float trainingDataAtRunStart;



    private void Start()
    {
        //load current upgrades etc
        SaveManager.LoadCurrentProgression();

        trainingDataAtRunStart = TrainingDataManager.instance.TrainingDataUnformatted;

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
        if (runEnded || playerTransform == null || navigation == null)
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

        PlayerDeathHandler deathHandler = player.GetComponentInChildren<PlayerDeathHandler>();

        if(deathHandler != null)
        {
            deathHandler.Initialize(this);
        }
        else
        {
            Debug.LogError("player prefab is missing on handler");
        }
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

    public void EndRun()
    {
        if (runEnded)
            return;

        runEnded = true;

        //stop waves
        runManager.enabled = false;

        //save currency
        SaveManager.SaveCurrentProgression();

        //freeze game
        Time.timeScale = 0f;

        UpdateRunSummary();
        runSummaryPanel.SetActive(true);
        shopPanel.SetActive(false);

        Debug.Log("RUN ENDED : progress saved");

    }

    public void ShowShop()
    {
        runSummaryPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void ShowRunSummary()
    {
        runSummaryPanel.SetActive(true);
        shopPanel.SetActive(false);
    }


    private void UpdateRunSummary()
    {
        // survival time
        int totalSeconds = Mathf.FloorToInt(runManager.ElapsedTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        survivalTimeText.text = $"{minutes:00}:{seconds:00}";

        // kills
        enemiesKilledText.text = runManager.EnemiesKilled.ToString();

        // collected training data
        float collected = Mathf.Max(
            0f,
            TrainingDataManager.instance.TrainingDataUnformatted
            - trainingDataAtRunStart
        );

        trainingDataEarnedText.text =
            TrainingDataDisplay.FormatTrainingData(collected);
    }

    public void RestartRun()
    {
        //save
        SaveManager.SaveCurrentProgression();

        //resume time freeze
        Time.timeScale = 1f;

        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
