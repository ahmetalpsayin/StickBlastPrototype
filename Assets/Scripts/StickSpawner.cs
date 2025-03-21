using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

public class StickSpawner : MonoBehaviour
{
    public GameObject iStickPrefab;
    public GameObject lStickPrefab;
    public GameObject uStickPrefab;

    public GridManager gridManager; // örnek

    public Transform selectionAreaParent; // Alt paneldeki çubuklarýn görüneceði yer
    public Vector2[] spawnPositions; // 3 çubuðun ekran üzerindeki pozisyonlarý

    private List<GameObject> currentSticks = new List<GameObject>();

    void Start()
    {
        SpawnNewSticks();
    }

    public void SpawnNewSticks()
    {
        ClearSelectionArea();

        GameObject[] shapes = new GameObject[] { iStickPrefab, lStickPrefab, uStickPrefab };

        for (int i = 0; i < 3; i++)
        {
            GameObject newStick = Instantiate(shapes[i], spawnPositions[i], Quaternion.identity, selectionAreaParent);
            newStick.GetComponent<Stick>().isPlaced = false;
            

            Stick stickScript = newStick.GetComponent<Stick>();
            if (stickScript != null)
            {
                stickScript.isPlaced = false;
                stickScript.gridManager = this.gridManager; 
                stickScript.stickSpawner = this;
            }

            currentSticks.Add(newStick);
        }
    }

    public void OnStickPlaced(GameObject stick)
    {
        if (stick == null) return;

        Stick stickScript = stick.GetComponent<Stick>();
        if (stickScript != null && stickScript.isPlaced == false)
        {
            stickScript.isPlaced = true;
        }

        if (AllSticksPlaced())
        {
            Invoke(nameof(SpawnNewSticks), 0.5f); // Küçük bir beklemeyle yeni çubuklar üret
        }
    }


    private bool AllSticksPlaced()
    {
        foreach (var stick in currentSticks)
        {
            if (stick == null) continue;

            Stick stickScript = stick.GetComponent<Stick>();
            if (stickScript != null && !stickScript.isPlaced)
                return false;
        }
        return true;
    }


    private void ClearSelectionArea()
    {
        foreach (var stick in currentSticks)
        {
            if (stick != null) Destroy(stick);
        }
        currentSticks.Clear();
    }
}

