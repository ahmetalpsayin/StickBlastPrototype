using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int position;

    public Node(Vector2Int pos)
    {
        position = pos;
    }

    public override bool Equals(object obj)
    {
        if (obj is Node other)
        {
            return position == other.position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}

