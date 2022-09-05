using System.Collections.Generic;

using UnityEngine;

public enum NavNodeType
{
    EMPTY,
    TRAVERSABLE,
    INACCESSIBLE
}

public struct NavNodeIndex
{
    public NavNodeIndex(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public readonly int x;
    public readonly int y;
}

public class NavNode
{
    public NavNode(NavNodeType _type, Vector3 _position, NavNodeIndex _index)
    {
        type = _type;
        position = _position;
        index = _index;
        adjacencies = new List<NavNodeIndex>();
    }

    public NavNodeType type;
    public Vector3 position;
    public NavNodeIndex index;
    public List<NavNodeIndex> adjacencies;
}