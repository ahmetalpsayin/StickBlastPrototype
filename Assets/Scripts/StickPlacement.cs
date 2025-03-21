using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlacement : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stickPrefab;  // �ubuk prefab'�

    private Camera cam;
    private GameObject currentStick;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Fare bas�l�nca
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentStick = Instantiate(stickPrefab, mousePos, Quaternion.identity);
            isDragging = true;
        }

        if (isDragging && currentStick != null)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            currentStick.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0) && isDragging) // Fare b�rak�l�nca
        {
            Vector3 snappedPosition = gridManager.GetNearestGridPosition(currentStick.transform.position);
            if (gridManager.PlaceStick(currentStick, snappedPosition))
            {
                currentStick.transform.position = snappedPosition;
            }
            else
            {
                Destroy(currentStick); // Ge�erli bir yere koyulmazsa sil
            }

            isDragging = false;
            gridManager.CheckForCompleteRowOrColumn();
        }
    }
}

