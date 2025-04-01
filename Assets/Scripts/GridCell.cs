using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public bool isPainted = false; // Bu hücre boyanmýþ mý?
    public Vector2Int position;    // Sol-alt köþe node pozisyonu

    public GridCell(Vector2Int pos)
    {
        position = pos;
    }
}
