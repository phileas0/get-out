using UnityEngine;

// Dieses Script verwendet einen CharacterController für Kollisionen, Treppensteige-Logik und Sprint
public class OVRLocomotionManager : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Root deines Rigs (z.B. [BuildingBlock] Camera Rig)")]
    public Transform rigRoot;
    [Tooltip("Head-/Kamera-Transform, um Bewegungsrichtung abzuleiten")]
    public Transform cameraTransform;

    private CharacterController cc;
    private float verticalVelocity = 0f;

    [Header("Bewegung")]
    [Tooltip("Einheiten pro Sekunde vorwärts/rückwärts")]
    public float moveSpeed = 2f;
    [Tooltip("Faktor für Sprint-Geschwindigkeit")]
    public float sprintMultiplier = 2f;
    [Tooltip("Deadzone für den linken Stick")]
    public float moveDeadZone = 0.2f;

    [Header("Drehung")]
    [Tooltip("Grad pro Sekunde für kontinuierliches Drehen")]
    public float turnSpeed = 90f;
    [Tooltip("Deadzone für den rechten Stick")]
    public float turnDeadZone = 0.2f;

    [Header("Gravity")]
    [Tooltip("Gravitationskraft")]
    public float gravity = -9.81f;

    void Awake()
    {
        if (rigRoot == null)
            Debug.LogError("OVRLocomotionManager: rigRoot ist nicht gesetzt!");
        cc = rigRoot.GetComponent<CharacterController>();
        if (cc == null)
            Debug.LogError("OVRLocomotionManager: Kein CharacterController auf rigRoot gefunden!");
    }

    void Update()
    {
        // 1) Eingabe für Sprint-Trigger
        bool isSprinting = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.1f
                          || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch) > 0.1f;

        // 2) Bewegung mit linkem Stick
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
        Vector3 motion = Vector3.zero;
        if (moveInput.magnitude > moveDeadZone)
        {
            // Blickrichtung in XZ-Ebene
            Vector3 forward = cameraTransform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cameraTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 dir = forward * moveInput.y + right * moveInput.x;
            float currentSpeed = moveSpeed * (isSprinting ? sprintMultiplier : 1f);
            motion = dir * currentSpeed;
        }

        // 3) Gravity anwenden
        if (cc.isGrounded)
            verticalVelocity = 0f;
        verticalVelocity += gravity * Time.deltaTime;
        motion.y = verticalVelocity;

        // 4) Bewegung mit Kollision
        cc.Move(motion * Time.deltaTime);

        // 5) Drehung mit rechtem Stick
        Vector2 turnInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick, OVRInput.Controller.Touch);
        if (Mathf.Abs(turnInput.x) > turnDeadZone)
        {
            float angle = turnInput.x * turnSpeed * Time.deltaTime;
            rigRoot.Rotate(Vector3.up, angle, Space.World);
        }
    }
}
