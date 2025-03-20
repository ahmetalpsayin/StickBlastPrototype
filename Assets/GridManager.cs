using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GridManager : MonoBehaviour
{
    public int width = 10;  // Izgara geniþliði
    public int height = 10; // Izgara yüksekliði
    public float cellSize = 1f; // Hücre boyutu

    private GridCell[,] grid; // Grid verisini tutacak dizi

    void Start()
    {
        InitializeGrid();
    }

    // Izgarayý baþlat
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

    // Hücreye çubuk yerleþtir
    public bool PlaceStick(Vector2Int position)
    {
        if (IsCellEmpty(position.x, position.y))
        {
            grid[position.x, position.y].isFilled = true;
            return true;
        }
        return false;
    }

    // Hücre dolu mu kontrol et
    public bool IsCellEmpty(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return false;
        return !grid[x, y].isFilled;
    }

    // Satýr/Sütun tamamlanmýþ mý kontrol et
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

    // Satýr tamamlandý mý?
    private bool IsRowComplete(int row)
    {
        for (int x = 0; x < width; x++)
        {
            if (!grid[x, row].isFilled) return false;
        }
        return true;
    }

    // Sütun tamamlandý mý?
    private bool IsColumnComplete(int column)
    {
        for (int y = 0; y < height; y++)
        {
            if (!grid[column, y].isFilled) return false;
        }
        return true;
    }

    // Satýrý temizle
    private void ClearRow(int row)
    {
        for (int x = 0; x < width; x++)
        {
            grid[x, row].isFilled = false;
        }
    }

    // Sütunu temizle
    private void ClearColumn(int column)
    {
        for (int y = 0; y < height; y++)
        {
            grid[column, y].isFilled = false;
        }
    }
}

// Grid içindeki hücrelerin durumunu tutan sýnýf
public class GridCell
{
    public bool isFilled = false; // Hücre dolu mu?
}

