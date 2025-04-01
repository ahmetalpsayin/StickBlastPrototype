using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBackgroundFiller : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;
    public GridManager gridManager;

    [Header("Tile Settings")]
    public bool clearBeforeFill = true;
 


    void Start()
    {
        if (tilemap == null || tile == null || gridManager == null)
        {
            Debug.LogWarning("TilemapFiller: Missing references.");
            return;
        } 

        // 🔧 1. Tilemap'i spacing'e göre ölçekle
        tilemap.transform.localScale = Vector3.one * gridManager.spacing;// spacing ile hizala
        tilemap.transform.localPosition = Vector3.zero;

        // ✅ 2. Tilemap'i node'ların merkezine kaydır
        tilemap.transform.localPosition = Vector3.zero;
        //tilemap.transform.localPosition = new Vector3(gridManager.spacing / 2f, gridManager.spacing / 2f, 0f);
        //tilemap.transform.localPosition = new Vector3( 0.5f * gridManager.spacing, 0.5f * gridManager.spacing, 0f );

        // 🔨 3. Ardından tile'ları bas
        FillTilemap();
    }

    void FillTilemap()
    {
        if (clearBeforeFill)
            tilemap.ClearAllTiles();

        for (int x = 0; x < gridManager.nodeGridWidth -1; x++)
        {
            for (int y = 0; y < gridManager.nodeGridHeight -1; y++)
            { 
                tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // Sync scale with GridManager.spacing
        tilemap.transform.localScale = Vector3.one * gridManager.spacing;

        Debug.Log("✅ Tilemap filled and scaled to match GridManager.");
    }
     
}

