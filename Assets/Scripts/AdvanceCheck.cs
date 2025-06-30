// AdvanceCheck.cs
using UnityEngine;

public class AdvanceCheck : MonoBehaviour
{
    public void ExecuteCheck()
    {
        Debug.Log("[Check] ExecuteCheck aufgerufen – hasAnomalies = " + AnomalyLogic.hasAnomalies);

        if (!AnomalyLogic.hasAnomalies)
        {
            var anomalyLogic = Object.FindFirstObjectByType<AnomalyLogic>();
            if (anomalyLogic != null)
            {
                anomalyLogic.anomalyDelete();
                anomalyLogic.anomalyPutNew();
            }
            else
            {
                Debug.LogWarning("AdvanceCheck: Kein AnomalyLogic in der Szene gefunden!");
            }
        }
        else
        {
            Debug.Log("[Check] Quitting…");
            Application.Quit();
        }
    }
}
