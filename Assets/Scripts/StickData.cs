using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStickData", menuName = "Stick Blast/Stick Data")]
public class StickData : ScriptableObject
{
    public string stickName;
    public Color stickColor = Color.white;

    // Offset'ler: (0,0), (1,0), (2,0) vs.
    public Vector2Int[] occupiedOffsets;

    // Ýsteðe baðlý: çubuðun görsel prefab'ý
    public GameObject stickPrefab;

    // Döndürülmüþ bir versiyonunu döndür
    public Vector2Int[] GetRotatedOffsets(int rotation90)
    {
        Vector2Int[] rotated = new Vector2Int[occupiedOffsets.Length];
        for (int i = 0; i < occupiedOffsets.Length; i++)
        {
            Vector2Int o = occupiedOffsets[i];
            switch (rotation90 % 4)
            {
                case 0: rotated[i] = o; break;                         // 0°
                case 1: rotated[i] = new Vector2Int(-o.y, o.x); break; // 90°
                case 2: rotated[i] = new Vector2Int(-o.x, -o.y); break;// 180°
                case 3: rotated[i] = new Vector2Int(o.y, -o.x); break; // 270°
            }
        }
        return rotated;
    }
}

