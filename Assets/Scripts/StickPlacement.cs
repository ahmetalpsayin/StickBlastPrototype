using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickPlacement : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject stickPrefab;  // Çubuk prefab'ý

    private Camera cam;
    private GameObject currentStick;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Fare basýlýnca
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

        if (Input.GetMouseButtonUp(0) && isDragging) // Fare býrakýlýnca
        {
            Vector3 snappedPosition = gridManager.GetNearestGridPosition(currentStick.transform.position);
            if (gridManager.PlaceStick(currentStick, snappedPosition))
            {
                currentStick.transform.position = snappedPosition;
            }
            else
            {
                Destroy(currentStick); // Geçerli bir yere koyulmazsa sil
            }

            isDragging = false;
            gridManager.CheckForCompleteRowOrColumn();
        }
    }
}

