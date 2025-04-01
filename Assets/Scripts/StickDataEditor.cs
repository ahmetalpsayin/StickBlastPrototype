using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(StickData))]
public class StickDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StickData stickData = (StickData)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("🔧 Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("💡 Generate Offsets From Prefab Children"))
        {
            if (stickData.stickPrefab == null)
            {
                Debug.LogWarning("⚠️ Please assign a stickPrefab first.");
                return;
            }

            GenerateOffsets(stickData);
        }
    }

    private void GenerateOffsets(StickData stickData)
    {
        GameObject prefab = stickData.stickPrefab;
        if (prefab == null) return;

        List<Vector2Int> offsets = new List<Vector2Int>();
        SpriteRenderer[] renderers = prefab.GetComponentsInChildren<SpriteRenderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("❌ No SpriteRenderers found in prefab.");
            return;
        }

        Vector3 basePos = renderers[0].transform.localPosition;

        foreach (var sr in renderers)
        {
            Vector3 localPos = sr.transform.localPosition;
            Vector2 diff = localPos - basePos;
            Vector2Int offset = new Vector2Int(Mathf.RoundToInt(diff.x), Mathf.RoundToInt(diff.y));
            offsets.Add(offset);
        }

        // İsteğe bağlı: sıralı hale getir
        offsets.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));

        stickData.occupiedOffsets = offsets.ToArray();
        Debug.Log($"✅ Generated {offsets.Count} offsets for '{stickData.name}'");

        EditorUtility.SetDirty(stickData);
        AssetDatabase.SaveAssets();
    }
}


