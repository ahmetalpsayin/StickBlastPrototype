using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameManager : MonoBehaviour
{
    [Header("Grid Ayarlarý")]
    public int gridWidth = 10;
    public int gridHeight = 10;

    [Header("Kamera Ayarlarý")]
    public float extraVerticalSpace = 4f; // Alt taraftaki seçim alaný için ekstra alan
    public float minOrthoSize = 6f;

    void Start()
    {
        CenterCameraOnGrid();
    }

    private void CenterCameraOnGrid()
    {
        float centerX = gridWidth / 2f;

        // Kamera ortasýný biraz aþaðýya kaydýrýyoruz:
        float centerY = (gridHeight / 2f) - 2f;  // Eskiden: + extraVerticalSpace / 2f

        Camera.main.transform.position = new Vector3(centerX, centerY, -10f);

        // Ortho size'ý geniþletiyoruz ki üst + alt alan görünür olsun
        Camera.main.orthographicSize = Mathf.Max(gridHeight / 2f + 4f, minOrthoSize);
    }
}

