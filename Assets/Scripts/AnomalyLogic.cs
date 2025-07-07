using UnityEngine;

public class AnomalyLogic : MonoBehaviour
{
    public static bool hasAnomalies = false;

    [System.Serializable]
    public struct AnomalyEntry
    {
        [Tooltip("Welches Prefab soll gespawnt werden?")]
        public GameObject prefab;

        [Tooltip("Wo in der Welt soll genau dieses Prefab erscheinen?")]
        public Transform spawnPoint;
    }

    [Tooltip("Liste aller möglichen Anomalien mitsamt ihrem festen Spawn-Punkt")]
    public AnomalyEntry[] anomalies;

    private GameObject anomalyContainer;

    private void Start()
    {
        anomalyPutNew();
    }

    public void anomalyPutNew()
    {
        // 50/50 Chance, überhaupt eine Anomalie zu setzen
        if (Random.value < 0.5f && anomalies != null && anomalies.Length > 0)
        {
            anomalyContainer = new GameObject("Anomalies");

            // Wähle zufällig einen Eintrag aus der Liste
            int idx = Random.Range(0, anomalies.Length);
            var entry = anomalies[idx];

            if (entry.prefab != null && entry.spawnPoint != null)
            {
                // 1) Erster Spawn
                Vector3 pos1 = entry.spawnPoint.position;
                Instantiate(
                    entry.prefab,
                    pos1,
                    entry.spawnPoint.rotation,
                    anomalyContainer.transform
                );

                // 2) Zweiter, gespiegelter Spawn um −25 auf Z
                Vector3 pos2 = new Vector3(pos1.x, pos1.y, pos1.z - 25f);
                Instantiate(
                    entry.prefab,
                    pos2,
                    entry.spawnPoint.rotation,
                    anomalyContainer.transform
                );

                hasAnomalies = true;
                Debug.Log($"Anomalie #{idx} erstellt an {pos1} und gespiegelt an {pos2}");
            }
            else
            {
                Debug.LogWarning($"AnomalyLogic: Eintrag #{idx} ist unvollständig!");
                hasAnomalies = false;
            }
        }
        else
        {
            hasAnomalies = false;
            Debug.Log("Keine Anomalie gesetzt (50/50 Entscheidung).");
        }
    }

    public void anomalyDelete()
    {
        if (anomalyContainer != null)
            Destroy(anomalyContainer);
        hasAnomalies = false;
    }
}
