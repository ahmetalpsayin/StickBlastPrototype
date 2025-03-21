using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private Camera cam;
    private IInputReceiver currentReceiver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        cam = Camera.main;
    }

    void Update()
    {
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);
            if (hit != null)
            {
                currentReceiver = hit.GetComponent<IInputReceiver>();
                currentReceiver?.OnInputDown(mouseWorldPos);
            }
        }

        if (Input.GetMouseButton(0) && currentReceiver != null)
        {
            currentReceiver.OnInputDrag(mouseWorldPos);
        }

        if (Input.GetMouseButtonUp(0) && currentReceiver != null)
        {
            currentReceiver.OnInputUp(mouseWorldPos);
            currentReceiver = null;
        }
    }
}

