using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class StickSpawner : MonoBehaviour
{
    public GameObject iStickPrefab;
    public GameObject lStickPrefab;
    public GameObject uStickPrefab;
    public GameObject iStickPrefab_rotated90;
    public GameObject lStickPrefab_rotated90;
    public GameObject uStickPrefab_rotated90;

    public Transform selectionArea;  // SelectionArea GameObject
    public Vector3[] spawnPositions; // Selection alan�nda 3 pozisyon

    private List<GameObject> currentSticks = new List<GameObject>();

    void Start()
    {
        SpawnNewSticks();
    }

    public void SpawnNewSticks()
    {
        ClearSelectionArea();

        GameObject[] pool = new GameObject[] { iStickPrefab, lStickPrefab, uStickPrefab , iStickPrefab_rotated90 };

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
            SpawnNewSticks(); // T�m stick�ler yerle�tirildiyse, 3 yeni tane getir
        }
    }
}

