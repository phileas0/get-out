using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Grundgeschwindigkeit in m/s")]
    public float moveSpeed = 2f;
    [Tooltip("Multiplikator beim Sprinten")]
    public float sprintMultiplier = 2f;
    [Tooltip("Sprunggeschwindigkeit")]
    public float jumpSpeed = 3f;
    [Tooltip("Schwerkraft (positiv nach unten)")]
    public float gravity = 9.81f;

    [Header("Rotation")]
    [Tooltip("Drehgeschwindigkeit (°/s) beim Blicken mit rechtem Stick")]
    public float rotationSpeed = 60f;

    private CharacterController cc;
    private Transform head;
    private float verticalVelocity = 0f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        head = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // 1) Rotation um Y-Achse mit rechtem Stick
        Vector2 rotInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        transform.Rotate(0f, rotInput.x * rotationSpeed * Time.deltaTime, 0f);

        // 2) Horizontal-Move mit linkem Stick
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        // Blickrichtung (Y-Rotation des Körpers angewandt)
        Vector3 forward = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right   = Vector3.ProjectOnPlane(head.right,   Vector3.up).normalized;
        Vector3 horizontal = forward * moveInput.y + right * moveInput.x;

        // 3) Sprinten, wenn man den linken Stick klickt
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            horizontal *= sprintMultiplier;

        // 4) Springen
        if (cc.isGrounded)
    {
    verticalVelocity = -0.1f;
    if (OVRInput.GetDown(OVRInput.Button.Two)) // Springen auf B-Button
        verticalVelocity = jumpSpeed;
    }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 5) Gesamte Bewegung
        Vector3 motion = horizontal * moveSpeed + Vector3.up * verticalVelocity;
        cc.Move(motion * Time.deltaTime);
    }
}
