using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10;  // Izgara geni�li�i
    public int height = 10; // Izgara y�ksekli�i
    public float cellSize = 1f; // H�cre boyutu

    private GridCell[,] grid; // Grid verisini tutacak dizi

    void Start()
    {
        InitializeGrid();
    }

    // Izgaray� ba�lat
    private void InitializeGrid()
    {
        grid = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new GridCell();
            }
        }
    }

    // H�creye �ubuk yerle�tir
    public bool PlaceStick(GameObject stick, Vector3 worldPosition)
    {
        Vector2Int gridPos = WorldToGridPosition(worldPosition);

        if (IsCellEmpty(gridPos.x, gridPos.y))
        {
            grid[gridPos.x, gridPos.y].isFilled = true;
            stick.transform.position = GridToWorldPosition(gridPos); // Stick'i grid'e hizala
            return true;
        }
        return false;
    }

    // H�cre dolu mu kontrol et
    public bool IsCellEmpty(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return !grid[x, y].isFilled;
    }

    // Sat�r/S�tun tamamlanm�� m� kontrol et
    public bool CheckForCompleteRowOrColumn()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsRowComplete(y))
            {
                ClearRow(y);
                return true;
            }
        }

        for (int x = 0; x < width; x++)
        {
            if (IsColumnComplete(x))
            {
                ClearColumn(x);
                return true;
            }
        }
        return false;
    }

    // Sat�r tamamland� m�?
    private bool IsRowComplete(int row)
    {
        for (int x = 0; x < width; x++)
        {
            if (!grid[x, row].isFilled) return false;
        }
        return true;
    }

    // S�tun tamamland� m�?
    private bool IsColumnComplete(int column)
    {
        for (int y = 0; y < height; y++)
        {
            if (!grid[column, y].isFilled) return false;
        }
        return true;
    }

    // Sat�r� temizle
    private void ClearRow(int row)
    {
        for (int x = 0; x < width; x++)
        {
            grid[x, row].isFilled = false;
        }
    }

    // S�tunu temizle
    private void ClearColumn(int column)
    {
        for (int y = 0; y < height; y++)
        {
            grid[column, y].isFilled = false;
        }
    }

    // D�nya pozisyonunu grid koordinat�na �evirir
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.y / cellSize);
        return new Vector2Int(x, y);
    }

    // Grid koordinat�n� d�nya pozisyonuna �evirir
    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0);
    }

    // En yak�n grid pozisyonunu d�nd�r�r
    public Vector3 GetNearestGridPosition(Vector3 worldPosition)
    {
        Vector2Int gridPos = WorldToGridPosition(worldPosition);
        return GridToWorldPosition(gridPos);
    }
}

// Grid i�indeki h�crelerin durumunu tutan s�n�f
public class GridCell
{
    public bool isFilled = false; // H�cre dolu mu?
}
