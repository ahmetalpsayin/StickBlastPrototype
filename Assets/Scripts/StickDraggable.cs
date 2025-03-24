using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickDraggable : MonoBehaviour
{
    public Vector2Int[] occupiedOffsets;

    private Vector3 startPosition;
    private Camera cam;
    private bool isDragging = false;

    public GameObject whiteNodePrefab;
    public GameObject whiteEdgePrefab;

    public GameObject highlightNodePrefab;
    public GameObject highlightEdgePrefab;

    private GameObject currentEdgeHighlight;
    private List<GameObject> currentNodeHighlights = new List<GameObject>();

    private Vector2Int? lastHighlightedFrom = null; 

    GridManager gridManager;

    void Start()
    {

        gridManager = FindObjectOfType<GridManager>();
        cam = Camera.main;
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
         

        Vector2Int? validBase = FindValidPlacementBaseNode();

        if (validBase != null)
        {
            // Yeni hedef farklıysa, highlight'ı yenile
            if (lastHighlightedFrom != validBase)
            {
                ClearHighlight();
                ShowHighlightForOffsets(validBase.Value, GetStickColor());
            }
        }
        else
        {
            // Highlight varsa temizle
            ClearHighlight();
        }


       
         
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        isDragging = false;
         
        Vector2Int baseNode = gridManager.GetClosestNode(transform.position);

        bool placed = gridManager.TryPlaceStickWithOffsets(baseNode, occupiedOffsets);

        if (placed)
        {
            // 1. Edge ve Node'lara beyaz görsel yerleştir
            for (int i = 0; i < occupiedOffsets.Length; i++)
            {
                Vector2Int nodePos = baseNode + occupiedOffsets[i];
                Vector3 worldPos = gridManager.GridToWorldPosition(nodePos);
                Instantiate(whiteNodePrefab, worldPos, Quaternion.identity);
            }

            for (int i = 0; i < occupiedOffsets.Length - 1; i++)
            {
                Vector2Int from = baseNode + occupiedOffsets[i];
                Vector2Int to = baseNode + occupiedOffsets[i + 1];

                Vector3 posA = gridManager.GridToWorldPosition(from);
                Vector3 posB = gridManager.GridToWorldPosition(to);
                Vector3 midPoint = (posA + posB) / 2f;
                Vector3 dir = posB - posA;
                float length = dir.magnitude;

                GameObject edgeVis = Instantiate(whiteEdgePrefab, midPoint, Quaternion.identity);
                edgeVis.transform.right = dir.normalized;
                edgeVis.transform.localScale = new Vector3(length, 0.2f, 1f);
            }

            // 2. StickSpawner'a haber ver
            StickSpawner spawner = FindObjectOfType<StickSpawner>();
            if (spawner != null)
                spawner.OnStickPlaced(gameObject);

            // 3. Kendini sahneden sil
            Destroy(gameObject);
        }
        else
        {
            transform.position = startPosition; // geri dön
        }
    }

    void ShowHighlightForOffsets(Vector2Int baseNode, Color stickColor)
    {
        // Edge highlight
        for (int i = 0; i < occupiedOffsets.Length - 1; i++)
        {
            Vector2Int from = baseNode + occupiedOffsets[i];
            Vector2Int to = baseNode + occupiedOffsets[i + 1];

            Vector3 worldA = gridManager.GridToWorldPosition(from);
            Vector3 worldB = gridManager.GridToWorldPosition(to);
            Vector3 mid = (worldA + worldB) / 2f;
            Vector3 dir = worldB - worldA;
            float length = dir.magnitude;

            GameObject edge = Instantiate(highlightEdgePrefab, mid, Quaternion.identity);

            // Yön ve scale
            if (Mathf.Abs(from.x - to.x) == 1)
                edge.transform.localScale = new Vector3(length, 0.2f, 1f);
            else
                edge.transform.localScale = new Vector3(0.2f, length, 1f);

            edge.GetComponent<SpriteRenderer>().color = Color.Lerp(stickColor, Color.gray, 0.5f);
            currentEdgeHighlight = edge;
        }

        // Node highlight
        foreach (Vector2Int offset in occupiedOffsets)
        {
            Vector2Int nodePos = baseNode + offset;
            Vector3 worldPos = gridManager.GridToWorldPosition(nodePos);

            GameObject node = Instantiate(highlightNodePrefab, worldPos, Quaternion.identity);
            node.GetComponent<SpriteRenderer>().color = Color.Lerp(stickColor, Color.gray, 0.5f);
            currentNodeHighlights.Add(node);
        }

        lastHighlightedFrom = baseNode;
    }

     

    void ClearHighlight()
    {
        if (currentEdgeHighlight != null)
            Destroy(currentEdgeHighlight);

        foreach (var node in currentNodeHighlights)
        {
            Destroy(node);
        }

        currentNodeHighlights.Clear();
        lastHighlightedFrom = null; 
    }

    Color GetStickColor()
    {
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        return sr != null ? sr.color : Color.black;
    }

    Vector2Int? FindValidTargetEdge(Vector2Int from)
    { 

        Vector2Int[] directions = {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

        foreach (var dir in directions)
        {
            Vector2Int to = from + dir;
            if (gridManager.CanPlaceStickBetween(from, to))
            {
                return to;
            }
        }

        return null;
    }


    Vector2Int? FindValidPlacementBaseNode()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int nearest = gridManager.GetClosestNode(mouseWorld);

        Vector2Int? best = null;
        float bestDist = float.MaxValue;

        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector2Int baseNode = new Vector2Int(x, y);
                if (gridManager.CanPlaceStickWithOffsets(baseNode, occupiedOffsets))
                {
                    Vector3 worldPos = gridManager.GridToWorldPosition(baseNode);
                    float dist = Vector2.Distance(worldPos, mouseWorld);

                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        best = baseNode;
                    }
                }
            }
        }

        return best;
    }



}

