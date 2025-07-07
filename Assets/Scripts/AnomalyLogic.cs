using System.Collections.Generic;
using UnityEngine;

public class AnomalyLogic : MonoBehaviour
{
    public static bool hasAnomalies = false;

    [System.Serializable]
    public struct AnomalyEntry
    {
        public GameObject prefab;    // prefab zum Spawnen
        public Transform spawnPoint; // spawn-Position
    }

    [System.Serializable]
    public struct RemovalEntry
    {
        public GameObject[] objectsToRemove; // die konkreten Instanzen in der Szene
    }

    public AnomalyEntry[] anomalies;        // mögliche neue Anomalien
    public RemovalEntry[] removalEntries;   // korrespondierende Szenen-Objekte zum Deaktivieren

    private GameObject anomalyContainer;    
    private List<GameObject> removedObjects = new List<GameObject>();

    private void Start()
    {
        anomalyPutNew();
    }

    public void anomalyPutNew()
    {
        // 50/50: überhaupt eine Anomalie?
        if (Random.value >= 0.5f)
        {
            hasAnomalies = false;
            Debug.Log("AnomalyLogic: Keine Anomalie in diesem Level.");
            return;
        }

        // 50/50: neu spawnen oder vorhandene entfernen
        if (Random.value < 0.5f)
            SpawnNewAnomaly();
        else
            RemoveExistingAnomaly();
    }

    private void SpawnNewAnomaly()
    {
        if (anomalies == null || anomalies.Length == 0)
        {
            Debug.LogWarning("AnomalyLogic: Keine Einträge zum Spawnen!");
            hasAnomalies = false;
            return;
        }

        anomalyContainer = new GameObject("Anomalies");
        int idx = Random.Range(0, anomalies.Length);
        var entry = anomalies[idx];

        if (entry.prefab == null || entry.spawnPoint == null)
        {
            Debug.LogWarning($"AnomalyLogic: Eintrag #{idx} unvollständig!");
            hasAnomalies = false;
            return;
        }

        Vector3 p1 = entry.spawnPoint.position;
        Vector3 p2 = new Vector3(p1.x, p1.y, p1.z - 25f);

        Instantiate(entry.prefab, p1, entry.spawnPoint.rotation, anomalyContainer.transform);
        Instantiate(entry.prefab, p2, entry.spawnPoint.rotation, anomalyContainer.transform);

        hasAnomalies = true;
        Debug.Log($"AnomalyLogic: Neue Anomalie #{idx} gespawnt an {p1} & {p2}");
    }

    private void RemoveExistingAnomaly()
    {
        if (removalEntries == null || removalEntries.Length == 0)
        {
            Debug.LogWarning("AnomalyLogic: Keine Einträge zum Entfernen!");
            hasAnomalies = false;
            return;
        }

        int idx = Random.Range(0, removalEntries.Length);
        var removal = removalEntries[idx];

        removedObjects.Clear();
        foreach (var go in removal.objectsToRemove)
        {
            if (go != null && go.activeSelf)
            {
                go.SetActive(false);
                removedObjects.Add(go);
            }
        }

        hasAnomalies = removedObjects.Count > 0;
        Debug.Log($"AnomalyLogic: Entfernte Instanzen für Entry #{idx}");
    }

    public void anomalyDelete()
    {
        // re-aktivieren
        foreach (var go in removedObjects)
            if (go != null)
                go.SetActive(true);
        removedObjects.Clear();

        // frisch gespawnte Prefabs aufräumen
        if (anomalyContainer != null)
            Destroy(anomalyContainer);

        hasAnomalies = false;
        Debug.Log("AnomalyLogic: Alle Anomalien zurückgesetzt.");
    }
}
