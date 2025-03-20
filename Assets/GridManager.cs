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
    public bool PlaceStick(Vector2Int position)
    {
        if (IsCellEmpty(position.x, position.y))
        {
            grid[position.x, position.y].isFilled = true;
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
}

// Grid i�indeki h�crelerin durumunu tutan s�n�f
public class GridCell
{
    public bool isFilled = false; // H�cre dolu mu?
}

