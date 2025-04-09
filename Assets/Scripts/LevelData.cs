using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "StickBlast/Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class PresetStick
    {
        public StickData stickData;           // ScriptableObject tanýmý
        public Vector2Int baseNodePosition;   // Yerleþtirileceði grid pozisyonu
        public int rotation;                  // 0, 90, 180, 270 dönüþ
    }

    public List<PresetStick> presetSticks;
}
