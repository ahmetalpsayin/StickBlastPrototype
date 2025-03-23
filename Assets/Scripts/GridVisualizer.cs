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

    void DrawNodes()
    {
        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                Vector3 worldPos = gridManager.GridToWorldPosition(gridPos);
                Instantiate(nodePrefab, worldPos, Quaternion.identity, transform);
            }
        }
    }

    void DrawEdges()
    {
        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                Vector3 worldCurrent = gridManager.GridToWorldPosition(current);

                // Yatay edge
                if (x < gridManager.nodeGridWidth - 1)
                {
                    Vector2Int right = new Vector2Int(x + 1, y);
                    Vector3 worldRight = gridManager.GridToWorldPosition(right);

                    CreateEdgeBetween(worldCurrent, worldRight);
                }

                // Dikey edge
                if (y < gridManager.nodeGridHeight - 1)
                {
                    Vector2Int top = new Vector2Int(x, y + 1);
                    Vector3 worldTop = gridManager.GridToWorldPosition(top);

                    CreateEdgeBetween(worldCurrent, worldTop);
                }
            }
        }
    }


    void CreateEdgeBetween(Vector3 a, Vector3 b)
    {
        Vector3 mid = (a + b) / 2f;
        Vector3 dir = b - a;
        float length = dir.magnitude;

        GameObject edge = Instantiate(edgePrefab, mid, Quaternion.identity, transform);

        // Uzunluk ve yön ayarla
        edge.transform.right = dir.normalized;
        float scaleMultiplier = 1f; // Ne kadar büyütmek istersen
        edge.transform.localScale = new Vector3(length * scaleMultiplier, 0.1f, 1);
    }
}

