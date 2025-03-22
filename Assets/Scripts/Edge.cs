using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node nodeA;
    public Node nodeB;

    public Edge(Node a, Node b)
    {
        // Ayný sýrada oluþturmak için sýralama
        if (a.position.x < b.position.x || (a.position.x == b.position.x && a.position.y < b.position.y))
        {
            nodeA = a;
            nodeB = b;
        }
        else
        {
            nodeA = b;
            nodeB = a;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is Edge other)
        {
            return nodeA.position == other.nodeA.position && nodeB.position == other.nodeB.position;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return nodeA.position.GetHashCode() ^ nodeB.position.GetHashCode();
    }
}



