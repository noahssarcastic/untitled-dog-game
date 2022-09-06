using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using NavigationGrid;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;

    private Node[,] tiles;
    private Vector3 cellSizeOffset;
    private Vector3Int leftClickedCell;
    private Vector3Int rightClickedCell;


    void Start()
    {
        // restore the bounds to the outmost tiles
        tilemap.CompressBounds();
        cellSizeOffset = gameObject.GetComponent<UnityEngine.Grid>().cellSize * 0.5f;
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

    void OnDrawGizmos()
    {
        if (tiles != null)
        {
            DrawAllAdjacencies();
            DrawAllNodes();
        }
    }

    public void GetTiles()
    {
        if (tiles == null)
            tiles = new Node[tilemap.cellBounds.size.x, tilemap.cellBounds.size.y];

        for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
        {
            for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
            {
                Vector3Int tileLocalPosition = new Vector3Int(x, y, 0);
                Vector3 tileWorldPosition = tilemap.CellToWorld(tileLocalPosition);
                if (tilemap.HasTile(tileLocalPosition))
                    tiles[x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin] = new Node(NodeType.TRAVERSABLE, tileWorldPosition, new NodeIndex(x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin));
                else
                    tiles[x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin] = new Node(NodeType.EMPTY, tileWorldPosition, new NodeIndex(x - tilemap.cellBounds.xMin, y - tilemap.cellBounds.yMin));
            }
        }
        CullInaccessibleTiles();
        GetAdjacencies();
    }

    public void CullInaccessibleTiles()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            // We want to exclude the ceiling tiles
            for (int y = 0; y < tiles.GetLength(1) - 1; y++)
            {
                Node tile = tiles[x, y];
                Node aboveTile = tiles[x, y + 1];
                if (aboveTile.type != NodeType.EMPTY)
                    tile.type = NodeType.INACCESSIBLE;
            }
        }
    }

    public void DrawNodeGizmo(Vector3 place, Color color)
    {
        Handles.color = color;
        Handles.DrawSolidDisc(place + cellSizeOffset, Vector3.forward, 0.2f);
    }

    public void DrawAdjacency(Vector3 from, Vector3 to)
    {
        Handles.color = Color.gray;
        Handles.DrawLine(from + cellSizeOffset, to + cellSizeOffset, 3f);
    }

    public void DrawAllNodes()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Node tile = tiles[x, y];
                if (tile?.type == NodeType.TRAVERSABLE)
                    DrawNodeGizmo(tile.position, GetNodeColor(x, y));
            }
        }
    }

    public void DrawAllAdjacencies()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Node tile = tiles[x, y];
                if (tile?.type != NodeType.TRAVERSABLE) continue;

                foreach (NodeIndex adjacentNode in tile.adjacencies)
                {
                    DrawAdjacency(tile.position, tiles[adjacentNode.x, adjacentNode.y].position);
                }
            }
        }
    }

    private Color GetNodeColor(int x, int y)
    {
        if (leftClickedCell.x == x && leftClickedCell.y == y)
            return Color.green;
        else if (rightClickedCell.x == x && rightClickedCell.y == y)
            return Color.red;
        else
            return Color.grey;
    }

    public void GetAdjacencies()
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Node tile = tiles[x, y];
                if (tile?.type != NodeType.TRAVERSABLE) continue;

                NodeIndex index = tile.index;
                if (index.x != 0)
                {
                    Node left = tiles[index.x - 1, index.y];
                    if (left.type == NodeType.TRAVERSABLE) tile.adjacencies.Add(left.index);
                }
                if (index.x != tiles.GetLength(0) - 1)
                {
                    Node right = tiles[index.x + 1, index.y];
                    if (right.type == NodeType.TRAVERSABLE) tile.adjacencies.Add(right.index);
                }
            }
        }
    }
}
