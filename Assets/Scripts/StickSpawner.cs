using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class StickSpawner : MonoBehaviour
{
    public GameObject iStickPrefab;
    public GameObject lStickPrefab;
    public GameObject uStickPrefab;
    public GameObject iStickPrefab_rotated180;
    public GameObject lStickPrefab_rotated180;
    public GameObject uStickPrefab_rotated180;

    public Transform selectionArea;  // SelectionArea GameObject
    public Vector3[] spawnPositions; // Selection alanýnda 3 pozisyon

    private List<GameObject> currentSticks = new List<GameObject>();

    void Start()
    {
        SpawnNewSticks();
    }

    public void SpawnNewSticks()
    {
        ClearSelectionArea();

        GameObject[] pool = new GameObject[] { iStickPrefab, lStickPrefab, uStickPrefab , iStickPrefab_rotated180, lStickPrefab_rotated180, uStickPrefab_rotated180 };

        for (int i = 0; i < 3; i++)
        {
            GameObject newStick = Instantiate(pool[Random.Range(0, pool.Length)], spawnPositions[i], Quaternion.identity, selectionArea);
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
            SpawnNewSticks(); // Tüm stick’ler yerleþtirildiyse, 3 yeni tane getir
        }
    }
}

