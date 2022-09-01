using UnityEngine;

public enum NavNodeType
{
    EMPTY,
    TRAVERSABLE,
    INACCESSIBLE
}

public class NavNode
{
    public NavNode(NavNodeType type, Vector3 position)
    {
        Type = type;
        Position = position;
    }

    public NavNodeType Type { get; set; }
    public Vector3 Position { get; private set; }
}