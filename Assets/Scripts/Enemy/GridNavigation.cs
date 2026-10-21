using UnityEngine;
using System.Collections.Generic;

public class GridNavigation
{
    private WorldGrid world;
    private int[,] distances;

    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,

        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
    };

    public GridNavigation(WorldGrid world)
    {
        this.world = world;
        distances = new int[world.Width, world.Height];
    }

    public bool IsWalkable(Vector2Int cell)
    {
        return world.IsInBounds(cell.x, cell.y) && world.GetCell(cell.x, cell.y) == CellType.Floor;
    }

    private bool CanStep(Vector2Int from, Vector2Int direction)
    {
        Vector2Int next = from + direction;

        if (!IsWalkable(next))
            return false;

        //diagonal cannot cut corners!

        if(direction.x != 0 && direction.y != 0)
        {
            Vector2Int horizontal = from + new Vector2Int(direction.x, 0);
            Vector2Int vertical = from + new Vector2Int(0, direction.y);

            if (!IsWalkable(horizontal) || !IsWalkable(vertical))
                return false;
        }

        return true;

    }


    //penis vad lång tid detta tog, fuck algorithmer
    //This whole thing is just dijkstras algorithm,  DU VET HAN FRÅN THE WITCHER! --- >cost based instead of int based.. works with diagonal :)
    public void BuildDistanceField(Vector2Int targetCell)
    {
        // Reset distances.
        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                distances[x, y] = -1;
            }
        }

        if (!IsWalkable(targetCell))
            return;

        // Sorted by cost, then coordinates.
        SortedSet<(int cost, int x, int y)> open = new();

        distances[targetCell.x, targetCell.y] = 0;
        open.Add((0, targetCell.x, targetCell.y));

        while (open.Count > 0)
        {
            var current = open.Min;
            open.Remove(current);

            Vector2Int cell = new Vector2Int(current.x, current.y);

            // Ignore outdated entries.
            if (current.cost != distances[cell.x, cell.y])
                continue;

            foreach (Vector2Int direction in directions)
            {
                if (!CanStep(cell, direction))
                    continue;

                Vector2Int next = cell + direction;

                bool diagonal = direction.x != 0 && direction.y != 0;
                int stepCost = diagonal ? 14 : 10; //diagnoal cost 14 while normal is 10. 14 is root of 2 * 10. ca 1.414 

                int newDistance = current.cost + stepCost;
                int oldDistance = distances[next.x, next.y];

                if (oldDistance == -1 || newDistance < oldDistance)
                {
                    distances[next.x, next.y] = newDistance;
                    open.Add((newDistance, next.x, next.y));
                }
            }
        }
    }


    public int GetDistance(Vector2Int cell)
    {
        if (!world.IsInBounds(cell.x, cell.y))
            return -1;

        return distances[cell.x, cell.y];
    }

    //AHHHH
    //Gets the next cell :) 
    public Vector2Int GetNextCell(Vector2Int currentCell)
    {
        int currentDistance = GetDistance(currentCell);

        // Already at the target
        if (currentDistance <= 0)
            return currentCell;

        foreach (Vector2Int direction in directions)
        {
            //ÄNTLIGEN FUNKAR DENNA SKITHÖG FUCK!!!
            if (!CanStep(currentCell, direction))
                continue;

            Vector2Int neighbor = currentCell + direction;
            int neighborDistance = GetDistance(neighbor);

            bool diagonal = direction.x != 0 && direction.y != 0;
            int stepCost = diagonal ? 14 : 10; //OH YEAH PYTHAGREAS OH YEAH

            // Follow a step that belongs to a shortest path.
            if (neighborDistance >= 0 &&
                neighborDistance + stepCost == currentDistance)
            {
                return neighbor;
            }
        }

        return currentCell;
    }


}
