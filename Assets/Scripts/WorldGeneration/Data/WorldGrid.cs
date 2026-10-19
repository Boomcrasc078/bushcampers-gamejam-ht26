using UnityEngine;

public class WorldGrid
{
    private CellType[,] cells;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public WorldGrid(int width, int height)
    {
        Width = width;
        Height = height;

        cells = new CellType[width, height];
    }

    public CellType GetCell(int x, int y)
    {
        if (IsInBounds(x, y))
        {
            return cells[x, y];
        }
        else
        {
            throw new System.ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds.");
        }
    }

    public void SetCell(int x, int y, CellType type)
    {
        if (IsInBounds(x, y))
        {
            cells[x, y] = type;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are out of bounds.");
        }
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }
}
