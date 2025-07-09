using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Dein VR-Kamera-Transform oder Main Camera (wird automatisch zugewiesen)")]
    public Transform cameraTransform;

    [Header("Ambient Sound")]
    [Tooltip("Loopender Ambient-Clip")]
    public AudioClip ambientClip;
    [Tooltip("AudioMixerGroup für Ambient")]
    public AudioMixerGroup ambientMixerGroup;

    [Header("Footsteps")]
    [Tooltip("Loopender Footstep-Clip")]
    public AudioClip footstepClip;
    [Tooltip("AudioMixerGroup für Footsteps")]
    public AudioMixerGroup footstepsMixerGroup;

    [Header("Einstellungen")]
    [Tooltip("Deadzone für linken Stick, ab wann Footsteps starten")]
    public float moveDeadZone = 0.2f;
    [Tooltip("Pitch im Walk (normal = 1)")]
    public float walkPitch = 1f;
    [Tooltip("Pitch im Sprint (z.B. 1.5)")]
    public float sprintPitch = 1.5f;

    private AudioSource ambientSource;
    private AudioSource footstepSource;

    void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (cameraTransform != null && cameraTransform.GetComponent<AudioListener>() == null)
            cameraTransform.gameObject.AddComponent<AudioListener>();
    }

    void Start()
    {
        ambientSource = gameObject.AddComponent<AudioSource>();
        ambientSource.clip = ambientClip;
        ambientSource.outputAudioMixerGroup = ambientMixerGroup;
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;
        ambientSource.spatialBlend = 0f; // 2D

        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.clip = footstepClip;
        footstepSource.outputAudioMixerGroup = footstepsMixerGroup;
        footstepSource.loop = true;
        footstepSource.playOnAwake = false;
        footstepSource.spatialBlend = 0f; // 2D

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

        // Sprint per Stick-Click
        bool isSprinting = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);

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
            // Passe Pitch: 1x beim Gehen, 1.5x beim Sprinten
            footstepSource.pitch = isSprinting ? sprintPitch : walkPitch;
        }
        else if (footstepSource.isPlaying)
        {
            footstepSource.Stop();
            Debug.Log("Footsteps stopped.");
        }

        Debug.Log($"isMoving: {isMoving}, isSprinting: {isSprinting}");
    }
}
