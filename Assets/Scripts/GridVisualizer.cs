using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GridVisualizer : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public GridManager gridManager;

    public float nodeSpacing = 1f;

    void Start()
    {
        nodeSpacing = gridManager.spacing; // default: 1 → artık 4 birim aralıklı

        DrawNodes();
        DrawEdges();
    }

    public void DrawNodes()
    {
        foreach (var node in gridManager.nodeVisuals)
        {
            if (Application.isPlaying)
                Destroy(node);
            else
                DestroyImmediate(node);
        }
        gridManager.nodeVisuals.Clear();

        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = gridManager.GridToWorldPosition(gridPos);
                GameObject n = Instantiate(nodePrefab, worldPos, Quaternion.identity, transform);
                gridManager.nodeVisuals.Add(n);
            }
        }
    }

    public void DrawEdges()
    {
        foreach (var edge in gridManager.edgeVisuals)
            Destroy(edge);
        gridManager.edgeVisuals.Clear();

        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                Vector3 worldCurrent = gridManager.GridToWorldPosition(current);

                if (x < gridManager.nodeGridWidth - 1)
                {
                    Vector2Int right = new Vector2Int(x + 1, y);
                    Vector3 worldRight = gridManager.GridToWorldPosition(right);
                    gridManager.edgeVisuals.Add(CreateEdgeBetween(worldCurrent, worldRight));
                }

                if (y < gridManager.nodeGridHeight - 1)
                {
                    Vector2Int top = new Vector2Int(x, y + 1);
                    Vector3 worldTop = gridManager.GridToWorldPosition(top);
                    gridManager.edgeVisuals.Add(CreateEdgeBetween(worldCurrent, worldTop));
                }
            }
        }
    }


    GameObject CreateEdgeBetween(Vector3 a, Vector3 b)
    {
        Vector3 mid = (a + b) / 2f;
        Vector3 dir = b - a;
        float length = dir.magnitude;

        GameObject edge = Instantiate(edgePrefab, mid, Quaternion.identity, transform);
        edge.transform.right = dir.normalized;
        edge.transform.localScale = new Vector3(length, 0.1f, 1);

        return edge;
    }

}

