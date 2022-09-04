using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class NavGrid : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private NavNode[,] tiles;
    private Vector3 cellSizeOffset;

    void Start()
    {
        // restore the bounds to the outmost tiles
        tilemap.CompressBounds();
        cellSizeOffset = gameObject.GetComponent<Grid>().cellSize * 0.5f;
        GetTiles();
    }

    void OnDrawGizmos()
    {
        if (tiles != null)
            DrawAllTiles();
    }

    public void GetTiles()
    {
        if (tiles == null)
        {
            tiles = new NavNode[tilemap.cellBounds.size.x, tilemap.cellBounds.size.y];
        }

        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int tileLocalPosition = new Vector3Int(x, y, 0);
                Vector3 tileWorldPosition = tilemap.CellToWorld(tileLocalPosition);
                if (tilemap.HasTile(tileLocalPosition))
                    tiles[x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin] = new NavNode(NavNodeType.TRAVERSABLE, tileWorldPosition);
                else
                    tiles[x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin] = new NavNode(NavNodeType.EMPTY, tileWorldPosition);
            }
        }
        CullInaccessibleTiles();
    }

    public void CullInaccessibleTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            // We want to exclude the ceiling tiles
            for (int y = 0; y < tiles.GetLength(1) - 1; y++)
            {
                NavNode tile = tiles[x, y];
                NavNode aboveTile = tiles[x, y + 1];
                if (aboveTile.Type != NavNodeType.EMPTY)
                {
                    tile.Type = NavNodeType.INACCESSIBLE;
                }
            }
        }
    }

    public void DrawTileGizmo(Vector3 place, Color color)
    {
        // Gizmos.color = color;
        // Gizmos.DrawSphere(place + cellSizeOffset, 0.2f);
        Handles.color = color;
        Handles.DrawWireDisc(place + cellSizeOffset, Vector3.forward, 0.2f, 5f);
    }

    public void DrawAllTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                NavNode tile = tiles[x, y];
                if (tile == null) continue;
                if (tile.Type == NavNodeType.TRAVERSABLE)
                {
                    DrawTileGizmo(tile.Position, Color.green);
                }
            }
        }
    }
}
