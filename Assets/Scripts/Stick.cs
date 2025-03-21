using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


public class Stick : MonoBehaviour, IInputReceiver
{
    public bool isPlaced = false;
    public string shapeType;
    public Vector2Int[] occupiedOffsets;
    public Vector3 startPosition;

    private Vector3 offset;
    private Camera cam;
    public GridManager gridManager;
    public StickSpawner stickSpawner;

    private void Start()
    {
        startPosition = transform.position;
        cam = Camera.main;
        gridManager = FindObjectOfType<GridManager>();
        stickSpawner = FindObjectOfType<StickSpawner>();
    }

    public void OnInputDown(Vector2 worldPos)
    {
        if (isPlaced) return;
        offset = transform.position - (Vector3)worldPos;
    }

    public void OnInputDrag(Vector2 worldPos)
    {
        if (isPlaced) return;
        transform.position = worldPos + (Vector2)offset;
    }

    public void OnInputUp(Vector2 worldPos)
    {
        if (isPlaced) return;

        Vector3 snapped = gridManager.GetNearestGridPosition(transform.position);
        bool success = gridManager.PlaceStick(gameObject, snapped);

        if (success)
        {
            transform.position = snapped;
            isPlaced = true;
            GetComponent<Collider2D>().enabled = false;
            tag = "Untagged";
            stickSpawner.OnStickPlaced(gameObject);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}



