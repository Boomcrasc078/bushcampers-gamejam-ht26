using System.Collections.Generic;
using UnityEngine;

public class CorridorGenerator
{

    public int[] ConnectionCounts { get; private set; }

    // Connects rooms in the world grid by carving corridors between them.
    public void ConnectRooms(WorldGrid world, List<RectInt> rooms, int corridorWidth, int seed, float extraConnectionChance)
    {
        ConnectionCounts = new int[rooms.Count];
        // Validate inputs
        if (corridorWidth < 1)
        {
            Debug.LogError("Corridor width must be at least 1.");
            return;
        }
        if (rooms.Count < 2)
        {
            Debug.LogWarning("Not enough rooms to connect.");
            return;
        }


        // Use a simple greedy algorithm to connect rooms based on their center points.
        HashSet<int> connected = new HashSet<int> { 0 };

        bool[,] directConnections = new bool[rooms.Count, rooms.Count];

        System.Random shapeRandom = new System.Random(seed);

        // While there are still unconnected rooms, find the closest unconnected room to any of the connected rooms and carve a corridor between them.
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

                    // Use squared distance to avoid unnecessary square root calculations
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


            Vector2Int start = GetRoomCenter(rooms[fromIndex]);
            Vector2Int end = GetRoomCenter(rooms[toIndex]);

            
            bool useDiagonal = shapeRandom.NextDouble() < 0.40;

            if (useDiagonal)
            {
                CarveDiagonalCorridor(world, start, end, corridorWidth);
            }
            else
            {
                CarveCorridor(world, start,end, corridorWidth, shapeRandom.Next(2) == 0); // Carve the corridor between the two rooms
            }

            directConnections[fromIndex, toIndex] = true;
            directConnections[toIndex, fromIndex] = true;

            //Debug.Log($"Connected room {fromIndex} to room {toIndex} with a corridor." + $"{directConnections[fromIndex, toIndex]}");

            connected.Add(toIndex);
        }
        System.Random random = new System.Random(seed);

        // Add extra connections between rooms based on the extraConnectionChance parameter.
        for (int i = 0; i < rooms.Count; i++)
        {
            if(random.NextDouble() >= extraConnectionChance)
                continue;

            // Find the closest unconnected room to the current room and carve a corridor between them.
            int candidateRoom = FindClosestUnconnectedRoom(rooms, directConnections, i);

            if(candidateRoom != -1)
            {
                Vector2Int start = GetRoomCenter(rooms[i]);
                Vector2Int end = GetRoomCenter(rooms[candidateRoom]);

                bool useDiagonal = shapeRandom.NextDouble() < 0.40;

                if (useDiagonal)
                {
                    CarveDiagonalCorridor(world, start, end, corridorWidth);
                }
                else
                {
                    CarveCorridor(world, start, end, corridorWidth, shapeRandom.Next(2) == 0);
                }



                directConnections[i, candidateRoom] = true;
                directConnections[candidateRoom, i] = true;

                //Debug.Log($"Extra connections candidate {i} -> {candidateRoom}");
            }
        }

        for(int i = 0; i < rooms.Count; i++)
        {
            int connections = 0;

            for (int j = 0; j < rooms.Count; j++)
            {
                if (directConnections[i, j])
                {
                    connections++;
                }
            }
            ConnectionCounts[i] = connections;
            if(connections == 1)
            {
                //Debug.Log($"Dead-end-candidate: Room {i}");
            }
        }
       

    }


    // Carves a corridor between two points in the world grid.
    private void CarveCorridor(WorldGrid world, Vector2Int start, Vector2Int end, int corridorWidth, bool horizontalFirst)
    {
        //horizontal

        int minX = Mathf.Min(start.x, end.x);
        int maxX = Mathf.Max(start.x, end.x);

        int horizontalY = horizontalFirst ? start.y : end.y;
        int verticalX = horizontalFirst ? end.x : start.x;

        for(int x = minX; x <= maxX; x++)
        {
            for (int offset = -corridorWidth / 2; offset < corridorWidth - corridorWidth / 2; offset++)
            {
                int y = horizontalY + offset;
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
                int x = verticalX + offset;
                if (world.IsInBounds(x, y))
                {
                    world.SetCell(x, y, CellType.Floor);
                }
            }
        }
    }

    
    private void CarveDiagonalCorridor(WorldGrid world, Vector2Int start, Vector2Int end, int corridorWidth)
    {
        int deltaX = Mathf.Abs(end.x - start.x);
        int deltaY = Mathf.Abs(end.y - start.y);    

        int steps = Mathf.Max(deltaX, deltaY);

        for (int i = 0; i<= steps; i++)
        {
            float t = steps == 0 ? 0f : (float)i / steps;

            int x = Mathf.RoundToInt(Mathf.Lerp(start.x, end.x, t));

            int y = Mathf.RoundToInt(Mathf.Lerp(start.y, end.y, t));

            for(int offsetX = -corridorWidth / 2; offsetX < corridorWidth - corridorWidth / 2; offsetX++)
            {
                for(int offsetY = -corridorWidth / 2; offsetY < corridorWidth - corridorWidth / 2; offsetY++)
                {
                    int tileX = x + offsetX;
                    int tileY = y + offsetY;

                    if(world.IsInBounds(tileX, tileY))
                    {
                        world.SetCell(tileX, tileY, CellType.Floor);
                    }
                }
            }
        }
    }

    private int FindClosestUnconnectedRoom(List<RectInt> rooms, bool[,] directConnections, int roomIndex)
    {
        int closestRoom = -1;
        int shortestDistance = int.MaxValue;

        for (int i = 0; i < rooms.Count; i++)
        {
            if(i == roomIndex || directConnections[roomIndex, i])
                continue;

            Vector2Int from = GetRoomCenter(rooms[roomIndex]);
            Vector2Int to = GetRoomCenter(rooms[i]);

            int distance = (from - to).sqrMagnitude; // Use squared distance for efficiency

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestRoom = i;
            }
        }
        return closestRoom;
    }

    private Vector2Int GetRoomCenter(RectInt room)
    {
        return new Vector2Int(room.xMin + room.width / 2, room.yMin + room.height / 2); // Calculate the center of the room
    }
}
