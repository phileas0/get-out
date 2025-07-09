// AdvanceCheck.cs
using UnityEngine;

public class AdvanceCheck : MonoBehaviour
{
    public void ExecuteCheck()
    {
        Debug.Log("[Advance] hasAnomalies = " + AnomalyLogic.hasAnomalies);
        var logic = Object.FindFirstObjectByType<AnomalyLogic>();

        if (!AnomalyLogic.hasAnomalies)
        {
            // richtig entschieden → Punktestand erhöhen
            AnomalyLogic.points++;
            Debug.Log($"AdvanceCheck: correct → points = {AnomalyLogic.points}");
            // check for win
            if (AnomalyLogic.points >= 10)
            {
                var win = Object.FindFirstObjectByType<WinScreenManager>();
                if (win != null)
                    win.ShowWinScreen();
                return; // skip spawning more anomalies
            }


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
            Debug.Log("[Advance] wrong → reset points to 0");
            var win = Object.FindFirstObjectByType<WinScreenManager>();
            if (win != null) win.winScreenUI.SetActive(false);

            if (logic != null)
            {
                logic.anomalyDelete();
                logic.anomalyPutNew();
            }
        }
    }
}
