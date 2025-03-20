using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlacement : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stickPrefab;  // Çubuk prefab'ý

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Fare týklanýnca veya mobilde dokunulunca
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int gridPos = WorldToGrid(mousePos);

            if (gridManager.PlaceStick(gridPos))
            {
                Instantiate(stickPrefab, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity);
            }

            gridManager.CheckForCompleteRowOrColumn();
        }
    }

    private Vector2Int WorldToGrid(Vector2 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
    }
}
