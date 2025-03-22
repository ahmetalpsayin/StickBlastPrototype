using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class StickDraggable : MonoBehaviour
{
    public Vector2Int[] occupiedOffsets;

    private Vector3 startPosition;
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;

        // Eğer elle atamadıysan: örnek I çubuğu
        if (occupiedOffsets == null || occupiedOffsets.Length == 0)
        {
            occupiedOffsets = new Vector2Int[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1)
            };
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

        GridManager gm = FindObjectOfType<GridManager>();
        Vector2Int nearestNode = gm.GetClosestNode(transform.position);

        bool placed = gm.TryPlaceStickWithOffsets(nearestNode, occupiedOffsets);

        if (placed)
        {
            StickSpawner spawner = FindObjectOfType<StickSpawner>();
            if (spawner != null)
                spawner.OnStickPlaced(gameObject);
        }
        else
        {
            transform.position = startPosition; // geri dön
        }
    }
}
