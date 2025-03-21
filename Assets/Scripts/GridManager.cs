﻿using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int nodeGridWidth = 6;   // 6x6 grid örneği
    public int nodeGridHeight = 6;

    private Node[,] nodes;
    private Dictionary<Edge, bool> occupiedEdges = new Dictionary<Edge, bool>();

    [SerializeField]
    public float spacing = 1.5f;

    void Start()
    {
        InitializeNodesAndEdges();
         
    }

    private void InitializeNodesAndEdges()
    {
        nodes = new Node[nodeGridWidth, nodeGridHeight];

        for (int x = 0; x < nodeGridWidth; x++)
        {
            for (int y = 0; y < nodeGridHeight; y++)
            {
                nodes[x, y] = new Node(new Vector2Int(x, y));
            }
        }

        // Tüm olası yatay ve dikey kenarları oluştur
        for (int x = 0; x < nodeGridWidth; x++)
        {
            for (int y = 0; y < nodeGridHeight; y++)
            {
                if (x < nodeGridWidth - 1)
                    occupiedEdges[new Edge(nodes[x, y], nodes[x + 1, y])] = false;

                if (y < nodeGridHeight - 1)
                    occupiedEdges[new Edge(nodes[x, y], nodes[x, y + 1])] = false;
            }
        }
    }
     

    // 1. En yakın node'u bulur
    public Vector2Int GetClosestNode(Vector2 worldPosition)
    {
        Vector2Int closest = Vector2Int.zero;
        float minDistance = float.MaxValue;

        for (int x = 0; x < nodeGridWidth; x++)
        {
            for (int y = 0; y < nodeGridHeight; y++)
            {
                Vector2 nodeWorld = new Vector2(x, y);
                float dist = Vector2.Distance(worldPosition, nodeWorld);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = new Vector2Int(x, y);
                }
            }
        }

        return closest;
    }

    private bool IsValidNode(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < nodeGridWidth && pos.y >= 0 && pos.y < nodeGridHeight;
    }

    // 2. Offset’li stick’i yerleştirir
    public bool TryPlaceStickWithOffsets(Vector2Int baseNode, Vector2Int[] offsets)
    {
        List<Edge> edgesToOccupy = new List<Edge>();

        for (int i = 0; i < offsets.Length - 1; i++)
        {
            Vector2Int from = baseNode + offsets[i];
            Vector2Int to = baseNode + offsets[i + 1];

            Edge edge = new Edge(new Node(from), new Node(to));

            if (occupiedEdges.ContainsKey(edge) && !occupiedEdges[edge])
            {
                edgesToOccupy.Add(edge);
            }
            else
            {
                Debug.Log($"Edge between {from} and {to} is already occupied or invalid");
                return false;
            }
        }

        foreach (Edge e in edgesToOccupy)
        {
            occupiedEdges[e] = true;
            Debug.Log($"✅ Filled edge between {e.nodeA.position} and {e.nodeB.position}");
        }

        return true;
    }

}
