using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour {
    public AudioMixer mixer;
    
    // Wert 0…1 → dB (-80…0)
    public void SetAmbient(float vol) {
        mixer.SetFloat("AmbientVol", Mathf.Log10(Mathf.Clamp(vol,0.0001f,1f)) * 20);
    }
    public void SetFootsteps(float vol) {
        mixer.SetFloat("FootstepsVol", Mathf.Log10(Mathf.Clamp(vol,0.0001f,1f)) * 20);
    }
    public void SetUI(float vol) {
        mixer.SetFloat("UIVol", Mathf.Log10(Mathf.Clamp(vol,0.0001f,1f)) * 20);
    }
}
