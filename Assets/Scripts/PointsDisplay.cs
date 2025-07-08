using UnityEngine;
using UnityEngine.UI;

public class PointsDisplay : MonoBehaviour
{
    [Tooltip("Referenz auf das UI Text-Element, das den Score anzeigt")]
    public Text pointsText;

    void Update()
    {
        // AnomalyLogic.points auslesen und in den Text packen
        pointsText.text = "Score: " + AnomalyLogic.points;
    }
}
