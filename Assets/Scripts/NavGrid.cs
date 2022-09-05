using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class NavGrid : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private NavNode[,] tiles;
    private Vector3 cellSizeOffset;
    private Vector3Int leftClickedCell;
    private Vector3Int rightClickedCell;


    void Start()
    {
        // restore the bounds to the outmost tiles
        tilemap.CompressBounds();
        cellSizeOffset = gameObject.GetComponent<Grid>().cellSize * 0.5f;
        GetTiles();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            leftClickedCell = tilemap.WorldToCell(clickCoords) - tilemap.cellBounds.min;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rightClickedCell = tilemap.WorldToCell(clickCoords) - tilemap.cellBounds.min;
        }
    }

    void OnDrawGizmosSelected()
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
        Handles.color = color;
        Handles.DrawSolidDisc(place + cellSizeOffset, Vector3.forward, 0.2f);
    }

    public void DrawAllTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                NavNode tile = tiles[x, y];
                if (tile == null) continue;

                Color nodeColor = Color.gray;
                if (leftClickedCell.x == x && leftClickedCell.y == y)
                {
                    nodeColor = Color.green;
                }
                else if (rightClickedCell.x == x && rightClickedCell.y == y)
                {
                    nodeColor = Color.red;
                }

                if (tile.Type == NavNodeType.TRAVERSABLE)
                {
                    DrawTileGizmo(tile.Position, nodeColor);
                }
            }
        }
    }
}
