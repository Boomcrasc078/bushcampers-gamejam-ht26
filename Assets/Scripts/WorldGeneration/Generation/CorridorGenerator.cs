using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator
{

    // Connects rooms in the world grid by carving corridors between them.
    public void ConnectRooms(WorldGrid world, List<RectInt> rooms, int corridorWidth)
    {
        // Validate inputs
        if(corridorWidth < 1)
        {
            Debug.LogWarning("Corridor width must be at least 1.");
            return;
        }
        if (rooms.Count < 2)
        {
            Debug.LogWarning("Not enough rooms to connect.");
            return;
        }


        // Use a simple greedy algorithm to connect rooms based on their center points.
        HashSet<int> connected = new HashSet<int> { 0 };

        while (connected.Count < rooms.Count)
        {
            int shortestDistance = int.MaxValue;

            int fromIndex = -1;
            int toIndex = -1;

            foreach (int connectedIndex in connected)
            {
                for (int i = 0; i < rooms.Count; i++)
                {
                    if (connected.Contains(i))
                        continue;

                    Vector2Int from = GetRoomCenter(rooms[connectedIndex]);
                    Vector2Int to = GetRoomCenter(rooms[i]);

                    int distance = (from - to).sqrMagnitude;

                    if(distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        fromIndex = connectedIndex;
                        toIndex = i;
                    }
                }
            }
            if (toIndex == -1)
            {
                break; // No more rooms to connect
            }

            CarveCorridor(world, GetRoomCenter(rooms[fromIndex]), GetRoomCenter(rooms[toIndex]), corridorWidth); // Carve the corridor between the two rooms

            connected.Add(toIndex);
        }
    }


    // Carves a corridor between two points in the world grid.
    private void CarveCorridor(WorldGrid world, Vector2Int start, Vector2Int end, int corridorWidth)
    {
        //horizontal

        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);

        for(int x = minX; x <= maxX; x++)
        {
            for (int offset = -corridorWidth / 2; offset < corridorWidth - corridorWidth / 2; offset++)
            {
                int y = start.y + offset;
                if (world.IsInBounds(x, y))
                {
                    world.SetCell(x, y, CellType.Floor);
                }
            }
        }

        //vertical

        int minY = Mathf.Min(start.y, end.y);
        int maxY = Mathf.Max(start.y, end.y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int offset = -corridorWidth / 2; offset < corridorWidth - corridorWidth / 2; offset++)
            {
                int x = end.x + offset;
                if (world.IsInBounds(x, y))
                {
                    world.SetCell(x, y, CellType.Floor);
                }
            }
        }
    }

    private Vector2Int GetRoomCenter(RectInt room)
    {
        return new Vector2Int(room.xMin + room.width / 2, room.yMin + room.height / 2); // Calculate the center of the room
    }
}
