using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenManager : MonoBehaviour
{
    [Tooltip("Root GameObject of your Win Screen UI (Panel, Buttons, etc.)")]
    public GameObject winScreenUI;

    [Tooltip("Play Again button inside the Win Screen UI")]
    public Button playAgainButton;

    [Tooltip("Quit button inside the Win Screen UI")]
    public Button quitButton;

    void Awake()
    {
        // make sure the win screen is hidden at start
        winScreenUI.SetActive(false);

        // hook up the button callbacks
        playAgainButton.onClick.AddListener(OnPlayAgain);
        quitButton.onClick.AddListener(OnQuit);
    }

    /// <summary>
    /// Call this when the player has reached 10 points in a row.
    /// </summary>
    public void ShowWinScreen()
    {
        winScreenUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnPlayAgain()
    {
        // reset time scale before reloading
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AnomalyLogic.points = 0; // reset points for the next game
        winScreenUI.SetActive(false);
    }

    private void OnQuit()
    {
        Application.Quit();
    }
}
