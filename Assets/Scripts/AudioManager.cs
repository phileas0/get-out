using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// AudioManager für ambienten Hintergrundsound und Footsteps in VR.
/// An einem leeren GameObject als Komponente hinzufügen.
/// Im Inspector: CameraTransform, AmbientClip, FootstepClip sowie die beiden AudioMixerGroups zuweisen.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Dein VR-Kamera-Transform oder Main Camera (wird automatisch zugewiesen)")]
    public Transform cameraTransform;

    [Header("Ambient Sound")]
    [Tooltip("Loopender Ambient-Clip")] public AudioClip ambientClip;
    [Tooltip("AudioMixerGroup für Ambient")] public AudioMixerGroup ambientMixerGroup;

    [Header("Footsteps")]
    [Tooltip("Loopender Footstep-Clip")] public AudioClip footstepClip;
    [Tooltip("AudioMixerGroup für Footsteps")] public AudioMixerGroup footstepsMixerGroup;

    [Header("Einstellungen")]
    [Tooltip("Deadzone für linken Stick, ab wann Footsteps starten")] public float moveDeadZone = 0.2f;
    [Tooltip("Pitch im Walk (normal = 1)")] public float walkPitch = 1f;
    [Tooltip("Pitch im Sprint (z.B. 2)")] public float sprintPitch = 2f;

    private AudioSource ambientSource;
    private AudioSource footstepSource;

    void Awake()
    {
        // Kamera-Transform automatisch finden, falls nicht gesetzt
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        
        // AudioListener sicherstellen
        if (cameraTransform != null && cameraTransform.GetComponent<AudioListener>() == null)
            cameraTransform.gameObject.AddComponent<AudioListener>();
    }

    void Start()
    {
        // Ambient-Source einrichten
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientClip;
        ambientSource.outputAudioMixerGroup = ambientMixerGroup;
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;  // Play im Script starten
        ambientSource.spatialBlend = 0f; // 2D

        // Footstep-Source einrichten
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepClip;
        footstepSource.outputAudioMixerGroup = footstepsMixerGroup;
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
        footstepSource.spatialBlend = 0f; // 2D

        // Starte Ambient-Sound explizit
        if (ambientClip != null)
        {
            ambientSource.Play();
            Debug.Log("Ambient sound started.");
        }
        else Debug.LogWarning("AmbientClip ist nicht im Inspector gesetzt!");
    }

    void Update()
    {
        // Bewegungseingabe abfragen
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        bool isMoving = moveInput.magnitude > moveDeadZone;

        // Sprint per analogem Trigger
        float leftTrig  = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        float rightTrig = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);
        bool isSprinting = leftTrig > 0.1f || rightTrig > 0.1f;

        // Footsteps abspielen / stoppen und pitchen
        if (isMoving)
        {
            if (!footstepSource.isPlaying)
            {
                if (footstepClip != null)
                {
                    footstepSource.Play();
                    Debug.Log("Footsteps started.");
                }
                else Debug.LogWarning("FootstepClip ist nicht im Inspector gesetzt!");
            }
            footstepSource.pitch = isSprinting ? sprintPitch : walkPitch;
        }
        else if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
            Debug.Log("Footsteps stopped.");
        }

        // Debugging-Ausgabe für Bewegung und Sprint
        Debug.Log($"isMoving: {isMoving}, isSprinting: {isSprinting}");
    }
}
