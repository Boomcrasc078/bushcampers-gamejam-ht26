using UnityEngine;
using System.Collections.Generic;

public class GridNavigation
{
    private WorldGrid world;
    private int[,] distances;

    private WorldGeneration worldGeneration;
    private float collisionRadius;
    private LayerMask wallLayer;


    //0 -> not checked 1-> allowed -1 -> blocked ?
    private sbyte[,] cellCache;
    private sbyte[,,] stepCache;


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

    public GridNavigation(WorldGeneration worldGeneration, float collisionRadius, LayerMask wallLayer)
    {
        this.worldGeneration = worldGeneration;
        this.world = worldGeneration.CurrentWorld;
        this.collisionRadius = collisionRadius;
        this.wallLayer = wallLayer;

        distances = new int[world.Width, world.Height];

        cellCache = new sbyte[world.Width, world.Height];
        stepCache = new sbyte[world.Width, world.Height, 8];
    
    }

    public bool IsWalkable(Vector2Int cell)
    {
        if (!world.IsInBounds(cell.x, cell.y))
            return false;

        if (world.GetCell(cell.x, cell.y) != CellType.Floor)
            return false;

        //return prev calc result >:)
        if (cellCache[cell.x, cell.y] != 0)
            return cellCache[cell.x, cell.y] == 1;

        Vector2 position = worldGeneration.CellToWorld(cell);

        bool canFit = Physics2D.OverlapCircle(position, collisionRadius, wallLayer) == null;
        cellCache[cell.x, cell.y] = (sbyte)(canFit ? 1 : -1);

        return canFit;
    }

    private bool CanStep(Vector2Int from, Vector2Int direction)
    {
        Vector2Int next = from + direction;

        if (!IsWalkable(from) || !IsWalkable(next))
            return false;

        int directionIndex = System.Array.IndexOf(directions, direction);

        if (directionIndex == -1)
            return false;

        //resue prev if there
        sbyte cached = stepCache[from.x, from.y, directionIndex];

        if (cached != 0)
            return cached == 1;

        bool canMove = true;

        //prevent diagonal
        if(direction.x != 0 && direction.y != 0)
        {
            Vector2Int horizontal = from + new Vector2Int(direction.x, 0);
            Vector2Int vertical = from + new Vector2Int(0, direction.y);

            if (!IsWalkable(horizontal) || !IsWalkable(vertical))
                canMove = false;
        }

        //check if col fits :P
        if (canMove)
        {
            Vector2 start = worldGeneration.CellToWorld(from);
            Vector2 end = worldGeneration.CellToWorld(next);

            Vector2 movement = end - start;

            RaycastHit2D hit = Physics2D.CircleCast(start, collisionRadius, movement.normalized, movement.magnitude, wallLayer);

            canMove = hit.collider == null;
        }
        stepCache[from.x, from.y, directionIndex] = (sbyte)(canMove ? 1 : -1);
        return canMove;

    }

    //? betyder att den kan returna null också :I
    private Vector2Int? FindNearestWalkableTarget(Vector2Int target)
    {
        Vector2 targetPosition = worldGeneration.CellToWorld(target);

        //search outwards
        for(int radius = 1; radius <=6; radius++)
        {
            Vector2Int? bestCell = null;
            float bestDistance = float.MaxValue;

            for(int x = -radius; x <=radius; x++)
            {
                for(int y = -radius; y <= radius; y++)
                {
                    // only check outer
                    if (Mathf.Max(Mathf.Abs(x), Mathf.Abs(y)) != radius)
                        continue;

                    Vector2Int candidate = target + new Vector2Int(x, y);

                    if (!IsWalkable(candidate))
                        continue;

                    Vector2 candidatePosition = worldGeneration.CellToWorld(candidate);

                    //avoid going through wall ... dumb fuck moron i am

                    if (Physics2D.Linecast(targetPosition, candidatePosition, wallLayer).collider != null)
                        continue;

                    float distance = (candidatePosition - targetPosition).sqrMagnitude;

                    if(distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestCell = candidate;
                    }
                }
            }
            if (bestCell.HasValue)
                return bestCell;
        }
        return null;
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
        {
            Vector2Int? replacement = FindNearestWalkableTarget(targetCell);

            if (!replacement.HasValue)
                return;

            targetCell = replacement.Value;
        }

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
