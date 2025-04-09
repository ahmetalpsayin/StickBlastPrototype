using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "StickBlast/Level Data")]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public class PresetStick
    {
        public StickData stickData;           // ScriptableObject tan�m�
        public Vector2Int baseNodePosition;   // Yerle�tirilece�i grid pozisyonu
        public int rotation;                  // 0, 90, 180, 270 d�n��
    }

    public List<PresetStick> presetSticks;
}
