using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public Toggle muteToggle;

    private bool isMuted;

    void Start()
    {
        // Lädt die gespeicherten Einstellungen beim Start des Spiels
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        volumeSlider.value = savedVolume;

        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        muteToggle.isOn = isMuted;

        // Stellt sicher, dass der Audio-Status beim Start korrekt ist
        if (isMuted)
        {
            ApplyMuteState(true);
        }
        else
        {
            SetVolume(savedVolume);
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Beende Spiel...");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    
    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            
            float dbVolume = volume > 0.001f ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("MasterVolume", dbVolume);
        }
        
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    
    public void ToggleMute(bool muteStatus)
    {
        isMuted = muteStatus;
        ApplyMuteState(isMuted);
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
    }

    
    private void ApplyMuteState(bool muteStatus)
    {
        if (muteStatus)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            
            SetVolume(volumeSlider.value);
        }
    }
}