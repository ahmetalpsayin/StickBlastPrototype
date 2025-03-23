using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBackgroundFiller : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;
    public GridManager gridManager;

    void Start()
    {
        for (int x = 0; x < gridManager.nodeGridWidth; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, tile);
            }
        }

        tilemap.transform.localScale = Vector3.one * gridManager.spacing; // spacing ile hizala
    }
}

