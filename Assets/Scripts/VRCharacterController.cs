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
        cc   = GetComponent<CharacterController>();
        head = GetComponentInChildren<Camera>().transform;
    }

    void Update()
    {
        // 1) Yaw-Rotation über rechten Stick
        Vector2 rotInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        float yaw = rotInput.x;
        transform.Rotate(0f, yaw * rotationSpeed * Time.deltaTime, 0f);

        // 2) Bewegung über linken Stick (relativ zur Kopfrichtung)
        Vector2 moveInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Vector3 forward  = Vector3.ProjectOnPlane(head.forward, Vector3.up).normalized;
        Vector3 right    = Vector3.ProjectOnPlane(head.right,   Vector3.up).normalized;
        Vector3 horizontal = forward * moveInput.y + right * moveInput.x;

        // 3) Sprint per Stick-Click
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            horizontal *= sprintMultiplier;

        // 4) Springen per B-Button
        if (cc.isGrounded)
        {
            verticalVelocity = -0.1f;
            if (OVRInput.GetDown(OVRInput.Button.Two))
                verticalVelocity = jumpSpeed;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // 5) Final Move
        Vector3 motion = horizontal * moveSpeed + Vector3.up * verticalVelocity;
        cc.Move(motion * Time.deltaTime);
    }
}
