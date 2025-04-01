using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
[CustomEditor(typeof(StickDraggable))]
public class StickOffsetGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StickDraggable stick = (StickDraggable)target;

        if (GUILayout.Button("💡 Generate occupiedOffsets from children"))
        {
            GenerateOffsets(stick);
        }
    }

    private void GenerateOffsets(StickDraggable stick)
    {
        List<Vector2Int> offsets = new List<Vector2Int>();
        Vector3 basePos = Vector3.zero;

        // Tüm çocuk SpriteRenderer’ları bul
        SpriteRenderer[] children = stick.GetComponentsInChildren<SpriteRenderer>();

        if (children.Length == 0)
        {
            Debug.LogWarning("No SpriteRenderers found under the prefab.");
            return;
        }

        // İlk sprite pozisyonunu referans al
        basePos = children[0].transform.localPosition;

        foreach (var sr in children)
        {
            Vector3 localPos = sr.transform.localPosition;
            Vector2Int offset = new Vector2Int(
                Mathf.RoundToInt(localPos.x / stick.gridManager.spacing),
                Mathf.RoundToInt(localPos.y / stick.gridManager.spacing)
            );

            offsets.Add(offset);
        }

        // Sıralı hale getir (isteğe bağlı)
        offsets.Sort((a, b) =>
        {
            if (a.x != b.x) return a.x.CompareTo(b.x);
            return a.y.CompareTo(b.y);
        });

        stick.occupiedOffsets = offsets.ToArray();

        Debug.Log($"✅ {offsets.Count} occupiedOffsets generated for '{stick.name}'");
        EditorUtility.SetDirty(stick);
    }
}

