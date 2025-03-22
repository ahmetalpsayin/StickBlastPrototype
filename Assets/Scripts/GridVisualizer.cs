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
                Vector3 pos = new Vector3(x * nodeSpacing, y * nodeSpacing, 0);
                Instantiate(nodePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    void DrawEdges()
    {
        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector3 current = new Vector3(x * nodeSpacing, y * nodeSpacing, 0);
                // Yatay edge
                if (x < gridManager.nodeGridWidth - 1)
                {
                    Vector3 right = new Vector3((x + 1) * nodeSpacing, y * nodeSpacing, 0);
                    CreateEdgeBetween(current, right);
                }
                // Dikey edge
                if (y < gridManager.nodeGridHeight - 1)
                {
                    Vector3 top = new Vector3(x * nodeSpacing, (y + 1) * nodeSpacing, 0);
                    CreateEdgeBetween(current, top);
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

