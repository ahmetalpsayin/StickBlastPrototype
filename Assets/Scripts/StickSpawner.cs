using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class StickSpawner : MonoBehaviour
{

    /*
    public GameObject iStickPrefab;
    public GameObject lStickPrefab;
    public GameObject uStickPrefab;
    public GameObject iStickPrefab_rotated180;
    public GameObject lStickPrefab_rotated180;
    public GameObject uStickPrefab_rotated180;

    */

    [Header("Stick Definitions")]
    public List<StickData> availableSticks;  // ✅ Artık ScriptableObject üzerinden

    public Transform selectionArea;  // SelectionArea GameObject
    public Vector3[] spawnPositions; // Selection alanında 3 pozisyon

    private List<GameObject> currentSticks = new List<GameObject>();

    void Start()
    {
        SpawnNewSticks();
    }

    public void SpawnNewSticks()
    {
        ClearSelectionArea();

        for (int i = 0; i < 3; i++)
        {
            StickData selectedData = availableSticks[Random.Range(0, availableSticks.Count)];
            GameObject newStick = Instantiate(selectedData.stickPrefab, spawnPositions[i], Quaternion.identity, selectionArea);

            // ScriptableObject referansını draggable objeye ver
            StickDraggable stickScript = newStick.GetComponent<StickDraggable>();
            if (stickScript != null)
            {
                stickScript.stickData = selectedData;
            }

            currentSticks.Add(newStick);
        }
    }

    private void ClearSelectionArea()
    {
        foreach (var stick in currentSticks)
        {
            Destroy(stick);
        }
        currentSticks.Clear();
    }

    public void OnStickPlaced(GameObject placedStick)
    {
        currentSticks.Remove(placedStick);
        if (currentSticks.Count == 0)
        {
            SpawnNewSticks(); // Tüm stick’ler yerleştirildiyse, 3 yeni tane getir
        }
    }
}

