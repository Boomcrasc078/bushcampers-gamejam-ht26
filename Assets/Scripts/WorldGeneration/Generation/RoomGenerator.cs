using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator
{
    public List<RectInt> GenerateRooms(int mapWidth, int mapHeight, int roomCount, int minRoomSize, int maxRoomSize, int seed)
    {
        System.Random random = new System.Random(seed);

        List<RectInt> rooms = new List<RectInt>();

        int maxAttempts = roomCount * 20; // Limit the number of attempts to place rooms

        for(int attempt = 0; attempt < maxAttempts; attempt++)
        {
            if (rooms.Count >= roomCount)
                break;
            int roomWidth = random.Next(minRoomSize, maxRoomSize + 1);
            int roomHeight = random.Next(minRoomSize, maxRoomSize + 1);

            if(roomWidth >= mapWidth -2 || roomHeight >= mapHeight - 2)
                continue;   

            int x = random.Next(1, mapWidth - roomWidth);
            int y = random.Next(1, mapHeight - roomHeight);

            RectInt newRoom = new RectInt(x, y, roomWidth, roomHeight);

            bool overlaps = false;
            foreach (RectInt room in rooms)
            {
                RectInt expanded = new RectInt(room.xMin-2, room.yMin - 2, room.width + 4, room.height + 4);
                if (newRoom.Overlaps(expanded))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
            }
        }

        return rooms;
    }
}
