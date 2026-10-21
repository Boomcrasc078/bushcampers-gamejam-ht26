using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldRenderer : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap floorTilemap;

    [Header("Tiles")]
    [SerializeField] private Tile wallTile;
    [SerializeField] private Tile floorTile;
    


    public void Render(WorldGrid world)
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();

        for (int x = 0; x < world.Width; x++)
        {
            for (int y = 0; y < world.Height; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                CellType cell = world.GetCell(x, y);

                switch (cell)
                {
                    case CellType.Floor:
                        floorTilemap.SetTile(position, floorTile);
                        break;

                    case CellType.Wall:
                        wallTilemap.SetTile(position, wallTile);
                        break;
                }
            }
        }

        TilemapCollider2D wallCollider = wallTilemap.GetComponent<TilemapCollider2D>();

        if(wallCollider != null)
        {
            wallCollider.ProcessTilemapChanges(); // process it instantly instead of lateupdate. FUCK UNITY TILES AHH
        }
    }
    public Vector3 GetCellCenterWorld(Vector2Int cell)
    {
        Vector3Int cellPosition = new Vector3Int(cell.x, cell.y, 0);
        return floorTilemap.GetCellCenterWorld(cellPosition);
    }

    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        Vector3Int cell = floorTilemap.WorldToCell(worldPosition);
        return new Vector2Int(cell.x, cell.y);
    }
}
