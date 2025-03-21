using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameManager : MonoBehaviour
{
    [Header("Grid Ayarlar�")]
    public int gridWidth = 10;
    public int gridHeight = 10;

    [Header("Kamera Ayarlar�")]
    public float extraVerticalSpace = 4f; // Alt taraftaki se�im alan� i�in ekstra alan
    public float minOrthoSize = 6f;

    void Start()
    {
        CenterCameraOnGrid();
    }

    private void CenterCameraOnGrid()
    {
        float centerX = gridWidth / 2f;

        // Kamera ortas�n� biraz a�a��ya kayd�r�yoruz:
        float centerY = (gridHeight / 2f) - 2f;  // Eskiden: + extraVerticalSpace / 2f

        Camera.main.transform.position = new Vector3(centerX, centerY, -10f);

        // Ortho size'� geni�letiyoruz ki �st + alt alan g�r�n�r olsun
        Camera.main.orthographicSize = Mathf.Max(gridHeight / 2f + 4f, minOrthoSize);
    }
}

