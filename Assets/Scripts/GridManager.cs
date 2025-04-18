﻿using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public int nodeGridWidth = 6;   // 6x6 grid örneği
    public int nodeGridHeight = 6;

    private Node[,] nodes;
    private Dictionary<Edge, bool> occupiedEdges = new Dictionary<Edge, bool>();


    [Header("Visual Prefabs")]
    public GameObject nodeVisualPrefab;
    public GameObject edgeVisualPrefab;

    [SerializeField]
    public float spacing = 1.5f;

    private GridCell[,] cellGrid;

    void Start()
    {
        InitializeNodesAndEdges();

    }

    private void InitializeNodesAndEdges()
    {

        // Hücreleri oluştur (node sayısından 1 eksik çünkü aradaki alanlar)
        cellGrid = new GridCell[nodeGridWidth - 1, nodeGridHeight - 1];

        for (int x = 0; x < nodeGridWidth - 1; x++)
        {
            for (int y = 0; y < nodeGridHeight - 1; y++)
            {
                cellGrid[x, y] = new GridCell(new Vector2Int(x, y));
            }
        }

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
                // 🔧 Node'ların world pozisyonunu spacing'e göre hesapla
                Vector2 nodeWorld = new Vector2(x * spacing, y * spacing);
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
            Debug.Log($" Filled edge between {e.nodeA.position} and {e.nodeB.position}");
        }


        //  Tüm kenarları yerleştirdikten sonra boyalı hücre kontrolü
        CheckAndPaintEnclosedCells();     // 🟩 önce boyalı alanları kontrol et
        CheckAndClearCompletedLines();    // 💥 sonra satır/sütun patlat

        return true;
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * spacing, gridPos.y * spacing, 0f);
    }

    public bool CanPlaceStickBetween(Vector2Int fromPos, Vector2Int toPos)
    {
        if (!IsValidNode(fromPos) || !IsValidNode(toPos))
            return false;

        Vector2Int diff = toPos - fromPos;
        if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) != 1)
            return false; // Sadece yatay/dikey komşu olabilir

        Edge edgeToCheck = new Edge(new Node(fromPos), new Node(toPos));

        if (occupiedEdges.TryGetValue(edgeToCheck, out bool isOccupied))
        {
            return !isOccupied;
        }

        return false; // Böyle bir edge yoksa geçersizdir
    }
    public bool CanPlaceStickWithOffsets(Vector2Int baseNode, Vector2Int[] offsets)
    {
        for (int i = 0; i < offsets.Length - 1; i++)
        {
            Vector2Int from = baseNode + offsets[i];
            Vector2Int to = baseNode + offsets[i + 1];

            if (!IsValidNode(from) || !IsValidNode(to))
                return false;

            Edge edge = new Edge(new Node(from), new Node(to));

            if (!occupiedEdges.ContainsKey(edge) || occupiedEdges[edge])
                return false;
        }

        return true;
    }


    private bool IsCellEnclosed(int x, int y)
    {
        Node bl = nodes[x, y];
        Node br = nodes[x + 1, y];
        Node tr = nodes[x + 1, y + 1];
        Node tl = nodes[x, y + 1];

        Edge bottom = new Edge(bl, br);
        Edge right = new Edge(br, tr);
        Edge top = new Edge(tl, tr);
        Edge left = new Edge(bl, tl);

        return
            occupiedEdges.ContainsKey(bottom) && occupiedEdges[bottom] &&
            occupiedEdges.ContainsKey(right) && occupiedEdges[right] &&
            occupiedEdges.ContainsKey(top) && occupiedEdges[top] &&
            occupiedEdges.ContainsKey(left) && occupiedEdges[left];
    }

    public void CheckAndPaintEnclosedCells()
    {
        for (int x = 0; x < nodeGridWidth - 1; x++)
        {
            for (int y = 0; y < nodeGridHeight - 1; y++)
            {
                GridCell cell = cellGrid[x, y];

                if (!cell.isPainted && IsCellEnclosed(x, y))
                {
                    cell.isPainted = true;
                    Debug.Log($"🟩 Painted cell at ({x},{y})");

                    // Opsiyonel: sahneye görsel boya efekti ekle
                    Vector3 worldPos = GridToWorldPosition(new Vector2Int(x, y)) + new Vector3(spacing / 2f, spacing / 2f, 0);
                    GameObject fill = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    fill.transform.position = worldPos;
                    fill.transform.localScale = Vector3.one * spacing * 0.9f;  
                    fill.name = $"Paint_{x}_{y}";  
                    var renderer = fill.GetComponent<Renderer>();
                    renderer.material = new Material(Shader.Find("Sprites/Default")); // 🔄 Unlit shader
                    renderer.material.color = Color.cyan;
                }
            }
        }
    }

    public void CheckAndClearCompletedLines()
    {
        int width = cellGrid.GetLength(0);
        int height = cellGrid.GetLength(1);

        // ✅ Satırlar (y ekseni boyunca)
        for (int y = 0; y < height; y++)
        {
            bool rowComplete = true;
            for (int x = 0; x < width; x++)
            {
                if (!cellGrid[x, y].isPainted)
                {
                    rowComplete = false;
                    break;
                }
            }

            if (rowComplete)
            {
                Debug.Log($"💥 Row {y} is complete!");
                ClearRow(y);
            }
        }

        // ✅ Sütunlar (x ekseni boyunca)
        for (int x = 0; x < width; x++)
        {
            bool columnComplete = true;
            for (int y = 0; y < height; y++)
            {
                if (!cellGrid[x, y].isPainted)
                {
                    columnComplete = false;
                    break;
                }
            }

            if (columnComplete)
            {
                Debug.Log($"💥 Column {x} is complete!");
                ClearColumn(x);
            }
        }
    }

    private void ClearRow(int y)
    {
        int width = cellGrid.GetLength(0);
        for (int x = 0; x < width; x++)
        {
            cellGrid[x, y].isPainted = false;

            // 💡 Sahnedeki renkli kareleri de bul ve yok et (quad veya tile)
            RemoveVisualAtCell(x, y);

            // 🔄 O hücreyi çevreleyen kenarları boşalt
            ClearCellEdges(x, y);
        }
    }

    private void ClearColumn(int x)
    {
        int height = cellGrid.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            cellGrid[x, y].isPainted = false;
            RemoveVisualAtCell(x, y);

            // 🔄 O hücreyi çevreleyen kenarları boşalt
            ClearCellEdges(x, y);
        }
    }

    private void RemoveVisualAtCell(int x, int y)
    {
        Vector3 worldPos = GridToWorldPosition(new Vector2Int(x, y)) + new Vector3(spacing / 2f, spacing / 2f, 0);
        /*
        // Çevresinde küçük bir alanda quad varsa yok et
        Collider[] hits = Physics.OverlapSphere(worldPos, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.gameObject.name.Contains("Quad") || hit.gameObject.name.Contains("Paint"))
            {
                StartCoroutine(FadeAndDestroy(hit.gameObject, 0.4f));
            }
        }
        */

        string name = $"Paint_{x}_{y}";
        GameObject quad = GameObject.Find(name);

        if (quad != null)
        {
            StartCoroutine(FadeAndDestroy(quad, 0.4f));
        }
        else
        {
            Debug.LogWarning($"❌ Quad not found for cell {x},{y}");
        }
    }

    IEnumerator FadeAndDestroy(GameObject obj, float duration = 0.5f)
    {
        if (obj == null) yield break;

        Renderer rend = obj.GetComponent<Renderer>();
        if (rend == null) { Destroy(obj); yield break; }

        Color startColor = rend.material.color;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            rend.material.color = Color.Lerp(startColor, Color.clear, t);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }

    private void ClearCellEdges(int x, int y)
    {
        Node bl = nodes[x, y];
        Node br = nodes[x + 1, y];
        Node tr = nodes[x + 1, y + 1];
        Node tl = nodes[x, y + 1];

        Edge[] edges = {
        new Edge(bl, br),
        new Edge(br, tr),
        new Edge(tl, tr),
        new Edge(bl, tl)
    };

        foreach (Edge edge in edges)
        {
            if (occupiedEdges.ContainsKey(edge))
            {
                occupiedEdges[edge] = false;

                // ✅ Görsel edge tekrar oluştur
                Vector3 worldA = GridToWorldPosition(edge.nodeA.position);
                Vector3 worldB = GridToWorldPosition(edge.nodeB.position);
                Vector3 mid = (worldA + worldB) / 2f;
                Vector3 dir = worldB - worldA;

                GameObject edgeVisual = Instantiate(edgeVisualPrefab, mid, Quaternion.identity);
                edgeVisual.transform.right = dir.normalized;
                edgeVisual.transform.localScale = new Vector3(dir.magnitude, 0.15f, 1f);
            }
        }

        // ✅ Köşe node'ları da yeşil hale getir
        Vector2Int[] nodePositions = { bl.position, br.position, tr.position, tl.position };
        foreach (Vector2Int pos in nodePositions)
        {
            Vector3 worldPos = GridToWorldPosition(pos);
            Instantiate(nodeVisualPrefab, worldPos, Quaternion.identity);
        }
    }

    private void RemoveNodeVisualAt(Vector3 worldPos)
    {
        Collider[] hits = Physics.OverlapSphere(worldPos, 0.1f); // Küçük bir yarıçapla ara
        foreach (var hit in hits)
        {
            if (hit.gameObject.name.Contains("NodeVisual"))
            {
                Destroy(hit.gameObject);
            }
        }
    }

    private void RemoveEdgeVisualBetween(Vector3 from, Vector3 to)
    {
        Vector3 mid = (from + to) / 2f;
        Collider[] hits = Physics.OverlapSphere(mid, 0.1f); // Kenarın ortasına bak

        foreach (var hit in hits)
        {
            if (hit.gameObject.name.Contains("EdgeVisual"))
            {
                Destroy(hit.gameObject);
            }
        }
    }
}
