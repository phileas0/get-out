using UnityEngine;
using TMPro;  // TextMeshPro namespace

public class PointsDisplay : MonoBehaviour
{
    [Tooltip("Referenz auf das TMP Text-Element, das den Score anzeigt")]
    public TMP_Text pointsText;

    void Update()
    {
        pointsText.text = "Score: " + AnomalyLogic.points;
    }
}
