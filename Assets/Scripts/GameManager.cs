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

    public LevelData levelData;
    public GridManager gridManager;

    void Start()
    {
        CenterCameraOnGrid();

        LoadLevel();
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

    public void LoadLevel()
    {
        foreach (var stick in levelData.presetSticks)
        {
            // Rotasyonlu offset'leri al
            Vector2Int[] rotatedOffsets = stick.stickData.GetRotatedOffsets(stick.rotation);

            // Kenarlarý yerleþtir
            bool placed = gridManager.TryPlaceStickWithOffsets(stick.baseNodePosition, rotatedOffsets);

            if (placed)
            {
                // Görsel node ve edge prefablarýný yerleþtir
                for (int i = 0; i < rotatedOffsets.Length; i++)
                {
                    Vector2Int nodePos = stick.baseNodePosition + rotatedOffsets[i];
                    Vector3 world = gridManager.GridToWorldPosition(nodePos);
                    Instantiate(gridManager.nodeVisualPrefab, world, Quaternion.identity);
                }

                for (int i = 0; i < rotatedOffsets.Length - 1; i++)
                {
                    Vector2Int from = stick.baseNodePosition + rotatedOffsets[i];
                    Vector2Int to = stick.baseNodePosition + rotatedOffsets[i + 1];
                    Vector3 mid = (gridManager.GridToWorldPosition(from) + gridManager.GridToWorldPosition(to)) / 2f;
                    Vector3 dir = gridManager.GridToWorldPosition(to) - gridManager.GridToWorldPosition(from);
                    float length = dir.magnitude;

                    GameObject edge = Instantiate(gridManager.edgeVisualPrefab, mid, Quaternion.identity);
                    edge.transform.right = dir.normalized;
                    edge.transform.localScale = new Vector3(length, 0.2f, 1f);
                }
            }
        }
    }
}

