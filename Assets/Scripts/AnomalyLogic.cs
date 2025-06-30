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
        if (Random.value < 1.0f && anomalies != null && anomalies.Length > 0)
        {
            anomalyContainer = new GameObject("Anomalies");

            // Wähle zufällig einen Eintrag aus der Liste
            int idx = Random.Range(0, anomalies.Length);
            var entry = anomalies[idx];

            if (entry.prefab != null && entry.spawnPoint != null)
            {
                // Instanziere exakt an dem Transform, das zum Prefab in der Liste gehört
                Instantiate(
                    entry.prefab, 
                    entry.spawnPoint.position, 
                    entry.spawnPoint.rotation, 
                    anomalyContainer.transform
                );

                hasAnomalies = true;
                Debug.Log($"Anomalie #{idx} erstellt an {entry.spawnPoint.position}");
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
