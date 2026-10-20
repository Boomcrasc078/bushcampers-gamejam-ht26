
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorldRenderer worldRenderer;

    [Header("World Settings")]
    [SerializeField] private int width = 96;
    [SerializeField] private int height = 96;
    [SerializeField] private int seed = 12345;

    [Header("Room Settings")]
    [SerializeField] private int roomCount = 18;
    [SerializeField] private int minRoomSize = 10;
    [SerializeField] private int maxRoomSize = 24;

    [Header("Corridor Settings")]
    [SerializeField] private int corridorWidth = 5;
    [Range(0, 1f)]
    [SerializeField] private float extraConnectionChance = 0.35f;

    [Header("Reward Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float normalRoomRewardChance = 0.15f;

    public WorldGrid CurrentWorld { get; private set; }
    public Vector2Int SpawnCell { get; private set; }
    public Vector3 SpawnWorldPosition { get; private set; }
    public List<Vector2Int> RewardSpawnCells { get; private set; }

    private List<RectInt> rooms;


    public void GenerateWorld()
    {
        if(corridorWidth < 1)
        {
            Debug.LogError("Corridor width must be at least 1");
            return;
        }

        // 1. Create the world grid (initially all walls)
        WorldGrid world = new WorldGrid(width, height);

        // 2. Generate rooms
        RoomGenerator roomGenerator = new RoomGenerator();

        rooms = roomGenerator.GenerateRooms(
            width,
            height,
            roomCount,
            minRoomSize,
            maxRoomSize,
            seed
        );

        if (rooms.Count == 0)
        {
            Debug.LogError("No rooms generated! Cannot place player.");
            return;
        }

        // 3. Carve rooms into the world
        CarveRooms(world, rooms);
        
        // 4. Connect rooms with corridors
        CorridorGenerator corridorGenerator = new CorridorGenerator();

        corridorGenerator.ConnectRooms(
            world,
            rooms,
            corridorWidth,
            seed,
            extraConnectionChance
        );

        //5. Find a room closest to the middle that isn't a dead end
        int spawnRoomIndex = FindSpawnRoom(rooms, corridorGenerator.ConnectionCounts);

        RectInt spawnRoom = rooms[spawnRoomIndex];

        Vector2Int spawnCell = new Vector2Int(spawnRoom.xMin + spawnRoom.width / 2, spawnRoom.yMin + spawnRoom.height / 2);

        Debug.Log(
            $"Selected spawn room: {spawnRoomIndex}, " +
            $"Connections: {corridorGenerator.ConnectionCounts[spawnRoomIndex]}"
        );

        //6

        RewardSpawnCells = GenerateRewardsLocations(rooms, corridorGenerator.ConnectionCounts, spawnRoomIndex, seed, normalRoomRewardChance);
        Debug.Log($"Generated {RewardSpawnCells.Count} reward locations.");


        // 6. Render the finished world
        worldRenderer.Render(world);

        CurrentWorld = world;
        SpawnCell = spawnCell;
        SpawnWorldPosition = worldRenderer.GetCellCenterWorld(spawnCell);


    }

    private void CarveRooms(WorldGrid world, List<RectInt> rooms)
    {
        foreach (RectInt room in rooms)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                for (int y = room.yMin; y < room.yMax; y++)
                {
                    world.SetCell(x, y, CellType.Floor);
                }
            }
        }
    }


    private int FindSpawnRoom(List<RectInt> rooms, int[] connectionCounts)
    {
        Vector2 mapCenter = new Vector2(width / 2f, height / 2f);

        int spawnRoomIndex = -1;
        float shortestDistance = float.MaxValue;

        // Find the closest room with at least two connections
        for (int i = 0; i < rooms.Count; i++)
        {
            if (connectionCounts[i] < 2)
                continue;

            float distance = (rooms[i].center - mapCenter).sqrMagnitude;

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                spawnRoomIndex = i;
            }
        }

        // Fallback: choose the closest room regardless of connections
        if (spawnRoomIndex == -1)
        {
            Debug.LogWarning("No room with 2+ connections. Finding closest room instead.");

            for (int i = 0; i < rooms.Count; i++)
            {
                float distance = (rooms[i].center - mapCenter).sqrMagnitude;

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    spawnRoomIndex = i;
                }
            }
        }

        return spawnRoomIndex;
    }

    private List<Vector2Int> GenerateRewardsLocations(List<RectInt> rooms, int[] connectionCounts, int spawnRoomIndex, int seed, float normalRoomRewardChance)
    {
        List<Vector2Int> rewardLocations = new List<Vector2Int>();

        System.Random random = new System.Random(seed + 1000);

        for(int i = 0; i < rooms.Count; i++)
        {
            if (i == spawnRoomIndex)
                continue;

            bool isDeadEnd = connectionCounts[i] == 1;

            if (!isDeadEnd && random.NextDouble() >= normalRoomRewardChance)
                continue;

            RectInt room = rooms[i];

            Vector2Int rewardCell = new Vector2Int(room.xMin + room.width / 2, room.yMin + room.height / 2);

            rewardLocations.Add(rewardCell);
        }
        return rewardLocations;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (rooms == null)
            return;

        for (int i = 0; i < rooms.Count; i++)
        {
            RectInt room = rooms[i];

            Vector3 center = new Vector3(
                room.center.x,
                room.center.y,
                0
            );

            UnityEditor.Handles.Label(center, $"Room {i}");
        }

        if(CurrentWorld != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(SpawnWorldPosition, 0.75f);

            UnityEditor.Handles.Label(SpawnWorldPosition + Vector3.up*3f, "PLAYER SPAWN");
        }

        if (CurrentWorld != null && RewardSpawnCells != null)
        {
            Gizmos.color = Color.yellow;

            foreach (Vector2Int rewardCell in RewardSpawnCells)
            {
                Vector3 position = worldRenderer.GetCellCenterWorld(rewardCell);

                Gizmos.DrawWireSphere(position, 0.5f);

                UnityEditor.Handles.Label(
                    position + Vector3.up,
                    "REWARD"
                );
            }
        }

    }
#endif

}
