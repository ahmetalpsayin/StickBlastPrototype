using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGrid : MonoBehaviour
{
    public int width = 10;  // Grid geni�li�i
    public int height = 10; // Grid y�ksekli�i
    public Tilemap tilemap; // Tilemap referans�
    public Tile tile;       // Grid i�in kullan�lacak Tile

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }
}
