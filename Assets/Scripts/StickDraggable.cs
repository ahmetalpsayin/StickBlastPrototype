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

    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
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

        GridManager gm = FindObjectOfType<GridManager>();
        Vector2Int baseNode = gm.GetClosestNode(transform.position);

        bool placed = gm.TryPlaceStickWithOffsets(baseNode, occupiedOffsets);

        if (placed)
        {
            // 1. Edge ve Node'lara beyaz görsel yerleştir
            for (int i = 0; i < occupiedOffsets.Length; i++)
            {
                Vector2Int nodePos = baseNode + occupiedOffsets[i];
                Instantiate(whiteNodePrefab, new Vector3(nodePos.x * gm.spacing, nodePos.y * gm.spacing, 0), Quaternion.identity);
            }

            for (int i = 0; i < occupiedOffsets.Length - 1; i++)
            {
                Vector2Int from = baseNode + occupiedOffsets[i];
                Vector2Int to = baseNode + occupiedOffsets[i + 1];

                Vector3 posA = new Vector3(from.x * gm.spacing, from.y * gm.spacing, 0);
                Vector3 posB = new Vector3(to.x * gm.spacing, to.y * gm.spacing, 0);
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
}

