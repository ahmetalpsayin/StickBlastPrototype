using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public bool isPainted = false; // Bu h�cre boyanm�� m�?
    public Vector2Int position;    // Sol-alt k��e node pozisyonu

    public GridCell(Vector2Int pos)
    {
        position = pos;
    }
}
