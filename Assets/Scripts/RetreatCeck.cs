// RetreatCheck.cs
using UnityEngine;

public class RetreatCheck : MonoBehaviour
{
    public void ExecuteCheck()
    {
        Debug.Log("[Retreat] hasAnomalies = " + AnomalyLogic.hasAnomalies);
        var logic = Object.FindFirstObjectByType<AnomalyLogic>();

        if (AnomalyLogic.hasAnomalies)
        {
            // richtig entschieden → Punktestand erhöhen
            AnomalyLogic.points++;
            Debug.Log($"RetreatCheck: correct → points = {AnomalyLogic.points}");

            if (logic != null)
            {
                logic.anomalyDelete();
                logic.anomalyPutNew();
            }
        }
        else
        {
            // falsch entschieden → Punkte-Reset + Anomalie neu/Reset
            AnomalyLogic.points = 0;
            Debug.Log("[Retreat] wrong → reset points to 0");

            if (logic != null)
            {
                logic.anomalyDelete();
                logic.anomalyPutNew();
            }
        }
    }
}
