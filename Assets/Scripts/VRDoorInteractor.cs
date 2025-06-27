using UnityEngine;
using DoorScript;             // Namespace deines Door-Scripts
using OVR;                    // für OVRInput

public class VRDoorInteractor : MonoBehaviour
{
    [Tooltip("Maximaler Abstand zum Öffnen in m")]
    public float reach = 2.5f;

    [Tooltip("World-Space-Canvas mit 'Drücke A zum Öffnen'-Text")]
    public GameObject interactPrompt;

    private Transform head;

    void Start()
    {
        // die Camera deines OVRCameraRig (Center Eye)
        head = GetComponentInChildren<Camera>().transform;
        if (interactPrompt) interactPrompt.SetActive(false);
    }

    void Update()
    {
        // 1) Raycast aus der Kopfposition
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, reach))
        {
            // 2) Versuche, das Door-Script im Parent zu finden
            Door door = hit.collider.GetComponentInParent<Door>();
            if (door != null)
            {
                // 3) Prompt einblenden
                if (interactPrompt) interactPrompt.SetActive(true);

                // 4) A-Button drücken?
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    door.OpenDoor();
                }
                return;
            }
        }
        // nichts getroffen oder kein Door → Prompt verstecken
        if (interactPrompt) interactPrompt.SetActive(false);
    }
}
