using UnityEngine;

namespace NavigationGrid
{
    public delegate void DoForEachCallback(Grid grid, Node node);

    public class Grid
    {
        public Grid(Vector2Int _size)
        {
            tiles = new Node[_size.x, _size.y];
            size = _size;
        }

        public Grid(int x, int y)
        {
            tiles = new Node[x, y];
            size = new Vector2Int(x, y);
        }

        public readonly Vector2Int size;
        private readonly Node[,] tiles;

        public Node GetNode(Vector2Int index)
        {
            return tiles[index.x, index.y];
        }

        public void SetNode(Vector2Int index, NodeType type, Vector3 position)
        {
            tiles[index.x, index.y] = new Node(type, position, index);
        }

        public Node GetNode(int x, int y)
        {
            return tiles[x, y];
        }

        public void DoForEach(DoForEachCallback func)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
                for (int y = 0; y < tiles.GetLength(1); y++)
                    func(this, tiles[x, y]);
        }
    }
}
