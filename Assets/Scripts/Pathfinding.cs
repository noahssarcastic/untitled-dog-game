using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using NavigationGrid;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;

    private NavigationGrid.Grid grid;
    private Vector3 cellSizeOffset;
    private Vector3Int leftClickedCell;
    private Vector3Int rightClickedCell;


    public void Start()
    {
        // restore the bounds to the outmost tiles
        tilemap.CompressBounds();

        cellSizeOffset = gameObject.GetComponent<UnityEngine.Grid>().cellSize * 0.5f;

        grid = new NavigationGrid.Grid((Vector2Int)tilemap.size);
        GenerateGrid();
    }

    public void Update()
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

    private Vector2Int CellToNode(Vector2Int cellIndex)
    {
        return cellIndex - (Vector2Int)tilemap.cellBounds.min;
    }

    private Vector2Int NodeToCell(Vector2Int nodeIndex)
    {
        return nodeIndex + (Vector2Int)tilemap.cellBounds.min;
    }

    private void CreateNode(Vector2Int cellIndex)
    {
        Vector3Int tileLocalPosition = new Vector3Int(cellIndex.x, cellIndex.y, 0);
        Vector3 tileWorldPosition = tilemap.CellToWorld(tileLocalPosition);
        Vector2Int nodeIndex = CellToNode(cellIndex);
        NodeType type = tilemap.HasTile(tileLocalPosition) ?
            NodeType.TRAVERSABLE :
            NodeType.EMPTY;
        grid.SetNode(nodeIndex, type, tileWorldPosition);
    }

    private void GenerateGrid()
    {
        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                CreateNode(new Vector2Int(x, y));

        CullInaccessibleTiles();
        GetAdjacencies();
    }

    private void CullInaccessibleTiles()
    {
        for (int x = 0; x < grid.size.x; x++)
            // -1 b/c we want to exclude the ceiling tiles
            for (int y = 0; y < grid.size.y - 1; y++)
            {
                Node tile = grid.GetNode(x, y);
                Node aboveTile = grid.GetNode(x, y + 1);
                if (aboveTile.type != NodeType.EMPTY)
                    tile.type = NodeType.INACCESSIBLE;
            }
    }

    private void GetAdjacencies()
    {
        for (int x = 0; x < grid.size.x; x++)
            for (int y = 0; y < grid.size.y; y++)
            {
                Node tile = grid.GetNode(x, y);
                if (tile?.type != NodeType.TRAVERSABLE) continue;

                Vector2Int index = tile.index;
                if (index.x != 0)
                {
                    Node left = grid.GetNode(index.x - 1, index.y);
                    if (left.type == NodeType.TRAVERSABLE) tile.adjacencies.Add(left.index);
                }
                if (index.x != grid.size.x - 1)
                {
                    Node right = grid.GetNode(index.x + 1, index.y);
                    if (right.type == NodeType.TRAVERSABLE) tile.adjacencies.Add(right.index);
                }
            }
    }

    public void OnDrawGizmos()
    {
        if (grid != null)
        {
            DrawAllAdjacencies();
            DrawAllNodes();
        }
    }

    private void DrawNodeGizmo(Vector3 place, Color color)
    {
        Handles.color = color;
        Handles.DrawSolidDisc(place + cellSizeOffset, Vector3.forward, 0.2f);
    }

    private void DrawAllNodes()
    {
        for (int x = 0; x < grid.size.x; x++)
            for (int y = 0; y < grid.size.y; y++)
            {
                Node tile = grid.GetNode(x, y);
                if (tile?.type == NodeType.TRAVERSABLE)
                    DrawNodeGizmo(tile.position, GetNodeColor(x, y));
            }
    }

    public void DrawAdjacency(Vector3 from, Vector3 to)
    {
        Handles.color = Color.gray;
        Handles.DrawLine(from + cellSizeOffset, to + cellSizeOffset, 3f);
    }

    public void DrawAllAdjacencies()
    {
        for (int x = 0; x < grid.size.x; x++)
            for (int y = 0; y < grid.size.y; y++)
            {
                Node tile = grid.GetNode(x, y);
                if (tile?.type != NodeType.TRAVERSABLE) continue;

                foreach (Vector2Int adjacentNode in tile.adjacencies)
                    DrawAdjacency(tile.position, grid.GetNode(adjacentNode).position);
            }

    }

    /// <summary>
    /// Get a Node's color based on its clicked status.
    /// </summary>
    /// <param name="x">The node's x-index.</param>
    /// <param name="y">The node's y-index</param>
    private Color GetNodeColor(int x, int y)
    {
        if (leftClickedCell.x == x && leftClickedCell.y == y)
            return Color.green;
        else if (rightClickedCell.x == x && rightClickedCell.y == y)
            return Color.red;
        else
            return Color.grey;
    }
}
