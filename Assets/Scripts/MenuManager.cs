using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadScene("Loop Hallway");
    }
    public void OpenSettings() {
        // hier Settings-Panel einblenden
    }
    public void QuitGame() {
        Application.Quit();
    }
}
