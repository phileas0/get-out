using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public float introDuration = 5f; // Länge des Intros in Sekunden

    private void Start()
    {
        // Starte den automatischen Szenenwechsel nach introDuration
        Invoke(nameof(LoadMainMenu), introDuration);
    }

    private void LoadMainMenu()
    {
        // Szene mit Index 1 laden (bitte anpassen, falls dein MainMenu-Index anders ist)
        SceneManager.LoadScene(1);
    }
}
