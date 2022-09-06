using System.Collections.Generic;
using UnityEngine;

namespace NavigationGrid
{
    /// <summary>
    /// Represents the type of node.
    /// </summary>
    /// <remarks>
    /// A node can be one of:
    ///     <list type="bullet">
    ///         <item><description>TRAVERSABLE</description></item>
    ///         <item><description>INACCESSIBLE (ignored)</description></item>
    ///         <item><description>EMPTY (ignored)</description></item>
    ///     </list>
    /// </remarks>
    public enum NodeType
    {
        /// <summary>
        /// Represents an empty space.
        /// </summary>
        EMPTY,
        /// <summary>
        /// Represents a position which can be walked to.
        /// </summary>
        TRAVERSABLE,
        /// <summary>
        /// Represents a position which is obscured.
        /// </summary>
        INACCESSIBLE
    }

    /// <summary>
    /// Represents a Node's two-part index in a Grid.
    /// </summary>
    public struct NodeIndex
    {
        public NodeIndex(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public readonly int x;
        public readonly int y;
    }

    /// <summary>
    /// Models a node in a NavigationGrid system.
    /// </summary>
    public class Node
    {
        public Node(NodeType _type, Vector3 _position, NodeIndex _index)
        {
            type = _type;
            position = _position;
            index = _index;
            adjacencies = new List<NodeIndex>();
        }

        /// <summary>
        /// The type of node.
        /// </summary>
        public NodeType type;

        /// <summary>
        /// The node's position in world coordinates.
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// The node's two-part Grid index.
        /// </summary>
        public NodeIndex index;

        /// <summary>
        /// A list of all adjacent nodes' NodeIndex.
        /// </summary>
        public List<NodeIndex> adjacencies;
    }
}