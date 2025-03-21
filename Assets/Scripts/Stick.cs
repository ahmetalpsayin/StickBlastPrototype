using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Stick : MonoBehaviour
{
    public bool isPlaced = false;
    public string shapeType;
    public Vector2Int[] occupiedOffsets;

    [HideInInspector] public Vector3 startPosition;

    // Otomatik ofset tanýmlama (tercihe baðlý)
    void Awake()
    {
        if (occupiedOffsets == null || occupiedOffsets.Length == 0)
        {
            switch (shapeType)
            {
                case "I":
                    occupiedOffsets = new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1)
                    };
                    break;
                case "L":
                    occupiedOffsets = new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(0, 1),
                        new Vector2Int(1, 0)
                    };
                    break;
                case "U":
                    occupiedOffsets = new Vector2Int[] {
                        new Vector2Int(0, 0),
                        new Vector2Int(1, 0),
                        new Vector2Int(2, 0),
                        new Vector2Int(1, 1)
                    };
                    break;
            }
        }

        // Doðduðu konumu kaydet
        startPosition = transform.position;
    }
}


