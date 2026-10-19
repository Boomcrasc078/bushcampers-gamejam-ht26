
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerationTest : MonoBehaviour
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

    private void Start()
    {
        // 1. Create the world grid (initially all walls)
        WorldGrid world = new WorldGrid(width, height);

        // 2. Generate rooms
        RoomGenerator roomGenerator = new RoomGenerator();

        List<RectInt> rooms = roomGenerator.GenerateRooms(
            width,
            height,
            roomCount,
            minRoomSize,
            maxRoomSize,
            seed
        );

        // 3. Carve rooms into the world
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

        // 4. Connect rooms with corridors
        CorridorGenerator corridorGenerator = new CorridorGenerator();

        corridorGenerator.ConnectRooms(
            world,
            rooms,
            corridorWidth
        );

        // 5. Render the finished world
        worldRenderer.Render(world);

        Debug.Log($"Generated {rooms.Count} rooms with seed {seed}.");
    }
}
