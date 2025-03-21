using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedStick;
    private Vector3 offset;
    private bool isDragging = false;
    public GridManager gridManager; // Referans ekledik

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0)) // Hold
        {
            TryPickUpStick();
        }
        else if (isDragging && Input.GetMouseButton(0)) // Drag
        {
            DragStick();
        }
        else if (isDragging && Input.GetMouseButtonUp(0)) // Release
        {
            DropStick();
        }
    }

    void TryPickUpStick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null && hit.collider.CompareTag("Stick"))
        {
            selectedStick = hit.collider.gameObject;
            offset = selectedStick.transform.position - GetMouseWorldPos();
            isDragging = true;
        }
    }

    void DragStick()
    {
        if (selectedStick != null)
        {
            selectedStick.transform.position = GetMouseWorldPos() + offset;
        }
    }

    void DropStick()
    {
        if (selectedStick != null)
        {
            Vector3 snappedPosition = gridManager.GetNearestGridPosition(selectedStick.transform.position); // GridManager'dan hizalama al
            selectedStick.transform.position = snappedPosition;
            gridManager.PlaceStick(selectedStick, snappedPosition); // Çubuðu yerleþtir
            selectedStick = null;
            isDragging = false;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; // Adjust if needed based on camera settings
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
}
